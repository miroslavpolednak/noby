using CIS.Core.Attributes;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Contracts;
using static DomainServices.CodebookService.Contracts.v1.CodebookService;

namespace DomainServices.DocumentOnSAService.Api.Common;

public interface ICommonSigningMethods
{
    Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class CommonSigningMethods : ICommonSigningMethods
{
    private readonly ICodebookServiceClient _codebookService;

    public CommonSigningMethods(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }

    public async Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> GetSalesArrangementType(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        return salesArrangementTypes.Single(r => r.Id == salesArrangement.SalesArrangementTypeId);
    }
}
