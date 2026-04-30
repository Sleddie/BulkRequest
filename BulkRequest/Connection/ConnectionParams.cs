using BulkRequest.Config;
using System.Diagnostics.CodeAnalysis;

namespace BulkRequest.Connection
{
    public class ConnectionParams
    {
        public required ConnectionMode Mode { get; set; }
        public required string Group { get; set; }
        [AllowNull]
        public ServerInfo[] Servers
        {
            get => field ?? [];
            set
            {
                switch (Mode)
                {
                    case ConnectionMode.ServerMany:
                        ArgumentNullException.ThrowIfNull(value, nameof(Servers));

                        if (value.Length == 0)
                        {
                            throw new ArgumentNullException(nameof(Servers), "Array of servers cannot be empty.");
                        }
                        break;
                }

                field = value;
            }
        }
        [AllowNull]
        public ServerInfo Server
        {
            get => field
                ?? new() { Host = "", Port = "" };
            set
            {
                switch (Mode)
                {
                    case ConnectionMode.Server:
                        ArgumentNullException.ThrowIfNull(value, nameof(Server));

                        if (string.IsNullOrWhiteSpace(value.Host)
                            || string.IsNullOrWhiteSpace(value.Port))
                        {
                            throw new ArgumentNullException(nameof(Server), "Host and port cannot be empty.");
                        }
                        break;
                }

                field = value;
            }
        }
        [AllowNull]
        public string[] Databases
        {
            get => field ?? [];
            set
            {
                switch (Mode)
                {
                    case ConnectionMode.SingleMany:
                        ArgumentNullException.ThrowIfNull(value, nameof(Databases));

                        if (value.Length == 0)
                        {
                            throw new ArgumentNullException(nameof(Databases), "Array of databases cannot be empty.");
                        }
                        break;
                }

                field = value;
            }
        }
        [AllowNull]
        public string Database
        {
            get => field ?? "";
            set
            {
                switch (Mode)
                {
                    case ConnectionMode.Single:
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            throw new ArgumentNullException(nameof(Database), "Database cannot be empty.");
                        }
                        break;
                }

                field = value;
            }
        }
    }
}