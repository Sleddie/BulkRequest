namespace BulkRequest.Request
{
    internal static class QueryBuilder
    {
        public static IEnumerable<string>? GetQueries(
            string rawQueryText,
            string databaseName,
            IMessageUnit? messageUnit = null)
        {
            throw new NotImplementedException();
        }

        public static string GetUnionAllQuery(
            string rawQueryText,
            string databaseName,
            IMessageUnit? messageUnit = null)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<string>? GetUnionAllQueries(
            string rawQueryText,
            string databaseName,
            IMessageUnit? messageUnit = null) =>
            [GetUnionAllQuery(rawQueryText, databaseName, messageUnit)];
    }
}