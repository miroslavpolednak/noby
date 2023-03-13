namespace CIS.Core.Types;

/// <summary>
/// Value type pro název/typ aplikačního prostředí.
/// </summary>
public sealed record ApplicationEnvironmentName
{
    /// <summary>
    /// Název aplikačního prostředí.
    /// </summary>
    /// <example>FAT</example>
    public string Name { get; init; }

    /// <param name="environment">Název prostředí</param>
    /// <exception cref="Exceptions.CisInvalidEnvironmentNameException">Název prostředí není zadaný nebo nemá platný formát.</exception>
    public ApplicationEnvironmentName(string? environment)
    {
        if (string.IsNullOrEmpty(environment))
            throw new Exceptions.CisInvalidEnvironmentNameException($"Environment name is empty", nameof(environment));
        if (!environment.All(Char.IsLetterOrDigit))
            throw new Exceptions.CisInvalidEnvironmentNameException($"Environment name '{environment}' is not valid", nameof(environment));
        Name = environment;
    }

    public static implicit operator string(ApplicationEnvironmentName d) => d.Name;

    public override string ToString() => $"{Name}";
}