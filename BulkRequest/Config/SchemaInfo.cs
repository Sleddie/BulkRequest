namespace BulkRequest.Config;

public class SchemaInfo
{
    public required string Name { get; init; }
    public SchemaInfo? Kernel { get; internal set; }
    public DatabaseInfo? Database { get; internal set; }
}