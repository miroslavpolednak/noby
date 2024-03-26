using DomainServices.MaintananceService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.OfferService.Api.Endpoints;

public sealed class MaintananceService(IMediator mediator)
        : DomainServices.MaintananceService.Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<Empty> ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, ServerCallContext context)
     => await mediator.Send(request, context.CancellationToken);
}
