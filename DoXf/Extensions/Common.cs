using System;
using Xamarin.Essentials;

namespace DoXf.Extensions
{
    public static class Common
    {
        public static T ToPixels<T>(this T db)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var density = mainDisplayInfo.Density;

            return (T)Convert.ChangeType(Convert.ToDouble(db) * density, typeof(T));
        }

        public static T ToDp<T>(this T pixels)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var density = mainDisplayInfo.Density;

            return (T)Convert.ChangeType(Convert.ToDouble(pixels) / density, typeof(T));
        }
    }
}
