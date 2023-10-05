namespace NOBY.Api.Endpoints.Household.UpdateCustomers.Dto;

internal sealed class CrudResult
{
    public int? OnHouseholdCustomerOnSAId { get; set; }
    
    public IEnumerable<SharedTypes.GrpcTypes.Identity>? Identities { get; set; }
    
    //public bool CancelSigning { get; set; }
    public Reasons Reason { get; set; }

    public CrudResult() { }

    public CrudResult(Reasons reason, int? onHouseholdCustomerOnSAId = null)
    {
        OnHouseholdCustomerOnSAId = onHouseholdCustomerOnSAId;
        Reason = reason;
    }

    public enum Reasons
    {
        None,
        CustomerRemoved,
        CustomerAdded,
        CustomerUpdated
    }
}
