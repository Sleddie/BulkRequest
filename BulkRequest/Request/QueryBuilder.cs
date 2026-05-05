using BulkRequest.Config;
using DefaultSolutions;
using System.Text;

namespace BulkRequest.Request
{
    internal static class QueryBuilder
    {
        public static IEnumerable<string>? GetQueries(
            this RawQuerySettings rawQuery,
            DatabaseInfo database,
            IMessageUnit? messageUnit = null)
        {
            DatabaseSchemasInfo? databaseSchemasInfo =
                SchemasConfiguration.Configuration.TryGet(database.Name);

            if (databaseSchemasInfo is null)
            {
                return null;
            }

            return rawQuery.FillQueries(databaseSchemasInfo, messageUnit);
        }

        public static string GetUnionAllQuery(
            this RawQuerySettings rawQuery,
            DatabaseInfo database,
            IMessageUnit? messageUnit = null)
        {
            IEnumerable<string>? queries =
                rawQuery.GetQueries(database, messageUnit);

            if (queries is null)
            {
                return "";
            }

            StringBuilder unionAllQueryBuilder = new();

            foreach (string queryText in queries)
            {
                if (unionAllQueryBuilder.Length > 0)
                {
                    unionAllQueryBuilder.AppendLine(" UNION ALL ").AppendLine();
                }

                unionAllQueryBuilder.Append(queryText);
            }

            return unionAllQueryBuilder.ToString();
        }

        public static IEnumerable<string>? GetUnionAllQueries(
            this RawQuerySettings rawQuery,
            DatabaseInfo database,
            IMessageUnit? messageUnit = null) =>
            [GetUnionAllQuery(rawQuery, database, messageUnit)];

        #region private

        #region FillQuery

        private static string FillQuery(
            this RawQuerySettings rawQuery,
            SchemaInfo schema,
            IMessageUnit? messageUnit = null)
        {
            if (!CheckArguments(rawQuery, schema, messageUnit))
            {
                return rawQuery;
            }

            StringBuilder preparedQueryBuilder = new(rawQuery);
            bool isKernel = schema.Kernel is null;

            if (!isKernel
                && !string.IsNullOrWhiteSpace(rawQuery.LocaleMark)
                && rawQuery.Query.Contains(rawQuery.LocaleMark))
            {
                preparedQueryBuilder.Replace(
                    rawQuery.LocaleMark,
                    schema.Name);
            }

            if (!string.IsNullOrWhiteSpace(rawQuery.KernelMark))
            {
                if (!isKernel)
                {
                    preparedQueryBuilder.Replace(
                        rawQuery.KernelMark,
                        schema.Kernel!.Name);
                }
                else
                {
                    preparedQueryBuilder.Replace(
                        rawQuery.KernelMark,
                        schema.Name);
                }
            }

            return preparedQueryBuilder.ToString();
        }

        private static IEnumerable<string> FillQueries(
            this RawQuerySettings rawQuery,
            DatabaseSchemasInfo database,
            IMessageUnit? messageUnit = null)
        {
            if (!CheckArguments(rawQuery, messageUnit: messageUnit))
            {
                return [];
            }

            List<string> preparedQueries = [];

            if (!string.IsNullOrWhiteSpace(rawQuery.LocaleMark)
                && rawQuery.Query.Contains(rawQuery.LocaleMark))
            {
                foreach (SchemaInfo schema in database.Schemas.Values)
                {
                    preparedQueries.Add(rawQuery
                        .FillQuery(schema, messageUnit));
                }
            }
            else if (!string.IsNullOrWhiteSpace(rawQuery.KernelMark)
                && rawQuery.Query.Contains(rawQuery.KernelMark))
            {
                preparedQueries.Add(rawQuery
                    .FillQuery(database.Kernel, messageUnit));
            }

            return preparedQueries;
        }

        #endregion

        private static bool CheckArguments(
            RawQuerySettings rawQuery,
            SchemaInfo? schema = null,
            IMessageUnit? messageUnit = null)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(rawQuery))
            {
                messageUnit?.Message = "Query text is empty.";
                isValid = false;
            }

            if (schema is not null)
            {
                if (string.IsNullOrWhiteSpace(schema.Name))
                {
                    messageUnit?.Message = "Schema name is empty.";
                    isValid = false;
                }

                if (schema.Kernel is not null
                    && string.IsNullOrWhiteSpace(schema.Kernel.Name))
                {
                    messageUnit?.Message = "Kernel schema name is empty.";
                    isValid = false;
                }
            }

            return isValid;
        }

        #endregion
    }
}