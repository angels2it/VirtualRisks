namespace CastleGo.Shared.Games.Events
{
    public abstract class AttackEventModel : EventBaseModel
    {
        public BattleLogModel BattleLog { get; set; }
    }
}