using CIS.Core.Exceptions;

namespace CIS.Core.Types;

/// <summary>
/// Value type pro název aplikace / služby.
/// </summary>
public sealed record ApplicationKey
{
    /// <summary>
    /// Název aplikace v prostředí NOBY.
    /// </summary>
    /// <remarks>DS:CustomerService</remarks>
    public string Key { get; init; }

    /// <param name="key">Název aplikace {DS|CIS}:{app_name}</param>
    /// <exception cref="CisInvalidApplicationKeyException">Název aplikace není vyplněný nebo nemá povolený formát.</exception>
    public ApplicationKey(string? key)
    {
        if (string.IsNullOrEmpty(key))
            throw new CisInvalidApplicationKeyException(key ?? "");

        if (!key.All(t => Char.IsLetterOrDigit(t) || t == ':'))
        {
            throw new CisInvalidApplicationKeyException(key);
        }
        else
        {
            this.Key = key;
        }
    }

    public static implicit operator string(ApplicationKey d) => d.Key;

    public override string ToString() => $"{Key}";
}
