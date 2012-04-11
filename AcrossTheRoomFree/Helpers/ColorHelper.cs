using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace AcrossTheRoom
{
    public class ColorHelper
    {
        public static Color GetColorFromString(string colorString)
        {
            Type colorType = (typeof(System.Windows.Media.Colors));
            Color c = System.Windows.Media.Colors.Black;

            if (colorType.GetProperty(colorString) != null)
            {
                object o = colorType.InvokeMember(colorString, BindingFlags.GetProperty, null, null, null);
                if (o != null)
                {
                    c = (Color)o;
                }
            }

            return c;
        }

        public static Brush GetBrushFromString(string colorText)
        {
            Color c = GetColorFromString(colorText);
            return new SolidColorBrush(c);
        }
    }
}
