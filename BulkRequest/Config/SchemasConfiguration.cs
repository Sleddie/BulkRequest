using DefaultSolutions;
using System.Text.Json;

namespace BulkRequest.Config
{
    internal static class SchemasConfiguration
    {
        public const string DefaultConfigFileName = "schema.config.json";

        private static Lazy<Dictionary<string, ServersGroupInfo>> s_connectionConfig =
            new(LoadSchemaConfig);

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

        private static Dictionary<string, ServersGroupInfo> LoadSchemaConfig()
        {
            string configText = FileExplore.OpenAndReadText(ConfigFileFullPath);
            Dictionary<string, ServersGroupInfo>? config =
                JsonSerializer.Deserialize<Dictionary<string, ServersGroupInfo>>(configText)
                ?? [];
            return config;
        }
    }
}