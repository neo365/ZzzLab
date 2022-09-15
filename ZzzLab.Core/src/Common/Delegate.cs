using ZzzLab.Configuration;

namespace ZzzLab
{
    public delegate void NoReturnDelegate();

    public delegate void ObjectArgDelegate(params object[] args);

    public delegate void ConfigBuilderDelegate(IConfigBuilder configBuilder, params object[] args);
}