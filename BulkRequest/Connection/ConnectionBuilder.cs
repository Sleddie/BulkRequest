using BulkRequest.Config;

namespace BulkRequest.Connection
{
    internal static class ConnectionBuilder
    {
        private const string ConnectionStringFormat =
            "Server={2};Database={3}:{4};User Id={0};Password={1};";
   
        public static ICollection<ConnectionInfo>? GetConnectionStrings(
            ConnectionParams connectionParams,
            OperationUnit? operationUnit = null)
        {
            throw new NotImplementedException();
        }

        private static string FillConnectionString(
            string host,
            string port,
            string database,
            string username,
            string password)
        {
            return string.Format(
                ConnectionStringFormat,
                username,
                password,
                host,
                port,
                database);
        }

        private static string FillConnectionString(
            DatabaseInfo? database,
            out string errorMessage)
        {
            if (database is null
                || string.IsNullOrWhiteSpace(database.Name)
                || database.Server is null
                || string.IsNullOrWhiteSpace(database.Server.Host)
                || string.IsNullOrWhiteSpace(database.Server.Port))
            {
                errorMessage = $"Отсутствуют или неполные сведения о базе данных ({nameof(database)})";
                return "";
            }

            errorMessage = "";
            return FillConnectionString(
                database.Server.Host,
                database.Server.Port,
                database.Name,
                "no_username",
                "no_pw");
        }
    }
}