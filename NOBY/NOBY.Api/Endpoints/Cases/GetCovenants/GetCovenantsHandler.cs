using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed class GetCovenantsHandler
    : IRequestHandler<GetCovenantsRequest, GetCovenantsResponse>
{
    public async Task<GetCovenantsResponse> Handle(GetCovenantsRequest request, CancellationToken cancellationToken)
    {
        var sectionsCodebook = await _codebookService.CovenantTypes(cancellationToken);
        var list = await _productService.GetCovenantList(request.CaseId, cancellationToken);
        var covenantGroups = list.Covenants.GroupBy(t => t.CovenantTypeId);

        return new GetCovenantsResponse
        {
            Sections = covenantGroups.Select(group =>
            {
                return new GetCovenantsResponseSection
                {
                    CovenantTypeId = group.Key,
                    Phases = group
                        .GroupBy(g => g.PhaseOrder)
                        .OrderBy(o => o.Key)
                        .Select(t =>
                        {
                            var phase = list.Phases.FirstOrDefault(p => p.Order == t.Key);

                            return new GetCovenantsResponsePhase()
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

    static Func<DomainServices.ProductService.Contracts.CovenantListItem, GetCovenantsResponseCovenant> mapCovenant()
    {
        return t => new GetCovenantsResponseCovenant
        {
            FulfillDate = t.FulfillDate,
            IsFulfilled = t.IsFulfilled,
            Name = t.Name,
            Order = t.Order,
            OrderLetter = t.OrderLetter
        };
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly IProductServiceClient _productService;

    public GetCovenantsHandler(IProductServiceClient productService, ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _productService = productService;
    }
}
