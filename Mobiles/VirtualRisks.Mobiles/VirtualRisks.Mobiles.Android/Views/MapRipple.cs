using Android.Animation;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views.Animations;
using Java.Lang;
using Exception = System.Exception;
using Android.Graphics;

namespace VirtualRisks.Mobiles.Droid.Views
{
    /**
 * Created by abhay yadav on 09-Aug-16.
 */
    public class MapRipple
    {
        private GoogleMap mGoogleMap;
        private LatLng _mLatLng, _mPrevLatLng;
        private BitmapDescriptor _mBackgroundImageDescriptor;  //ripple image.
        private float _mTransparency = 0.5f;                   //transparency of image.
        private double _mDistance = 2000;             //distance to which ripple should be shown in metres
        private int _mNumberOfRipples = 1;                     //number of ripples to show, max = 4
        private int _mFillColor = Color.Transparent;           //fill color of circle
        private Color _mStrokeColor = Color.Black;               //border color of circle
        private int _mStrokeWidth = 10;                        //border width of circle
        private long _mDurationBetweenTwoRipples = 4000;       //in microseconds.
        private long _mRippleDuration = 12000;                 //in microseconds
        private ValueAnimator[] mAnimators;
        private Handler[] mHandlers;
        private GroundOverlay[] mGroundOverlays;
        private GradientDrawable mBackground;
        private bool _isAnimationRunning;
        private Runnable _mCircleOneRunnable, _mCircleTwoRunnable, _mCircleThreeRunnable, _mCircleFourRunnable;

        public MapRipple(GoogleMap googleMap, LatLng latLng, Context context)
        {
            mGoogleMap = googleMap;
            _mLatLng = latLng;
            _mPrevLatLng = latLng;
            mBackground = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.background);
            mAnimators = new ValueAnimator[4];
            mHandlers = new Handler[4];
            mGroundOverlays = new GroundOverlay[4];
        }

        /**
         * @param transparency sets transparency for background of circle
         */
        public MapRipple WithTransparency(float transparency)
        {
            _mTransparency = transparency;
            return this;
        }

        /**
         * @param distance sets radius distance for circle
         */
        public MapRipple WithDistance(double distance)
        {
            if (distance < 20)
            {
                distance = 20;
            }
            _mDistance = distance;
            return this;
        }

        /**
         * @param latLng sets position for center of circle
         */
        public MapRipple WithLatLng(LatLng latLng)
        {
            _mPrevLatLng = _mLatLng;
            _mLatLng = latLng;
            return this;
        }

        /**
         * @param numberOfRipples sets count of ripples
         */
        public MapRipple WithNumberOfRipples(int numberOfRipples)
        {
            if (numberOfRipples > 4 || numberOfRipples < 1)
            {
                numberOfRipples = 4;
            }
            _mNumberOfRipples = numberOfRipples;
            return this;
        }

        /**
         * @param fillColor sets fill color
         */
        public MapRipple WithFillColor(int fillColor)
        {
            _mFillColor = fillColor;
            return this;
        }

        /**
         * @param strokeColor sets stroke color
         */
        public MapRipple WithStrokeColor(Color strokeColor)
        {
            _mStrokeColor = strokeColor;
            return this;
        }
        /**
         * @param strokeWidth sets stroke width
         */
        public MapRipple WithStrokeWidth(int strokeWidth)
        {
            _mStrokeWidth = strokeWidth;
            return this;
        }

        /**
         * @param durationBetweenTwoRipples sets duration before pulse tick animation
         */
        public MapRipple WithDurationBetweenTwoRipples(long durationBetweenTwoRipples)
        {
            _mDurationBetweenTwoRipples = durationBetweenTwoRipples;
            return this;
        }

        /**
         * @return current state of animation
         */
        public bool IsAnimationRunning()
        {
            return _isAnimationRunning;
        }

        /**
         * @param rippleDuration sets duration of ripple animation
         */
        public MapRipple WithRippleDuration(long rippleDuration)
        {
            _mRippleDuration = rippleDuration;
            return this;
        }
        
        private void StartAnimation(int numberOfRipple)
        {
            ValueAnimator animator = ValueAnimator.OfInt(0, (int)_mDistance);
            animator.RepeatCount = (ValueAnimator.Infinite);
            animator.RepeatMode = (ValueAnimatorRepeatMode.Restart);
            animator.SetDuration(_mRippleDuration);
            animator.SetEvaluator(new IntEvaluator());
            animator.SetInterpolator(new LinearInterpolator());
            animator.Update += (sender, valueAnimator) =>
            {
                int animated = (int)valueAnimator.Animation.AnimatedValue;
                mGroundOverlays[numberOfRipple].SetDimensions(animated);
                if (_mDistance - animated <= 10)
                {
                    if (_mLatLng != _mPrevLatLng)
                    {
                        mGroundOverlays[numberOfRipple].Position = _mLatLng;
                    }
                }
            };
            animator.Start();
            mAnimators[numberOfRipple] = animator;
        }

        private void SetDrawableAndBitmap()
        {
            mBackground.SetColor(_mFillColor);
            mBackground.SetStroke(UiUtil.dpToPx(_mStrokeWidth), _mStrokeColor);
            _mBackgroundImageDescriptor = UiUtil.drawableToBitmapDescriptor(mBackground);
        }

        /**
         * Stops current animation if it running
         */
        public void StopRippleMapAnimation()
        {
            if (_isAnimationRunning)
            {
                try
                {
                    for (int i = 0; i < _mNumberOfRipples; i++)
                    {
                        if (i == 0)
                        {
                            mHandlers[i].RemoveCallbacks(_mCircleOneRunnable);
                            mAnimators[i].Cancel();
                            mGroundOverlays[i].Remove();
                        }
                        if (i == 1)
                        {
                            mHandlers[i].RemoveCallbacks(_mCircleTwoRunnable);
                            mAnimators[i].Cancel();
                            mGroundOverlays[i].Remove();
                        }
                        if (i == 2)
                        {
                            mHandlers[i].RemoveCallbacks(_mCircleThreeRunnable);
                            mAnimators[i].Cancel();
                            mGroundOverlays[i].Remove();
                        }
                        if (i == 3)
                        {
                            mHandlers[i].RemoveCallbacks(_mCircleFourRunnable);
                            mAnimators[i].Cancel();
                            mGroundOverlays[i].Remove();
                        }
                    }
                }
                catch (Exception)
                {
                    //no need to handle it
                }
            }
            _isAnimationRunning = false;
        }

        /**
         * Starts animations
         */
        public void StartRippleMapAnimation()
        {
            if (!_isAnimationRunning)
            {
                SetDrawableAndBitmap();
                InitCircle();
                for (int i = 0; i < _mNumberOfRipples; i++)
                {
                    if (i == 0)
                    {
                        mHandlers[i] = new Handler();
                        mHandlers[i].PostDelayed(_mCircleOneRunnable, _mDurationBetweenTwoRipples * i);
                    }
                    if (i == 1)
                    {
                        mHandlers[i] = new Handler();
                        mHandlers[i].PostDelayed(_mCircleTwoRunnable, _mDurationBetweenTwoRipples * i);
                    }
                    if (i == 2)
                    {
                        mHandlers[i] = new Handler();
                        mHandlers[i].PostDelayed(_mCircleThreeRunnable, _mDurationBetweenTwoRipples * i);
                    }
                    if (i == 3)
                    {
                        mHandlers[i] = new Handler();
                        mHandlers[i].PostDelayed(_mCircleFourRunnable, _mDurationBetweenTwoRipples * i);
                    }
                }
            }
            _isAnimationRunning = true;
        }

        
        private void InitCircle()
        {
            _mCircleOneRunnable = new Runnable(() =>
            {
                mGroundOverlays[0] = mGoogleMap.AddGroundOverlay(new GroundOverlayOptions()
                    .Position(_mLatLng, (int)_mDistance)
                    .InvokeTransparency(_mTransparency)
                    .InvokeImage(_mBackgroundImageDescriptor));
                StartAnimation(0);
            });
            _mCircleTwoRunnable = new Runnable(() =>
            {
                mGroundOverlays[1] = mGoogleMap.AddGroundOverlay(new GroundOverlayOptions()
                    .Position(_mLatLng, (int)_mDistance)
                    .InvokeTransparency(_mTransparency)
                    .InvokeImage(_mBackgroundImageDescriptor));
                StartAnimation(1);
            });
            _mCircleThreeRunnable = new Runnable(() =>
            {
                mGroundOverlays[2] = mGoogleMap.AddGroundOverlay(new GroundOverlayOptions()
                    .Position(_mLatLng, (int)_mDistance)
                    .InvokeTransparency(_mTransparency)
                    .InvokeImage(_mBackgroundImageDescriptor));
                StartAnimation(2);
            });
            _mCircleFourRunnable = new Runnable(() =>
            {
                mGroundOverlays[3] = mGoogleMap.AddGroundOverlay(new GroundOverlayOptions()
                    .Position(_mLatLng, (int)_mDistance)
                    .InvokeTransparency(_mTransparency)
                    .InvokeImage(_mBackgroundImageDescriptor));
                StartAnimation(3);
            });
        }
    }
}