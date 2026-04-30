using System.Text.Json.Serialization;

namespace BulkRequest.Config
{
    internal class DatabaseSchemasInfo : DatabaseInfo, IJsonOnDeserialized
    {
        public required SchemaInfo Kernel { get; init; }
        public required Dictionary<string, SchemaInfo> DataBanks { get; init; }

        public void OnDeserialized()
        {
            foreach (SchemaInfo dataBank in DataBanks.Values)
            {
                dataBank.Kernel = Kernel;
            }
        }
    }
}