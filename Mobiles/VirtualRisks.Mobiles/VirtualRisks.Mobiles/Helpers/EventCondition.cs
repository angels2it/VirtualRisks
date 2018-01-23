using System;
using CastleGo.Shared.Games.Events;

namespace VirtualRisks.Mobiles.Helpers
{
    public class EventCondition<T> : ICondition<T> where T : EventBaseModel
    {
        private readonly Action<T> _handle;
        public EventCondition(Action<T> handle)
        {
            _handle = handle;
        }
        public bool IsTrue(EventBaseModel model)
        {
            var name = typeof(T).Name;
            return model.GetType().Name == name;
        }

        public void Handle(EventBaseModel model)
        {
            _handle(model as T);
        }
    }
}