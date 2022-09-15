using ZzzLab.Configuration;
using ZzzLab.Web.Auth;

namespace ZzzLab.Web.Configuration
{
    public static class WebBuilderExtention
    {
        public static IConfigBuilder UseWebAuth<T>(this IConfigBuilder configBuilder) where T : IAuthorization
        {
            if (Activator.CreateInstance(typeof(T)) is IAuthorization auth)
            {
                configBuilder.Use(new WebBuilder(auth));
            }
            else throw new InvalidTypeException(typeof(T));

            return configBuilder;
        }

        public static IConfigBuilder UseWebAuth(this IConfigBuilder configBuilder, IAuthorization? auth = null)
        {
            configBuilder.Use(new WebBuilder(auth));
            return configBuilder;
        }

        public static IConfigBuilder UseJWTAuth<T>(this IConfigBuilder configBuilder) where T : IAuthorization
        {
            if (Activator.CreateInstance(typeof(T)) is IAuthorization auth)
            {
                configBuilder.Use(new WebBuilder(auth));
            }
            else throw new InvalidTypeException(typeof(T));

            return configBuilder;
        }

        public static IConfigBuilder UseJWTAuth(this IConfigBuilder configBuilder, IAuthorization? auth = null)
        {
            configBuilder.Use(new WebBuilder(auth));
            return configBuilder;
        }
    }
}