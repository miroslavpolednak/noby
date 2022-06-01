namespace CIS.Core.Attributes;

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
