namespace DomainServices.RiskIntegrationService.Api.Endpoints.TestService.HalloWorld;

internal class HalloWorldHandler
    : IRequestHandler<Contracts.HalloWorldRequest, Contracts.HalloWorldResponse>
{
    public async Task<Contracts.HalloWorldResponse> Handle(Contracts.HalloWorldRequest request, CancellationToken cancellationToken)
    {
        var user = await _svc.GetUserData("990111a", "MPAD");

        return new Contracts.HalloWorldResponse { Name = $"My name is {request.Name} with {user.PersonID} / {user.PersonSurname}" };
    }

    private readonly Mpss.Rip.Infrastructure.Services.PersonDealer.IPersonDealerService _svc;

    public HalloWorldHandler(Mpss.Rip.Infrastructure.Services.PersonDealer.IPersonDealerService svc)
    {
        _svc = svc;
    }
}
