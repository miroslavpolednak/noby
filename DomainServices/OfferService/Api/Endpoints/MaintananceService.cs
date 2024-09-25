using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Diagnostics.CodeAnalysis;

namespace DomainServices.OfferService.Api.Endpoints;

public sealed class MaintananceService(IMediator mediator)
        : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<Empty> ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, [NotNull] ServerCallContext context)
      => await mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteRefixationOfferOlderThan(DeleteRefixationOfferOlderThanRequest request, [NotNull]  ServerCallContext context)
     => await mediator.Send(request, context.CancellationToken);
}
