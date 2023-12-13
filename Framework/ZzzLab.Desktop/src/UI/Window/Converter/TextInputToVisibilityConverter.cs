namespace System.Windows.Data
{
    public class TextInputToVisibilityConverter : MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Always test MultiValueConverter inputs for non-null
            // (to avoid crash bugs for views in the designer)
            if (values[0] is bool v1 && values[1] is bool v2)
            {
                bool hasText = !v1;
                bool hasFocus = v2;

                if (hasFocus || hasText)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }
    }
}