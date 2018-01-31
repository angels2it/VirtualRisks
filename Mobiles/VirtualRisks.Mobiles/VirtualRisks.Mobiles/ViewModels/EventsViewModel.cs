using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Games.Events;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using VirtualRisks.Mobiles.Messages;

namespace VirtualRisks.Mobiles.ViewModels
{
    public abstract class MvxViewModelBase<T> : MvxViewModelBase, IMvxViewModel<T>
    {
        public abstract void Prepare(T parameter);
    }

    public class EventsViewRequest
    {
        public List<EventModel> Events { get; }

        public EventsViewRequest(List<EventModel> events)
        {
            Events = events;
        }
    }
    public class EventsViewModel : MvxViewModelBase<EventsViewRequest>
    {
        public MvxObservableCollection<EventModel> Items { get; set; } = new MvxObservableCollection<EventModel>();

        public EventsViewModel(IMvxMessenger messenger)
        {
        }

        public override void Prepare(EventsViewRequest parameter)
        {
            Items.Clear();
            if(parameter.Events == null)
                return;
            foreach (var @event in parameter.Events)
            {
                Items.Add(@event);
            }
        }
    }
}
