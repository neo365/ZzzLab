namespace ZzzLab.Event
{
    public delegate void ServerStatusChangeEventHandler(object sender, ServerStatusChangeEventArgs e);

    public enum ServerStatus : int
    {
        Unknown = 0,
        Startting,
        Started,
        Stopping,
        Stoped
    }

    public class ServerStatusChangeEventArgs : EventArgs
    {
        public ServerStatus Status { get; } = ServerStatus.Unknown;

        public string? Message { get; }

        public DateTime CurrentDateTime { get; }

        public ServerStatusChangeEventArgs(ServerStatus status, string? message = null, DateTime? currentDateTime = null)
        {
            Status = status;
            Message = message;
            CurrentDateTime = currentDateTime ?? DateTime.Now;
        }

        public override string ToString()
            => $"[{CurrentDateTime.To24Hours()}] | {Status} | {Message}";
    }
}