
using CastleGo.Shared.Games.Events;

namespace VirtualRisks.Mobiles.Helpers
{
    public interface ICondition<out T> where T : class
    {
        bool IsTrue(EventBaseModel model);
        void Handle(EventBaseModel model);
    }
}