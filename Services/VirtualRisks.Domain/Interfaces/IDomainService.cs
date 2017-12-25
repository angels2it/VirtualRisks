using System;
using System.Collections.Generic;
using CastleGo.Domain.Bases;
using NEventStore;

namespace CastleGo.Domain.Interfaces
{
    public interface IDomainService
    {
        void AddEvent(Guid streamId, params EventBase[] @events);

        EventBase GetLatestEvent(Guid id);

        void Build<T>(Guid id, Func<Guid, T> initSnap, params Action<T>[] relateBuildsAction) where T : Aggregate;

        T GetLatestEvent<T>(Guid id) where T : EventBase;
        T GetLatestEvent<T>(Guid id, Func<T, bool> expression) where T : EventBase;
        List<T> GetNotExecuteEvents<T>(Guid id) where T : EventBase;
        bool IsEventExecuted<T>(Guid streamId, int streamVersion, Guid eventId) where T : EventBase;
        List<EventBase> GetEvents(Guid id, string userId, int from);
        ISnapshot GetGameSnapshot(Guid gameId);
    }
}
