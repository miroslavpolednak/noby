namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class NobyRequiredCaseStatesAttribute
    : Attribute
{
    public EnumCaseStates[] CaseStates { get; init; }

    public NobyRequiredCaseStatesAttribute(params EnumCaseStates[] caseStates)
    {
        CaseStates = caseStates;
    }
}
