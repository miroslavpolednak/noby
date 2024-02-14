using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Endpoints;

[Authorize]
internal sealed class MaintananceService
    : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<GetCancelCaseJobIdsResponse> GetCancelCaseJobIds(GetCancelCaseJobIdsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;

    public MaintananceService(IMediator mediator)
        => _mediator = mediator;
}
