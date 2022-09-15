namespace ZzzLab.Web.Logging
{
    public interface IHttpLog
    {
        string? TraceIdentifier { get; }
        string? Protocol { get; }
        IDictionary<string, string>? Headers { get; }
        string? Body { get; }
        DateTime RegDateTime { get; }
    }
}