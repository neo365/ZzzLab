using System.IO;
using System.Windows.Media.Imaging;

namespace System.Windows.Data
{
    public class PathToImageConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string filepath)
            {
                if (string.IsNullOrWhiteSpace(filepath)) throw new ArgumentNullException(nameof(value));

                if (File.Exists(filepath)) return new BitmapImage(new Uri(filepath));
            }

            throw new ArgumentException($"{nameof(value)} is Invalid");
        }
    }
}