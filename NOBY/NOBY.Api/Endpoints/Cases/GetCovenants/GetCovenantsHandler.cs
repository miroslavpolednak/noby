using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenants;

internal sealed class GetCovenantsHandler
    : IRequestHandler<GetCovenantsRequest, GetCovenantsResponse>
{
    public async Task<GetCovenantsResponse> Handle(GetCovenantsRequest request, CancellationToken cancellationToken)
    {
        return new GetCovenantsResponse();
    }

    private readonly ICaseServiceClient _caseService;

    public GetCovenantsHandler(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }
}
