namespace NOBY.Infrastructure.Security.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class NobyRequiredCaseStatesAttribute
    : Attribute
{
    public CaseStates[] CaseStates { get; init; }

    public NobyRequiredCaseStatesAttribute(params CaseStates[] caseStates)
    {
        CaseStates = caseStates;
    }
}
