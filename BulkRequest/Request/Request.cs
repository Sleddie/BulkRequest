using BulkRequest.Connection;

namespace BulkRequest.Request
{
    public static class Request
    {
        private static bool SafeRequest<TResult>(
            string rawQueryText,
            ConnectionParams connectionParams,
            RequestFunc<TResult> requestFromDb,
            ResultUnit<TResult> resultUnit,
            MergeFunc<TResult>? mergeResults = null,
            bool receiveAsap = true)
        {
            try
            {
                ICollection<ConnectionInfo>? connectionStrings = ConnectionBuilder.GetConnectionStrings(
                    connectionParams,
                    resultUnit);

                if (connectionStrings is null)
                {
                    return false;
                }

                bool receivedAtLeastOne = false;
                TResult? mergedResultData = default;

                foreach (ConnectionInfo connectionStringInfo in connectionStrings)
                {
                    QueryBuilderFunc queryBuilderFunc =
                        receiveAsap
                        ? QueryBuilder.GetQueries
                        : QueryBuilder.GetUnionAllQueries;

                    bool received = RunRawRequest(
                        rawQueryText,
                        connectionStringInfo.Database.Name,
                        connectionStringInfo.ConnectionString,
                        queryBuilderFunc,
                        requestFromDb,
                        out TResult? singleResult,
                        resultUnit);
                    receivedAtLeastOne |= received;

                    if (receiveAsap
                        || !received)
                    {
                        continue;
                    }

                    if (mergeResults is null)
                    {
                        resultUnit.Result = singleResult;
                    }
                    else
                    {
                        mergedResultData = mergeResults(
                            mergedResultData,
                            singleResult);
                    }
                }

                if (!receiveAsap
                    && mergedResultData is not null)
                {
                    resultUnit.Result = mergedResultData;
                }

                return receivedAtLeastOne;
            }
            catch (Exception ex)
            {
                resultUnit.Message =
                    "SafeRequest exception: " +
                    $"{Environment.NewLine}{ex.Message}";
                return false;
            }
        }

        private static bool RunRawRequest<TResult>(
            string rawQueryText,
            string databaseName,
            string connectionString,
            QueryBuilderFunc buildQueries,
            RequestFunc<TResult> requestFromDb,
            out TResult? result,
            ResultUnit<TResult> resultUnit)
        {
            IEnumerable<string>? preparedQueries =
                buildQueries(
                    rawQueryText,
                    databaseName,
                    resultUnit);

            if (preparedQueries is null)
            {
                result = default;
                return false;
            }

            resultUnit.TotalCount += (uint)preparedQueries.Count();
            result = default;

            foreach (string query in preparedQueries)
            {
                requestFromDb(
                    query,
                    connectionString,
                    out result,
                    resultUnit);
            }

            return true;
        }

        private delegate IEnumerable<string>? QueryBuilderFunc(
            string rawQueryText,
            string databaseName,
            IMessageUnit? messageUnit = null);

        private delegate TResult RequestFunc<TResult>(
            string connectionString,
            string queryText,
            out TResult result,
            ResultUnit<TResult> resultUnit);

        private delegate TResult? MergeFunc<TResult>(
            TResult? first,
            TResult? second);
    }
}