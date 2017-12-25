using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Common
{
    public static class Constant
    {
#if DEBUG
        public static double BattalionMovementSpeed = 100 * 1.0 / 60; // m/s
#else
        public static double BattalionMovementSpeed = 10 * 1.0 / 60; // m/s
#endif
    }
}
