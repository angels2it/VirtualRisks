using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRisks.Mobiles.Helpers
{
    public static class Utils
    {
        public static bool IsUriIcon(string icon)
        {
            return !string.IsNullOrEmpty(icon) && (icon.StartsWith("http") || icon.StartsWith("https"));
        }
    }
}
