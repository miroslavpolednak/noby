using DomainServices.RiskIntegrationService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Services;

[Authorize]
internal class RiskIntegrationService 
    : Contracts.v1.RiskIntegrationService.RiskIntegrationServiceBase
{
    private readonly IMediator _mediator;

    public RiskIntegrationService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<MyTestResponse> MyTest(MyTestRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.MyTestMediatrRequest(request), context.CancellationToken);
}
