using DomainServices.DocumentOnSAService.Contracts;
using Grpc.Core;

namespace DomainServices.DocumentOnSAService.Api.Endpoints;

internal sealed class DocumentOnSAServiceGrpc : Contracts.v1.DocumentOnSAService.DocumentOnSAServiceBase
{
    private readonly IMediator _mediator;

    public DocumentOnSAServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GenerateFormIdResponse> GenerateFormId(GenerateFormIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

}
