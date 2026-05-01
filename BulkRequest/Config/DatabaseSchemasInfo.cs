using System.Text.Json.Serialization;

namespace BulkRequest.Config
{
    internal class DatabaseSchemasInfo : DatabaseInfo, IJsonOnDeserialized
    {
        public required SchemaInfo Kernel { get; init; }
        public required Dictionary<string, SchemaInfo> Schemas { get; init; }

        public void OnDeserialized()
        {
            foreach (SchemaInfo schema in Schemas.Values)
            {
                schema.Kernel = Kernel;
            }
        }
    }
}