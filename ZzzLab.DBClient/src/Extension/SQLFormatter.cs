namespace ZzzLab.Data
{
    public static class SQLUtils
    {
        public static string Formatter(string commandText)
            => NSQLFormatter.Formatter.Format(commandText);
    }
}