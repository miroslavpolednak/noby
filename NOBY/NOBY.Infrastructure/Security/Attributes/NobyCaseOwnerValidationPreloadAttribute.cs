namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class NobyCaseOwnerValidationPreloadAttribute
    : Attribute
{
    public LoadableEntities Preload { get; init; }

    public NobyCaseOwnerValidationPreloadAttribute(LoadableEntities preload = LoadableEntities.None)
    {
        Preload = preload;
    }

    [Flags]
    public enum LoadableEntities
    {
        None,
        Case,
        SalesArrangement,
        Household,
        CustomerOnSA
    }
}
