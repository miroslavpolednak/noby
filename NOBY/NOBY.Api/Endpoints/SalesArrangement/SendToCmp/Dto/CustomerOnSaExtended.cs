using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp.Dto;

internal class CustomerOnSaExtended
{
    public required Identity IdentityMp { get; init; }

    public required Identity IdentityKb { get; init; }

    public required CustomerOnSA Customer { get; init; }
}