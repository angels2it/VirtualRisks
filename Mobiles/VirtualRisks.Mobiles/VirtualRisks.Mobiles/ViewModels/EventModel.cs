using System;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class EventModel
    {
        public string TimeText { get; set; }
        public string Text { get; set; }
        public bool HasAction { get; set; }
        public Action Action { get; set; }
        public object ActionParamater { get; set; }
        public InitActionModel InitAction { get; set; }
        public string Type { get; set; }

        public EventModel(string type, DateTime time, string text)
        {
            Type = type;
            time = time.ToLocalTime();
            if (time.Day != DateTime.Now.Day)
                TimeText = time.ToString("DD-MM hh:mm");
            else
                TimeText = time.ToString("hh:mm");
            Text = text;
        }
    }
}