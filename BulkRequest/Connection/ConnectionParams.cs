using BulkRequest.Config;
using System.ComponentModel.DataAnnotations;

namespace BulkRequest.Connection;

public class ConnectionParams : IValidatableObject
{
    public required ConnectionMode Mode { get; init; }

    public required string Group { get; init; }

    public ServerInfo[] Servers
    {
        get => field ?? [];
        init;
    }

    public string[] Databases
    {
        get => field ?? [];
        init;
    }

    public string[] Schemas
    {
        get => field ?? [];
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
            case ConnectionMode.Schema:
            case ConnectionMode.Database:
                if (Mode is ConnectionMode.Schema)
                {
                    if (Schemas is null
                        || Schemas.Length == 0)
                    {
                        yield return new ValidationResult(
                            "Array of schemas cannot be empty.",
                            [nameof(Schemas)]);
                    }
                }

                if (Databases is null
                    || Databases.Length == 0)
                {
                    yield return new ValidationResult(
                        "Array of databases cannot be empty.",
                        [nameof(Databases)]);
                }
                break;
            case ConnectionMode.Server:
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
        IEnumerable<ValidationResult> errors = Validate(new(this));

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