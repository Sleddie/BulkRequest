namespace BulkRequest.Config
{
    internal class SchemaInfo
    {
        public required string Name { get; init; }
        public SchemaInfo? Kernel { get; set; }
    }
}