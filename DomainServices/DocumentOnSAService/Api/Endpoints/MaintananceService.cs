using DomainServices.DocumentOnSAService.Contracts;
using Grpc.Core;

namespace DomainServices.DocumentOnSAService.Api.Endpoints;

[Authorize]
internal sealed class MaintananceService
    : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<GetUpdateDocumentStatusIdsResponse> GetUpdateDocumentStatusIds(GetUpdateDocumentStatusIdsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;

    public MaintananceService(IMediator mediator)
        => _mediator = mediator;
}
