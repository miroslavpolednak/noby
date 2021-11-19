namespace CIS.Core.Types;

public record ApplicationEnvironmentName
{
    public string Name { get; init; }

    public ApplicationEnvironmentName(string? environment)
    {
        if (string.IsNullOrEmpty(environment))
            throw new Exceptions.CisInvalidEnvironmentNameException($"Environment name is empty", "environment");
        if (!environment.All(Char.IsLetterOrDigit))
            throw new Exceptions.CisInvalidEnvironmentNameException($"Environment name '{environment}' is not valid", "environment");
        Name = environment;
    }

    public static implicit operator string(ApplicationEnvironmentName d) => d.Name;

    public override string ToString() => $"{Name}";
}
