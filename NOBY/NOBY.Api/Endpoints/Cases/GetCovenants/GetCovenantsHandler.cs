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
                List<GetCovenantsResponsePhase> phases = list.Phases
                    .OrderBy(t => t.Order)
                    .Select(t => new GetCovenantsResponsePhase()
                    {
                        Name = t.Name,
                        OrderLetter = t.OrderLetter,
                        Covenants = list.Covenants
                            .Where(c => c.PhaseOrder == t.Order)
                            .Select(mapCovenant())
                            
                            .ToList()
                    })
                    .ToList();

                if (list.Covenants.Any(t => t.PhaseOrder == 0))
                {
                    phases.Insert(0, new()
                    {
                        Covenants = list.Covenants
                            .Where(t => t.PhaseOrder == 0)
                            .Select(mapCovenant())
                            .OrderBy(c => c.OrderLetter)
                            .ToList()
                    });
                }

                return new GetCovenantsResponseSection
                {
                    CovenantTypeId = group.Key,
                    Phases = phases
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
