using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed class GetCovenantsHandler
    : IRequestHandler<GetCovenantsRequest, GetCovenantsResponse>
{
    public async Task<GetCovenantsResponse> Handle(GetCovenantsRequest request, CancellationToken cancellationToken)
    {
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
                        .GroupBy(x => x.PhaseOrder)
                        .OrderBy(x => x)
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

    private readonly IProductServiceClient _productService;

    public GetCovenantsHandler(IProductServiceClient productService)
    {
        _productService = productService;
    }
}
