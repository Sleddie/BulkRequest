using DefaultSolutions;

namespace BulkRequest.Config
{
    public class ServersConfiguration
    {
        /// <summary>Имя файла конфигурации распределённой базы данных.</summary>
        public const string DefaultConfigFileName = "database.config.json";

        private readonly static Lazy<Dictionary<string, ServersGroupInfo>> s_connectionConfig =
            new(LoadConnectionConfig);

        private static string ConfigFileFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    field = Path.GetFullPath(DefaultConfigFileName);
                }

                return field;
            }
        }

        internal static Dictionary<string, ServersGroupInfo> Configuration
        {
            get => s_connectionConfig.Value;
        }

        /// <summary>Коллекция имён групп серверов.</summary>
        public static ICollection<string> ServerGroups { get => Configuration.Keys; }
        
        /// <summary>Коллекция объектов типа ServerInfo со сведениями
        /// о сервере.</summary>
        /// <param name="serverGroup">Имя группы серверов.</param>
        /// <returns></returns>
        public static ICollection<ServerInfo> Servers(string serverGroup)
        {
            ServersGroupInfo? targetServersGroup =
                Configuration.TryGet(serverGroup);

            if (targetServersGroup == null)
            {
                return [];
            }

            return [.. targetServersGroup.Servers.Values];
        }

        /// <summary>Коллекция объектов типа DatabaseInfo со сведениями
        /// о базе данных.</summary>
        /// <param name="serverGroup">Имя группы серверов.</param>
        /// <param name="servers">Перечень объектов типа ServerInfo со
        /// сведениями о сервере.</param>
        /// <returns></returns>
        public static ICollection<DatabaseInfo> Databases(string serverGroup,
            IEnumerable<ServerInfo>? servers = null)
        {
            ServersGroupInfo? targetServersGroup = Configuration.TryGet(serverGroup);

            if (targetServersGroup == null)
            {
                return [];
            }

            if (servers == null)
            {
                return targetServersGroup.Databases.Values;
            }

            return [.. targetServersGroup.Servers.Values
                .Where(s => s.ToString() == serverGroup.ToString())
                .SelectMany(s => s.Databases.Values)];
        }

        private static Dictionary<string, ServersGroupInfo> LoadConnectionConfig()
        {
            return FileExplore.OpenAndReadJson<Dictionary<string, ServersGroupInfo>>(ConfigFileFullPath)
                ?? [];
        }
    }
}