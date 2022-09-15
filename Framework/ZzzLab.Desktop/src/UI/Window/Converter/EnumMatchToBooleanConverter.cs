using System.Globalization;

namespace System.Windows.Data
{
    public class EnumMatchToBooleanConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string checkValue || parameter is not string targetValue) return false;

            if (checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase)) return !InvertBoolean;

            return InvertBoolean;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool useValue || parameter is not string targetValue) return "";

            if (useValue) return Enum.Parse(targetType, targetValue);

            return "";
        }

        public bool InvertBoolean { get; set; } = false;
    }
}