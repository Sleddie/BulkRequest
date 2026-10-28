using System.Diagnostics.CodeAnalysis;

namespace BulkRequest.Config;

public class ServerInfo
{
    public string Name
    {
        get => field ??= ToString();
        init;
    }
    public required string Host { get; init; }
    public required string Port { get; init; }

    public override string ToString()
    {
        return $"{Host}:{Port}";
    }

    public bool IsValid(out string errorMessage)
    {
        string props = "";
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(Host))
        {
            props = "ip-address";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Port))
        {
            if (props != "")
            {
                props += " and ";
            }

            props += "port";
            isValid = false;
        }

        if (isValid)
            errorMessage = "";
        else
            errorMessage = $"Server {props} should be specified.";

        return isValid;
    }
}

public class ServerInfoComparer : IEqualityComparer<ServerInfo>
{
    public bool Equals(ServerInfo? x, ServerInfo? y) =>
        string.Equals(x?.ToString(), y?.ToString(), StringComparison.OrdinalIgnoreCase);

    public int GetHashCode([DisallowNull] ServerInfo obj) =>
        obj.ToString().GetHashCode();
}