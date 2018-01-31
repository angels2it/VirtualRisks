namespace VirtualRisks.Mobiles.ViewModels
{
    public interface IGamePageViewResource
    {
        /// <summary>
        /// {0} - source castle name
        /// {1} - destination castle name
        /// </summary>
        string ConfirmBattalionMessage { get; }

        string LoadingBattalion { get; }
        string StatusBuildGame { get; }
        string Move { get; }
        string Regret { get; }
        string MoveSoldier { get; }
        /// <summary>
        /// {0} - selected soldiers
        /// </summary>
        string MoveSoldiers { get; }

        string BattalionError { get; }
        string BattalionSuccess { get; }
        string GameStateError { get; }
        string GameEnded { get; }
        string MyHero { get; }
        string OpponentHero { get; }
        /// <summary>
        /// {0} time formatted
        /// </summary>
        string OpponentHeroWithTime { get; }

        string YourCastleHasInit { get; }
        string NeutralCastleHasInit { get; }
        string OpponentCastleHasInit { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string SiegeEventText { get; }

        /// <summary>
        /// {0} - user name
        /// </summary>
        string BattleVsSiegeEventText { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string AddMoreSoldiersToSiegeEventText { get; }
        /// <summary>
        /// {0} - user name
        /// {1} - castle name
        /// </summary>
        string AddMoreSoldiersToCastleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string BattleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string CastleHasBeenOccupiedEventText { get; }

        string FailedAttackCastleEventText { get; }
        string DefendedCastleEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string OccupiedCastleEventText { get; }

        /// <summary>
        /// {0} - castle name
        /// </summary>
        string SiegeHasBeenOccupiedEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string DefendedSiegeEventText { get; }
        /// <summary>
        /// {0} castle name
        /// </summary>
        string FailedAttackSiegeEventText { get; }
        /// <summary>
        /// {0} - castle name
        /// </summary>
        string OccupiedSiegeEventText { get; }

        /// <summary>
        /// {0} - distance text
        /// {1} - time text
        /// </summary>
        string RouteInfoText { get; }
    }
}