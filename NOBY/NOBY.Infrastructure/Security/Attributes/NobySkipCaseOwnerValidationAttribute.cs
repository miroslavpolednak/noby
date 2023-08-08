namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class NobySkipCaseOwnerValidationAttribute
    : Attribute
{
}
