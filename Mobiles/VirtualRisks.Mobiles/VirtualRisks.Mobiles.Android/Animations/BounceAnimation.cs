﻿using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views.Animations;
using Java.Lang;

namespace VirtualRisks.Mobiles.Droid.Animations
{
    [Register("virtualrisks.mobiles.droid.views.animations.BounceAnimation")]
    public class BounceAnimation : Java.Lang.Object, IRunnable
    {
        private long mStart, mDuration;
        private IInterpolator mInterpolator;
        private Marker mMarker;
        private Handler mHandler;

        public BounceAnimation(Marker marker, Handler handler)
        {
            mMarker = marker;
            mHandler = handler;
            mInterpolator = new BounceInterpolator();
            mStart = SystemClock.UptimeMillis();
            mDuration = 1500;
        }

        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - mStart;
            float t = Math.Max(
                1 - mInterpolator.GetInterpolation((float)elapsed
                                                  / mDuration), 0);
            mMarker.SetAnchor(0.5f, 1.0f + 2 * t);

            if (t > 0.0)
            {
                // Post again 16ms later.
                mHandler.PostDelayed(this, 16);
            }
            else
            {
                mStart = SystemClock.UptimeMillis();
                mHandler.Post(this);
            }

            //long elapsed = SystemClock.UptimeMillis() - mStart;
            //float t = System.Math.Max(1 - mInterpolator.GetInterpolation((float)elapsed / mDuration), 0f);
            //mMarker.SetAnchor(0.5f, 1f + 0.5f * t);
            //if (t > 0.0)
            //{
            //    // Post again 16ms later.
            //    mHandler.PostDelayed(this, 16L);
            //}
            //else
            //{
            //    mStart = SystemClock.UptimeMillis();
            //    mHandler.Post(this);
            //}
        }
    }
}

