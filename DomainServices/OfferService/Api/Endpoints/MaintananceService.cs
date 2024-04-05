using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.OfferService.Api.Endpoints;

public sealed class MaintananceService(IMediator mediator)
        : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<Empty> ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, ServerCallContext context)
      => await mediator.Send(request, context.CancellationToken);
}
