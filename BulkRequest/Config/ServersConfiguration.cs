using DefaultSolutions;
using System.Text.Json;

namespace BulkRequest.Config
{
    public class ServersConfiguration
    {
        public const string DefaultConfigFileName = "database.config.json";

        private static Lazy<Dictionary<string, ServersGroupInfo>> s_connectionConfig =
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
        private static Dictionary<string, ServersGroupInfo> Configuration
        {
            get => s_connectionConfig.Value;
        }
        public static ICollection<string> ServerGroups { get => Configuration.Keys; }
        
        public static ICollection<ServerInfo> Servers(string serverGroup)
        {
            ServersGroupInfo? targetServersGroup = GetServersGroup(serverGroup);

            if (targetServersGroup == null)
            {
                return [];
            }

            return [.. targetServersGroup.Servers.Values];
        }

        public static ICollection<DatabaseInfo> Databases(string serverGroup,
            IEnumerable<ServerInfo>? servers = null)
        {
            ServersGroupInfo? targetServersGroup = GetServersGroup(serverGroup);

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
            string configText = FileExplore.OpenAndReadText(ConfigFileFullPath);
            Dictionary<string, ServersGroupInfo>? config =
                JsonSerializer.Deserialize<Dictionary<string, ServersGroupInfo>>(configText)
                ?? [];
            return config;
        }

        private static ServersGroupInfo? GetServersGroup(string serverGroup)
        {
            if (string.IsNullOrWhiteSpace(serverGroup))
            {
                return null;
            }

            Configuration.TryGetValue(
                serverGroup,
                out ServersGroupInfo? serverInfo);
            return serverInfo;
        }
    }
}