namespace CIS.Core.Attributes;

/// <summary>
/// Používá se k označení výchozí hodnoty enumu.
/// </summary>
/// <remarks>
/// Volající kód může podobným dotazem zjistit, zda je daná enum value výchozí:
/// `IsDefault = t.HasAttribute&lt;CIS.Core.Attributes.CisDefaultValueAttribute&gt;()`
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class CisDefaultValueAttribute : Attribute
{
    public string IsDefaultFor { get; init; } = "";

    public CisDefaultValueAttribute()
    {
    }

    public CisDefaultValueAttribute(string isDefaultFor)
    {
        IsDefaultFor = isDefaultFor;
    }
}