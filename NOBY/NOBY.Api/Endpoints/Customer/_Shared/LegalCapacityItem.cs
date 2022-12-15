namespace NOBY.Api.Endpoints.Customer.Shared;

public sealed class LegalCapacityItem
{
    public int? RestrictionTypeId { get; set; }

    public DateTime? RestrictionUntil { get; set; }
}
