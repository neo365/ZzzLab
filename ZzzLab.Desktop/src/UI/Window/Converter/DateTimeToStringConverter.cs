namespace System.Windows.Data
{
    public class DateTimeToStringConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime v)
            {
                DateTime dt = v;
                return dt.ToString(this.DateForamt);
            }

            return string.Empty;
        }

        public string DateForamt { get; set; } = "yyyy-MM-dd HH:mm:ss";
    }
}