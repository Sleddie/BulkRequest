namespace BulkRequest.Config
{
    public class DatabaseInfo
    {
        public required string Name { get; init; }
        public ServerInfo? Server { get; internal set; }
    }
}