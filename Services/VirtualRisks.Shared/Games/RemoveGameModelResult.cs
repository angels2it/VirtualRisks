using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CastleGo.Shared.Games
{
    public class RemoveGameModelResult
    {
        public string Id { get; set; }
    }

    public class UpgradeCastleResult
    {
        public string Id { get; set; }
        public double Strength { get; set; }
        public bool HasError => Errors?.Any() ?? false;
        public List<string> Errors { get; set; }
    }
}
