namespace BulkRequest.Request
{
    public readonly struct RawQuerySettings() : IEquatable<string>, IEquatable<RawQuerySettings>
    {
        public required string Query { get; init; }
        public IEnumerable<string> Marks
        {
            get
            {
                return [
                    LocaleMark,
                    KernelMark,
                    MonthMark,
                    YearMark
                ];
            }
        }
        public string LocaleMark { get; init; } = "[l]";
        public string KernelMark { get; init; } = "[k]";
        public string MonthMark { get; init; } = "[m]";
        public string YearMark { get; init; } = "[y]";

        public bool Equals(string? other) => Query.Equals(other);

        public bool Equals(RawQuerySettings other) => Equals(other.Query);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (obj is string otherString)
                return Equals(otherString);

            if (obj is RawQuerySettings otherRawQuery)
                return Equals(otherRawQuery);

            return false;
        }

        public override int GetHashCode() => Query.GetHashCode();

        public override string ToString() => Query;

        public static bool operator ==(
            RawQuerySettings first,
            RawQuerySettings second) => first.Equals(second);

        public static bool operator ==(
            RawQuerySettings first,
            string second) => first.Equals(second);

        public static bool operator ==(
            string first,
            RawQuerySettings second) => second.Equals(first);

        public static bool operator !=(
            RawQuerySettings first,
            RawQuerySettings second) => !first.Equals(second);

        public static bool operator !=(
            RawQuerySettings first,
            string second) => !first.Equals(second);

        public static bool operator !=(
            string first,
            RawQuerySettings second) => !second.Equals(first);

        public static implicit operator string(RawQuerySettings value) =>
            value.Query;

        public static implicit operator RawQuerySettings(string value) =>
            new() { Query = value };
    }

    public interface IRawQuerySettings
    {
        string Query { get; init; }
        IEnumerable<string> Marks { get; }
    }
}