using System;
using System.Collections.Generic;

namespace CastleGo.Domain.Events
{
    public static class EventConfig
    {
        public static List<Type> PlayableEvents = new List<Type>()
        {
            typeof(BattalionMovementEvent),
            typeof(OccupiedCastleEvent),
            typeof(CastleHasBeenOccupiedEvent),
            typeof(DefendedCastleEvent),
            typeof(FailedAttackCastleEvent),
            typeof(OccupiedSiegeInCastleEvent),
            typeof(FailedAttackSiegeEvent),
            typeof(DefendedSiegeEvent),
            typeof(SiegeHasBeenOccupiedEvent),

        };
    }
}
