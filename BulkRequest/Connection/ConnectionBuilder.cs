using BulkRequest.Config;
using DefaultSolutions;

namespace BulkRequest.Connection
{
    internal static class ConnectionBuilder
    {
        private const string ConnectionStringFormat =
            "Server={2};Database={3}:{4};User Id={0};Password={1};";

        private static string Username
        {
            get
            {
                ArgumentNullException.ThrowIfNull(field, nameof(Username));
                return field;
            }
            set;
        }
        private static string Password
        {
            get
            {
                ArgumentNullException.ThrowIfNull(field, nameof(Password));
                return field;
            }
            set;
        }

        public static void SetUsernamePassword(
            string username,
            string password)
        {
            Username = username;
            Password = password;
        }

        public static ICollection<ConnectionInfo>? GetConnectionStrings(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit = null)
        {
            if (connectionParams is null
                || connectionParams.Mode is ConnectionMode.None)
            {
                messageUnit?.Message = "Not set connection parameters!";
                return null;
            }

            switch (connectionParams.Mode)
            {
                case ConnectionMode.Single:
                    return GetConnectionStringsForSingle(
                        connectionParams,
                        messageUnit);
                case ConnectionMode.SingleMany:
                    return GetConnectionStringsForSingleMany(
                        connectionParams,
                        messageUnit);
                case ConnectionMode.Server:
                    return GetConnectionStringsForServer(
                        connectionParams,
                        messageUnit);
                case ConnectionMode.ServerMany:
                    return GetConnectionStringsForServerMany(
                        connectionParams,
                        messageUnit);
                case ConnectionMode.Bulk:
                    return GetConnectionStringsForBulk(
                        connectionParams,
                        messageUnit);
                default:
                    messageUnit?.Message = $"Undefined connection mode ({connectionParams.Mode})!";
                    return null;
            }
        }

        internal static ConnectionInfo? GetConnectionStringInfo(
            DatabaseInfo? database,
            IMessageUnit? messageUnit = null)
        {
            string connectionString = FillConnectionString(
                database,
                out string errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                messageUnit?.Message = errorMessage;
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            return new ConnectionInfo(database!, connectionString);
        }

        internal static ICollection<ConnectionInfo> GetConnectionStringsInfo(
            IEnumerable<DatabaseInfo> databases,
            IMessageUnit? messageUnit = null)
        {
            List<ConnectionInfo> connectionStringsInfo = [];

            foreach (DatabaseInfo dbInfo in databases)
            {
                ConnectionInfo? connectionStringInfo = GetConnectionStringInfo(
                    dbInfo,
                    messageUnit);

                if (connectionStringInfo is null)
                {
                    continue;
                }

                connectionStringsInfo.Add(connectionStringInfo);
            }

            return connectionStringsInfo;
        }

        #region private

        #region FillConnectionString

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
            string username,
            string password,
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
                username,
                password);
        }

        private static string FillConnectionString(
            DatabaseInfo? database,
            out string errorMessage)
        {
            return FillConnectionString(
                database,
                Username,
                Password,
                out errorMessage);
        }

        #endregion

        private static ServersGroupInfo? GetServersGroup(
            string groupName,
            IMessageUnit? messageUnit)
        {
            ServersGroupInfo? serversGroupInfo = ServersConfiguration
                .Configuration.TryGet(groupName);

            if (serversGroupInfo is null)
            {
                messageUnit?.Message = $"Servers group is not found.";
            }

            return serversGroupInfo;
        }

        #region GetConnectionStrings by Mode

        private static ICollection<ConnectionInfo>? GetConnectionStringsForSingle(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit)
        {
            if (!connectionParams.IsValid(
                ConnectionMode.Single,
                messageUnit))
            {
                return null;
            }

            ServersGroupInfo? serversGroupInfo = GetServersGroup(
                connectionParams.Group,
                messageUnit);

            if (serversGroupInfo is null)
            {
                return null;
            }

            DatabaseInfo? dbInfo = serversGroupInfo
                .Databases.TryGet(connectionParams.Database);
            ConnectionInfo? connectionStringInfo = GetConnectionStringInfo(
                dbInfo,
                messageUnit);
            return connectionStringInfo is null
                ? null
                : [connectionStringInfo];
        }

        private static ICollection<ConnectionInfo>? GetConnectionStringsForSingleMany(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit)
        {
            if (!connectionParams.IsValid(
                ConnectionMode.SingleMany,
                messageUnit))
            {
                return null;
            }

            ServersGroupInfo? serversGroupInfo = GetServersGroup(
                connectionParams.Group,
                messageUnit);

            if (serversGroupInfo is null)
            {
                return null;
            }

            IEnumerable<DatabaseInfo> targetDatabases = connectionParams.Databases
                .Select(db => serversGroupInfo.Databases[db]);
            return GetConnectionStringsInfo(
                targetDatabases,
                messageUnit);
        }

        private static ICollection<ConnectionInfo>? GetConnectionStringsForServer(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit)
        {
            if (!connectionParams.IsValid(
                ConnectionMode.Server,
                messageUnit))
            {
                return null;
            }

            ServersGroupInfo? serversGroupInfo = GetServersGroup(
                connectionParams.Group,
                messageUnit);

            if (serversGroupInfo is null)
            {
                return null;
            }

            ServerDatabasesInfo? serverInfo = serversGroupInfo
                .Servers.TryGet(connectionParams.Server.ToString());

            if (serverInfo is null)
            {
                return null;
            }

            return GetConnectionStringsInfo(
                serverInfo.Databases.Values,
                messageUnit);
        }

        private static ICollection<ConnectionInfo>? GetConnectionStringsForServerMany(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit)
        {
            if (!connectionParams.IsValid(
                ConnectionMode.Server,
                messageUnit))
            {
                return null;
            }

            ServersGroupInfo? serversGroupInfo = GetServersGroup(
                connectionParams.Group,
                messageUnit);

            if (serversGroupInfo is null)
            {
                return null;
            }

            IEnumerable<DatabaseInfo> targetDatabases = connectionParams.Servers
                .Select(s => serversGroupInfo.Servers[s.ToString()])
                .SelectMany(s => s.Databases.Values);
            return GetConnectionStringsInfo(
                targetDatabases,
                messageUnit);
        }

        private static ICollection<ConnectionInfo>? GetConnectionStringsForBulk(
            ConnectionParams connectionParams,
            IMessageUnit? messageUnit)
        {
            if (!connectionParams.IsValid(
                ConnectionMode.Bulk,
                messageUnit))
            {
                return null;
            }

            ServersGroupInfo? serversGroupInfo = GetServersGroup(
                connectionParams.Group,
                messageUnit);

            if (serversGroupInfo is null)
            {
                return null;
            }

            return GetConnectionStringsInfo(
                serversGroupInfo.Databases.Values,
                messageUnit);
        }

        #endregion

        #endregion
    }
}