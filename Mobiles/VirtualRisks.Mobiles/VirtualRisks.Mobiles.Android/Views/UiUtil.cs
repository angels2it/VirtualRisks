using Android.Content.Res;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Java.Lang;

namespace VirtualRisks.Mobiles.Droid.Views
{
    public class UiUtil
    {

        public static BitmapDescriptor drawableToBitmapDescriptor(Drawable drawable)
        {
            Bitmap bitmap;

            if (drawable is BitmapDrawable)
            {
                BitmapDrawable bitmapDrawable = (BitmapDrawable)drawable;
                if (bitmapDrawable.Bitmap != null)
                {
                    return BitmapDescriptorFactory.FromBitmap(bitmapDrawable.Bitmap);
                }
            }
            if (drawable.IntrinsicWidth <= 0 || drawable.IntrinsicHeight <= 0)
            {
                bitmap = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888); // Single color bitmap will be created of 1x1 pixel
            }
            else
            {
                bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
            }

            Canvas canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
            drawable.Draw(canvas);
            return BitmapDescriptorFactory.FromBitmap(bitmap);
        }

        public static int dpToPx(float dp)
        {
            float density = Resources.System.DisplayMetrics.Density;
            return (int)Math.Ceil(dp * density);
        }
    }
}