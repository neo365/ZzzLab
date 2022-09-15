using ZzzLab;

namespace System.Windows.Data
{
    public class EnumToBooleanConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                string[] ValueArr = str.Split('|');

                foreach (string v in ValueArr)
                {
                    if (str.EqualsIgnoreCase(v))
                    {
                        return !InvertBoolean;
                    }
                }
            }

            return InvertBoolean;
        }

        public string Value { get; set; } = string.Empty;
        public bool InvertBoolean { get; set; } = false;
    }
}