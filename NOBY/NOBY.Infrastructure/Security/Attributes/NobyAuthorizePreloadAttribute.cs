namespace NOBY.Infrastructure.Security.Attributes;

/// <summary>
/// Dekorátor endpointu. Nastavení kešování entit v NOBY autorizaci.
/// </summary>
/// <remarks>
/// Zajistí, že v případě jakékoliv NOBY custom autorizace se v service clientu kešuje celá entita (tj. volá se GetHousehold, GetCase...) místo toho, aby se volal jen validační endpoint dané služby (ValidateHouseholdId, ValidateCaseId...).
/// Použije se v případě, kdy víme, že v následném handleru budeme danou entitu také potřebovat.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class NobyAuthorizePreloadAttribute
    : Attribute
{
    /// <summary>
    /// Entity, které se mají načítat celé.
    /// </summary>
    public LoadableEntities Preload { get; init; }

    public NobyAuthorizePreloadAttribute(LoadableEntities preload)
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
