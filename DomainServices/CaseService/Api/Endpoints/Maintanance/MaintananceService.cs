using DomainServices.CaseService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CaseService.Api.Endpoints.Maintanance;

[Authorize]
internal sealed class MaintananceService(IMediator _mediator)
    : Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<GetConfirmedPriceExceptionsResponse> GetConfirmedPriceExceptions(GetConfirmedPriceExceptionsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteConfirmedPriceException(DeleteConfirmedPriceExceptionRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);
}
