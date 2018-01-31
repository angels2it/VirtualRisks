using MvvmCross.Plugins.Messenger;
using VirtualRisks.Mobiles.ViewModels;

namespace VirtualRisks.Mobiles.Messages
{
    public class AddEventMessage : MvxMessage
    {
        public AddEventMessage(object sender, EventModel @event) : base(sender)
        {
            Event = @event;
        }

        public EventModel Event { get; private set; }
    }
}
