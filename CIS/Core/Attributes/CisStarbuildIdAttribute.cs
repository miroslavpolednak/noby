namespace CIS.Core.Attributes;

/// <summary>
/// Specifický atribut pro číselníkové enumy.
/// </summary>
/// <remarks>
/// Označuje hodnotu dané enum value ve Starbuildu:
/// `StarbuildId = t.GetAttribute&lt;CIS.Core.Attributes.CisStarbuildIdAttribute&gt;()?.StarbuildId`
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class CisStarbuildIdAttribute : Attribute
{
    public int? StarbuildId { get; init; }

    public CisStarbuildIdAttribute()
    {
    }

    public CisStarbuildIdAttribute(int starbuildId)
    {
        StarbuildId = starbuildId;
    }
}