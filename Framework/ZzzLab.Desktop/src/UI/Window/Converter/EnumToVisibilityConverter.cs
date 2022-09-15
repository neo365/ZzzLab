using ZzzLab;

namespace System.Windows.Data
{
    public class EnumToVisibilityConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visible = false;

            if (value is string str)
            {
                string[] ValueArr = str.Split('|');

                foreach (string v in ValueArr)
                {
                    if (str.EqualsIgnoreCase(v))
                    {
                        visible = !InvertVisibility;
                        break;
                    }
                }
            }

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value is Visibility visibility && visibility == Visibility.Visible) ? visibility : Visibility.Collapsed;
        }

        public string Value { set; get; } = string.Empty;
        public bool InvertVisibility { get; set; } = false;
    }
}