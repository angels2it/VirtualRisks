using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Games.Events;
using MvvmCross.Core.ViewModels;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class EventsViewRequest
    {
        public List<EventBaseModel> Items { get; set; }
    }

    public abstract class MvxViewModelBase<T> : MvxViewModelBase, IMvxViewModel<T>
    {
        public abstract void Prepare(T parameter);
    }
    public class EventsViewModel : MvxViewModelBase<EventsViewRequest>
    {
        public MvxObservableCollection<EventBaseModel> Items { get; set; } = new MvxObservableCollection<EventBaseModel>();

        public override void Prepare(EventsViewRequest parameter)
        {
            if(parameter.Items == null)
                return;
            foreach (var item in parameter.Items)
            {
                Items.Add(item);
            }
        }
    }
}
