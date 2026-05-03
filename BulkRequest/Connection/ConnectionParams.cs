using BulkRequest.Config;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BulkRequest.Connection
{
    public class ConnectionParams : IValidatableObject
    {
        public required ConnectionMode Mode { get; init; }
        public required string Group { get; init; }
        [AllowNull]
        public ServerInfo[] Servers
        {
            get => field ?? [];
            init;
        }
        [AllowNull]
        public ServerInfo Server
        {
            get => field
                ?? new() { Host = "", Port = "" };
            init;
        }
        [AllowNull]
        public string[] Databases
        {
            get => field ?? [];
            init;
        }
        [AllowNull]
        public string Database
        {
            get => field ?? "";
            init;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Mode is not ConnectionMode.None
                && string.IsNullOrWhiteSpace(Group))
            {
                yield return new ValidationResult(
                    "Group name cannot be empty.",
                    [nameof(Group)]);
            }

            switch (Mode)
            {
                case ConnectionMode.Single:
                    if (string.IsNullOrWhiteSpace(Database))
                    {
                        yield return new ValidationResult(
                            "Database name cannot be empty.",
                            [nameof(Database)]);
                    }
                    break;
                case ConnectionMode.SingleMany:
                    if (Databases is null
                        || Databases.Length == 0)
                    {
                        yield return new ValidationResult(
                            "Array of databases cannot be empty.",
                            [nameof(Databases)]);
                    }
                    break;
                case ConnectionMode.Server:
                    if (Server is null
                        || string.IsNullOrWhiteSpace(Server.Host)
                        || string.IsNullOrWhiteSpace(Server.Port))
                    {
                        yield return new ValidationResult(
                            "Host and port cannot be empty.",
                            [nameof(Server)]);
                    }
                    break;
                case ConnectionMode.ServerMany:
                    if (Servers is null
                        || Servers.Length == 0)
                    {
                        yield return new ValidationResult(
                            "Array of servers cannot be empty.",
                            [nameof(Servers)]);
                    }
                    break;
            }
        }

        public bool IsValid(IMessageUnit? messageUnit = null)
        {
            IEnumerable<ValidationResult> errors = Validate(new ValidationContext(this));

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    messageUnit?.Message = error.ErrorMessage;
                }

                return false;
            }

            return true;
        }

        public bool IsValid(
            ConnectionMode expectedMode,
            IMessageUnit? messageUnit = null)
        {
            if (Mode != expectedMode)
            {
                messageUnit?.Message = $"Inappropriate connection mode (expected {expectedMode} instead of {Mode})!";
                return false;
            }

            return IsValid(messageUnit);
        }
    }
}