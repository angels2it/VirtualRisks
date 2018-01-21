using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class MoveSoldierEventHandler : IDomainEventHandler<MoveSoldierEvent>
    {
        public bool Handle(DomainEventHandlerData<MoveSoldierEvent> @event)
        {
            GameAggregate snapshot = @event.Snapshot as GameAggregate;
            if (snapshot == null)
                return false;
            if (@event.EventObject.Soldiers == null)
                return true;
            var army = snapshot.UserId == @event.EventObject.CreatedBy ? Army.Blue : Army.Red;
            var soldiers = army == Army.Blue ? snapshot.UserSoldiers : snapshot.OpponentSoldiers ?? new List<SoldierAggregate>();
            var moveSoldiers = soldiers.Where(e => @event.EventObject.Soldiers.Contains(e.Id.ToString()));
            var moveToCastle = snapshot.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (moveToCastle == null)
                return false;
            if(moveToCastle.Soldiers == null)
                moveToCastle.Soldiers = new List<SoldierAggregate>();
            moveToCastle.Soldiers.AddRange(moveSoldiers);
            soldiers.RemoveAll(e => @event.EventObject.Soldiers.Contains(e.Id.ToString()));
            snapshot.UpdateSoldierAmount();
            return true;
        }
    }
}
