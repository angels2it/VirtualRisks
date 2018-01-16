using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;

namespace VirtualRisks.Mobiles.Droid.Views
{
    public class FabDragEnd : EventArgs
    {
        public FabDragEnd(float upRawX, float upRawY)
        {
            X = upRawX;
            Y = upRawY;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
    public class MovableFloatingActionButton : FloatingActionButton, View.IOnTouchListener
    {

        private static float CLICK_DRAG_TOLERANCE = 10; // Often, there will be a slight, unintentional, drag when the user taps the FAB, so we need to account for this.

        private float downRawX, downRawY;
        private float dX, dY;
        protected MovableFloatingActionButton(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            init();
        }

        public event EventHandler<FabDragEnd> DragEnd;
        public MovableFloatingActionButton(Context context) : base(context)
        {
            init();
        }

        public MovableFloatingActionButton(Context context, IAttributeSet attributeSet) : base(context, attributeSet)
        {
            init();
        }

        public MovableFloatingActionButton(Context context, IAttributeSet attributeSet, int defStyle) : base(context, attributeSet, defStyle)
        {
            init();
        }

        private void init()
        {
            SetOnTouchListener(this);
        }

        public bool OnTouch(View view, MotionEvent motionEvent)
        {
            var action = motionEvent.Action;
            if (action == MotionEventActions.Down)
            {
                var moveView = (View)view.Parent;
                downRawX = motionEvent.RawX;
                downRawY = motionEvent.RawY;
                dX = moveView.GetX() - downRawX;
                dY = moveView.GetY() - downRawY;

                return true; // Consumed

            }
            else if (action == MotionEventActions.Move)
            {
                var moveView = (View)view.Parent;
                int viewWidth = moveView.Width;
                int viewHeight = moveView.Height;

                View viewParent = (View)moveView.Parent;
                int parentWidth = viewParent.Width;
                int parentHeight = viewParent.Height;

                float newX = motionEvent.RawX + dX;
                newX = Java.Lang.Math.Max(0, newX); // Don't allow the FAB past the left hand side of the parent
                newX = Java.Lang.Math.Min(parentWidth - viewWidth, newX); // Don't allow the FAB past the right hand side of the parent

                float newY = motionEvent.RawY + dY;
                newY = Java.Lang.Math.Max(0, newY); // Don't allow the FAB past the top of the parent
                newY = Java.Lang.Math.Min(parentHeight - viewHeight, newY); // Don't allow the FAB past the bottom of the parent

                moveView.Animate()
                        .X(newX)
                        .Y(newY)
                        .SetDuration(0)
                        .Start();

                return true; // Consumed

            }
            else if (action == MotionEventActions.Up)
            {

                float upRawX = motionEvent.RawX;
                float upRawY = motionEvent.RawY;

                float upDX = upRawX - downRawX;
                float upDY = upRawY - downRawY;

                if (Java.Lang.Math.Abs(upDX) < CLICK_DRAG_TOLERANCE && Java.Lang.Math.Abs(upDY) < CLICK_DRAG_TOLERANCE)
                { // A click
                    return base.PerformClick();
                }
                else
                { // A drag
                    var moveView = (View)view.Parent;
                    View viewParent = (View)moveView.Parent;
                    moveView.Animate()
                        .X(viewParent.Width - moveView.Width - 10)
                        .Y(viewParent.Height - moveView.Height - 10)
                        .SetDuration(1000)
                        .Start();
                    DragEnd?.Invoke(this, new FabDragEnd(upRawX, upRawY));
                    return true; // Consumed
                }

            }
            else
            {
                return base.OnTouchEvent(motionEvent);
            }
        }
    }
}