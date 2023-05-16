namespace ZzzLab.Web.Configuration
{
    public class WebConfig
    {
        public int Port { set; get; }

        public IEnumerable<string>? Origins { set; get; }
    }
}