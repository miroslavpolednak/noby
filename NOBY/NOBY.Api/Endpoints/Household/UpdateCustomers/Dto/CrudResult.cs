namespace NOBY.Api.Endpoints.Household.UpdateCustomers.Dto;

internal sealed class CrudResult
{
    public int? OnHouseholdCustomerOnSAId { get; set; }
    
    public IEnumerable<CIS.Infrastructure.gRPC.CisTypes.Identity>? Identities { get; set; }
    
    public bool CancelSigning { get; set; }

    public CrudResult()
    {
        CancelSigning = false;
    }

    public CrudResult(bool cancelSigning)
    {
        CancelSigning = cancelSigning;
    }

    public CrudResult(bool cancelSigning, int onHouseholdCustomerOnSAId)
        : this(cancelSigning)
    {
        OnHouseholdCustomerOnSAId = onHouseholdCustomerOnSAId;
    }
}
