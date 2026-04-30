namespace BulkRequest.Config
{
    public class ServerInfo
    {
        public required string Host { get; init; }
        public required string Port { get; init; }

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}