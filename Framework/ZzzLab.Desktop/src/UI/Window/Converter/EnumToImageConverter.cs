namespace System.Windows.Data
{
    public class EnumToImageConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string item && Enums != null && ImageSources != null)
            {
                string[] enumArr = Enums.Split('|');
                string[] imageArr = ImageSources.Split('|');

                if (enumArr.Length != imageArr.Length)
                {
                    return "";
                }

                for (int i = 0; i < enumArr.Length; i++)
                {
                    if (enumArr[i].Trim().Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        return imageArr[i].Trim();
                    }
                }
            }

            return "";
        }

        public string Enums { get; set; } = "";

        public string ImageSources { get; set; } = "";
    }
}