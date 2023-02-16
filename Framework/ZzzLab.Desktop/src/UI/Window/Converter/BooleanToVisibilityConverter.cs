namespace System.Windows.Data
{
    public class BooleanToVisibilityConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                var visible = System.Convert.ToBoolean(value, culture);
                if (InvertVisibility)
                {
                    visible = !visible;
                }

                return visible ? Visibility.Visible : Visibility.Collapsed;
            }

            throw new InvalidOperationException("Converter can only convert to value of type Visibility.");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible ? true : (object)false;
        }

        public bool InvertVisibility { get; set; } = false;
    }
}