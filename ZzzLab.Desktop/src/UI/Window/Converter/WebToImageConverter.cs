using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using ZzzLab;

namespace System.Windows.Data
{
    public class WebToImageConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is string address)
            {
                if (string.IsNullOrWhiteSpace(address)) return this.NoImage;

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = Task.Run(async () => await client.GetAsync(address)).Result;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Task.Run(async () => await response.Content.CopyToAsync(ms));

                            if (ms.Length > 0)
                            {
                                ms.Position = 0;
                                return BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                            }
                        }
                    }
                    return this.NoImage;
                }
                catch (Exception ex)
                {
                    Logger.Warning(ex);
                    return this.NoImage;
                }
            }

            return this.NoImage;
        }

        public string NoImage { set; get; } = "";
    }
}