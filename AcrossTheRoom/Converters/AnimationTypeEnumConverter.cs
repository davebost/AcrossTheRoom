using System;
using System.Windows.Media;
using System.Windows.Data;

namespace AcrossTheRoom
{
    public class AnimationTypeEnumConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                return null;
            }

            if (value is AnimationType)
            {
                return ((Enum)value).ToString();
            }

            //Type type = value.GetType();            
            //throw new InvalidOperationException("Unsupported type ["+type.Name+"]");                    
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (AnimationType)Enum.Parse(typeof(AnimationType), value.ToString(), true);
        }
    }
}
