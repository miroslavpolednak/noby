using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed class GetCovenantsHandler(
    IProductServiceClient _productService, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetCovenantsRequest, CasesGetCovenantsResponse>
{
    public async Task<CasesGetCovenantsResponse> Handle(GetCovenantsRequest request, CancellationToken cancellationToken)
    {
        var sectionsCodebook = await _codebookService.CovenantTypes(cancellationToken);
        var list = await _productService.GetCovenantList(request.CaseId, cancellationToken);
        var covenantGroups = list.Covenants.GroupBy(t => t.CovenantTypeId);

        return new CasesGetCovenantsResponse
        {
            Sections = covenantGroups.Select(group =>
            {
                return new CasesGetCovenantsResponseSection
                {
                    CovenantTypeId = group.Key,
                    Phases = group
                        .GroupBy(g => g.PhaseOrder)
                        .OrderBy(o => o.Key)
                        .Select(t =>
                        {
                            var phase = list.Phases.FirstOrDefault(p => p.Order == t.Key);

                            return new CasesGetCovenantsResponsePhase
                            {
                                Name = phase?.Name,
                                OrderLetter = phase?.OrderLetter,
                                Covenants = t.Select(mapCovenant()).ToList()
                            };
                        })
                        .ToList()
                };
            })
            .OrderBy(o => sectionsCodebook.First(t => t.Id == o.CovenantTypeId).Order)
            .ToList()
        };
    }

    static Func<DomainServices.ProductService.Contracts.CovenantListItem, CasesGetCovenantsResponseCovenant> mapCovenant()
    {
        return t => new CasesGetCovenantsResponseCovenant
        {
            FulfillDate = t.FulfillDate,
            IsFulfilled = t.IsFulfilled,
            Name = t.Name,
            Order = t.Order,
            OrderLetter = t.OrderLetter
        };
    }
}
