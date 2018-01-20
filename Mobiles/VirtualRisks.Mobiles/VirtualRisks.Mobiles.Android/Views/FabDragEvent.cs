using System;

namespace VirtualRisks.Mobiles.Droid.Views
{
    public class FabDragEvent : EventArgs
    {
        public FabDragEvent(float upRawX, float upRawY)
        {
            X = upRawX;
            Y = upRawY;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}