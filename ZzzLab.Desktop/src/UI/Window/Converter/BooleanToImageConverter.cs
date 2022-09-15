namespace System.Windows.Data
{
    public class BooleanToImageConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => value is bool boolean && boolean ? ImageSourceTrue : ImageSourceFalse;

        public string ImageSourceTrue { get; set; } = "";

        public string ImageSourceFalse { get; set; } = "";
    }
}