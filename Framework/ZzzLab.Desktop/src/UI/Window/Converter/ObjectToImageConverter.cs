namespace System.Windows.Data
{
    public class ObjectToImageConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? ImageSourceTrue : ImageSourceFalse;
        }

        public string ImageSourceTrue { get; set; } = "";

        public string ImageSourceFalse { get; set; } = "";
    }
}