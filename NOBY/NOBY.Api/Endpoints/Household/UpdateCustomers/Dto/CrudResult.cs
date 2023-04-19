using DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers.Dto;

internal sealed class CrudResult
{
    public int? DeletedCustomerOnSAId { get; set; }
    public long? DeletedPartnerId { get; set; }

    public int? CreatedCustomerOnSAId { get; set; }
    public long? CreatedPartnerId { get; set; }

    public int? OnHouseholdCustomerOnSAId { get; set; }
    
    public IEnumerable<CIS.Infrastructure.gRPC.CisTypes.Identity>? Identities { get; set; }
    
    public bool CancelSigning { get; set; }

    public CrudResult() { }

    public CrudResult(bool cancelSigning)
    {
        CancelSigning = cancelSigning;
    }

    public CrudResult(bool cancelSigning, int? onHouseholdCustomerOnSAId)
        : this(cancelSigning)
    {
        OnHouseholdCustomerOnSAId = onHouseholdCustomerOnSAId;
    }

    public CrudResult SetDeleted(List<CustomerOnSA> customers, int customerOnSAId)
    {
        var c = customers.FirstOrDefault(t => t.CustomerOnSAId == customerOnSAId);

        DeletedCustomerOnSAId = customerOnSAId;
        DeletedPartnerId = c?
            .CustomerIdentifiers?
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)?
            .IdentityId;

        return this;
    }

    public CrudResult SetCreated()
    {
        CreatedCustomerOnSAId = OnHouseholdCustomerOnSAId;
        CreatedPartnerId = Identities?
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)?
            .IdentityId;

        return this;
    }
}
