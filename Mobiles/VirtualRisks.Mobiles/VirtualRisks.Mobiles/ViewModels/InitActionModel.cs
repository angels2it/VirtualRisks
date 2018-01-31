using System;
using System.Threading;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class InitActionModel
    {
        public ActionType Type { get; set; }
        public TimeSpan Interval { get; set; }
        public Func<string> UpdateAction { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
    }
}