using System;
using System.Collections.Generic;
using CastleGo.Shared.Common;

namespace CastleGo.Shared.Games.Events
{
    public class EventBaseModel
    {
        public Guid Id { get; set; }
        public DateTime RunningAt { get; set; }
        public DateTime ExecuteAt { get; set; }
#if MOBILE
        public string Type { get; set; }
#else
        public string Type => GetType().FullName;
#endif

        public string GetEventName()
        {
            return GetType().Name.Replace("Model", "");
        }
    }
}
