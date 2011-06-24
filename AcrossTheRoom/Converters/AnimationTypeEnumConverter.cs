using System;
using System.Windows.Media;
using System.Windows.Data;

namespace AcrossTheRoom
{
    public class AnimationTypeEnumConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Default to Scrolling
            if (null == value)
            {
                return "Scrolling";
            }

            if (value is AnimationType)
            {
                    string s = ((Enum)value).ToString();
                    if (s == "SlideDown")
                        return "Slide Down";
                    else
                        return s;
            }

            //Type type = value.GetType();            
            //throw new InvalidOperationException("Unsupported type ["+type.Name+"]");                    
            return "Scrolling";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (AnimationType)Enum.Parse(typeof(AnimationType), value.ToString().Replace(" ", ""), true);
        }
    }
}
