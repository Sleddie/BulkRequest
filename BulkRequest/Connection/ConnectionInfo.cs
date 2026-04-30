using BulkRequest.Config;

namespace BulkRequest.Connection
{
    internal record ConnectionInfo(DatabaseInfo Database, string ConnectionString);
}