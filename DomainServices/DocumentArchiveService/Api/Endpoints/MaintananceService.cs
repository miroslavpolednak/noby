using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.DocumentArchiveService.Api.Endpoints;

public class MaintananceService(IMediator mediator) : Contracts.MaintananceService.MaintananceServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<Empty> DeleteDocumentDataFromArchiveQueue(DeleteDataFromArchiveQueueRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);
}
