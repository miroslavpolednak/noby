namespace CIS.Infrastructure.WebApi.Swagger;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RollbackDescriptionAttribute
    : Attribute
{
    public string Description { get; private set; }

    public RollbackDescriptionAttribute(string description)
    {
        Description = description;
    }
}
