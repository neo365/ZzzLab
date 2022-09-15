namespace System.Windows.Data
{
    public class ObjectToBooleanConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return !InvertBoolean;
            }

            return InvertBoolean;
        }

        public bool InvertBoolean { get; set; } = false;
    }
}