namespace NOBY.Api.Endpoints.Customer.Shared;

public sealed class LegalCapacityItem
{
    /// <summary>
    /// Typ právního omezení klienta
    /// </summary>
    public int? RestrictionTypeId { get; set; }

    /// <summary>
    /// Datum platnosti právního omezení klienta do
    /// </summary>
    public DateTime? RestrictionUntil { get; set; }
}
