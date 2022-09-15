namespace ZzzLab.Data
{
    public interface IQuery
    {
        string CommandText { get; }
        int CommandTimeout { get; set; }
        QueryParameterCollection Parameters { get; }

        Query Clone();

        string ToString();
    }
}