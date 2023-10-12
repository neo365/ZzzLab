namespace System.Windows.Data
{
    public class ObjectToVisibilityConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visible = (value != null) ? !InvertVisibility : InvertVisibility;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => (value is Visibility visibility && visibility == Visibility.Visible);


        public bool InvertVisibility { get; set; } = false;
    }
}