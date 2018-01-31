using VirtualRisks.Mobiles.Resources;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class GamePageViewResource : IGamePageViewResource
    {
        public string ConfirmBattalionMessage => AppResource.confirm_battalion_message;
        public string LoadingBattalion => AppResource.loading_battalion;
        public string StatusBuildGame => AppResource.status_buildgame;
        public string Move => AppResource.bt_move;
        public string Regret => AppResource.bt_regret;
        public string MoveSoldier => AppResource.lb_movesoldier;
        public string MoveSoldiers => AppResource.lb_movesoldiers;
        public string BattalionError => AppResource.error_battalion;
        public string BattalionSuccess => AppResource.success_battalion;
        public string GameStateError => AppResource.error_gamestate;
        public string GameEnded => AppResource.alert_gameended;
        public string MyHero => AppResource.myhero;
        public string OpponentHero => AppResource.opponent_hero;
        public string OpponentHeroWithTime => AppResource.opponent_hero_with_time;
        public string YourCastleHasInit => AppResource.lb_castleinit_user;
        public string NeutralCastleHasInit => AppResource.lb_castleinit_neutral;
        public string OpponentCastleHasInit => AppResource.lb_castleinit_opponent;
        public string SiegeEventText => AppResource.event_siegehassetup;
        public string BattleVsSiegeEventText => AppResource.event_battlevssiege;
        public string AddMoreSoldiersToSiegeEventText => AppResource.event_addmoresoldiertosiege;
        public string AddMoreSoldiersToCastleEventText => AppResource.event_addmoresoldiertocastle;
        public string BattleEventText => AppResource.event_battlebegun;
        public string CastleHasBeenOccupiedEventText => AppResource.event_castlehasbeenoccupied;
        public string FailedAttackCastleEventText => AppResource.event_failedattackcastle;
        public string DefendedCastleEventText => AppResource.event_defendedcastle;
        public string OccupiedCastleEventText => AppResource.event_occupiedcastle;
        public string SiegeHasBeenOccupiedEventText => AppResource.event_siegehasbeenoccupied;
        public string DefendedSiegeEventText => AppResource.event_defendedsiege;
        public string FailedAttackSiegeEventText => AppResource.event_failedattacksiege;
        public string OccupiedSiegeEventText => AppResource.event_occupiedsiege;
        public string RouteInfoText => AppResource.label_battalionrouteinfo;
    }
}