using DomainServices.CodebookService.Contracts;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Endpoints;

[Authorize]
internal sealed class MaintananceService
    : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<Empty> DownloadRdmCodebooks(DownloadRdmCodebooksRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;

    public MaintananceService(IMediator mediator)
        => _mediator = mediator;
}
