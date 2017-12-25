using CastleGo.Domain.Bases;
using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CastleGo.Domain.Interfaces;
using Medallion.Threading;

namespace CastleGo.Domain
{
    public class DomainService : IDomainService
    {
        private readonly IStoreEvents _store;

        public DomainService(IStoreEvents store)
        {
            _store = store;
        }

        public EventBase GetLatestEvent(Guid id)
        {
            using (IEventStream eventStream = _store.OpenStream(id))
                return eventStream.CommittedEvents.Select(e => e.Body).Cast<EventBase>().OrderByDescending(e => e.ExecuteAt).FirstOrDefault();
        }

        public void AddEvent(Guid streamId, params EventBase[] @events)
        {
            using (IEventStream eventStream = _store.OpenStream(streamId))
            {
                foreach (var @event in events)
                {
                    eventStream.Add(new EventMessage()
                    {
                        Body = @event
                    });
                }
                eventStream.CommitChanges(Guid.NewGuid());
            }
        }

        public bool IsEventExecuted<T>(Guid streamId, int streamVersion, Guid eventId) where T : EventBase
        {
            using (IEventStream eventStream = _store.OpenStream(streamId, streamVersion))
            {
                var events = eventStream.CommittedEvents.Where(e =>
                {
                    if (e.Body != null)
                        return e.Body.GetType() == typeof(T);
                    return false;
                }).Select(e => e.Body as T)
                .OrderBy(e => e.ExecuteAt)
                .GroupBy(e => e.Id)
                .Select(e => e.Last()).ToList();
                var @event = @events.FirstOrDefault(e => e.Id == eventId);
                if (@event == null)
                    return true;
                return @event.ExecuteAt.CompareTo(DateTime.UtcNow) < 0;
            }
        }
        public void Build<T>(Guid id, Func<Guid, T> initSnap, params Action<T>[] relateBuildsAction) where T : Aggregate
        {
            var myLock = new SystemDistributedLock(SystemDistributedLock.GetSafeLockName($"Game{id}"));
            using (myLock.Acquire())
            {
                PrivateBuild(id, initSnap, relateBuildsAction);
            }
        }

        private void PrivateBuild<T>(Guid id, Func<Guid, T> initSnap, Action<T>[] relateBuildsAction) where T : Aggregate
        {
            ISnapshot latestSnap = _store.Advanced.GetSnapshot(id, int.MaxValue);
            T snapData = latestSnap?.Payload as T ?? initSnap(id);
            if (relateBuildsAction != null)
                foreach (Action<T> action in relateBuildsAction)
                    action(snapData);
            using (IEventStream eventStream = _store.OpenStream(id, latestSnap?.StreamRevision ?? 0))
            {
                if (eventStream.StreamRevision == 0)
                    return;
                int version = latestSnap?.StreamRevision ?? 0;
                int unBuild = eventStream.StreamRevision - (latestSnap?.StreamRevision ?? 0);
                int start = latestSnap == null ? 0 : 1;
                List<EventBase> source = new List<EventBase>();
                for (int index = start; index < unBuild + start; ++index)
                {
                    EventBase body = eventStream.CommittedEvents.ElementAt(index).Body as EventBase;
                    if (body != null)
                        source.Add(body);
                }
                var orderedEvents = source.OrderBy(e => e.ExecuteAt).ToList();
                List<EventBase> notExecuteEvents = new List<EventBase>();
                bool flag = false;
                foreach (EventBase eventBase in orderedEvents)
                {
                    if (eventBase.ExecuteAt.CompareTo(DateTime.UtcNow) > 0)
                    {
                        notExecuteEvents.Add(eventBase);
                    }
                    else
                    {
                        flag = true;
                        EventHandler.Publish(new DomainEventHandlerData<EventBase>()
                        {
                            Store = _store,
                            Event = eventBase,
                            Snapshot = snapData,
                            Id = id
                        });
                    }
                }
                if (!flag)
                    return;
                _store.Advanced.AddSnapshot(new Snapshot(id.ToString(), version + source.Count, snapData));
                if (notExecuteEvents.Count > 0)
                    AddEvent(id, notExecuteEvents.ToArray());
                PrivateBuild(id, initSnap, relateBuildsAction);
            }
        }

        public T GetLatestEvent<T>(Guid id) where T : EventBase
        {
            using (IEventStream eventStream = _store.OpenStream(id))
                return eventStream.CommittedEvents.Where(e =>
                {
                    if (e.Body != null)
                        return e.Body.GetType() == typeof(T);
                    return false;
                }).Select(e => e.Body as T).OrderByDescending(e => e.ExecuteAt).FirstOrDefault();
        }

        public T GetLatestEvent<T>(Guid id, Func<T, bool> expression) where T : EventBase
        {
            using (IEventStream eventStream = _store.OpenStream(id))
                return eventStream.CommittedEvents.Where(e =>
                {
                    if (e.Body != null)
                        return e.Body.GetType() == typeof(T) && expression((T)e.Body);
                    return false;
                }).Select(e => (T)e.Body).OrderByDescending(e => e.ExecuteAt).FirstOrDefault();
        }

        public List<T> GetNotExecuteEvents<T>(Guid id) where T : EventBase
        {
            var snap = _store.Advanced.GetSnapshot(id, int.MaxValue);
            using (IEventStream eventStream = _store.OpenStream(id, snap?.StreamRevision ?? 0))
                return eventStream.CommittedEvents.Skip(snap != null ? 1 : 0).Where(e =>
                        {
                            if (e.Body != null)
                                return e.Body.GetType() == typeof(T);
                            return false;
                        }).Select(e => e.Body as T)
                .OrderBy(e => e.ExecuteAt)
                .GroupBy(e => e.Id)
                .Select(e => e.Last()).ToList();
        }

        public List<EventBase> GetEvents(Guid id, string userId, int @from)
        {
            var snap = _store.Advanced.GetSnapshot(id, int.MaxValue);
            if (snap == null || snap.StreamRevision < @from)
                return new List<EventBase>();
            using (IEventStream stream = _store.OpenStream(id, @from))
            {
                var total = stream.StreamRevision;
                var events = stream.CommittedEvents.Select(e => e.Body as EventBase).ToList();
                var futureEventsCount = total - snap.StreamRevision;
                var futureEvents = events.GetRange(events.Count - futureEventsCount, futureEventsCount);
                var unknowEventTo = snap.StreamRevision - from;
                if (unknowEventTo < 0)
                    unknowEventTo = 0;
                return events
                    .GetRange(0, unknowEventTo)
                    .Where(e => string.IsNullOrEmpty(e.CreatedBy) || e.CreatedBy == userId)
                    .Where(e => futureEvents.All(f => f.Id != e.Id))
                    .OrderBy(e => e.ExecuteAt)
                    .GroupBy(e => e.Id)
                    .Select(e => e.Last())
                    .ToList();
            }
        }

        public ISnapshot GetGameSnapshot(Guid gameId)
        {
            return _store.Advanced.GetSnapshot(gameId, int.MaxValue);
        }
    }
}
