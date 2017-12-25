using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Shared.Common;

namespace CastleGo.Application.Games.Dtos
{
    public class ChangeTroopTypeDto
    {
        public string Id { get; set; }
        public string CastleId { get; set; }
        public string TroopType { get; set; }
    }
}
