using System.Text.Json.Serialization;

namespace BulkRequest.Config
{
    internal class ServerDatabasesInfo : ServerInfo, IJsonOnDeserialized
    {
        public required Dictionary<string, DatabaseInfo> Databases { get; init; }

        public void OnDeserialized()
        {
            foreach (DatabaseInfo db in Databases.Values)
            {
                db.Server = this;
            }
        }
    }
}