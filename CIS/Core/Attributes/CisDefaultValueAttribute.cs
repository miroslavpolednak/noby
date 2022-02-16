namespace CIS.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class CisDefaultValueAttribute : Attribute
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