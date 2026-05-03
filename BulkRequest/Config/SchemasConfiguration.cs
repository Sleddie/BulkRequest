using DefaultSolutions;

namespace BulkRequest.Config
{
    internal static class SchemasConfiguration
    {
        /// <summary>Имя файла конфигурации схем в базах данных.</summary>
        public const string DefaultConfigFileName = "schema.config.json";

        private readonly static Lazy<Dictionary<string, DatabaseSchemasInfo>> s_connectionConfig =
            new(LoadSchemasConfig);

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

        private static Dictionary<string, DatabaseSchemasInfo> Configuration
        {
            get => s_connectionConfig.Value;
        }

        /// <summary>Коллекция имён баз данных.</summary>
        public static ICollection<string> Databases { get => Configuration.Keys; }

        /// <summary>Коллекция объектов типа SchemaInfo со сведениями о схеме
        /// в базе данных.</summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns></returns>
        public static ICollection<SchemaInfo> Schemas(string database)
        {
            DatabaseSchemasInfo? targetDatabaseInfo =
                Configuration.TryGet(database);

            if (targetDatabaseInfo is null)
            {
                return [];
            }

            return targetDatabaseInfo.Schemas.Values;
        }

        private static Dictionary<string, DatabaseSchemasInfo> LoadSchemasConfig()
        {
            return FileExplore.OpenAndReadJson<Dictionary<string, DatabaseSchemasInfo>>(ConfigFileFullPath)
                ?? [];
        }
    }
}