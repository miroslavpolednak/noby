using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RealEstateValuationService.Api.Endpoints;

[Authorize]
internal sealed class RealEstateValuationService
    : Contracts.v1.RealEstateValuationService.RealEstateValuationServiceBase
{
    public override async Task<CreateRealEstateValuationResponse> CreateRealEstateValuation(CreateRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteRealEstateValuation(DeleteRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetRealEstateValuationListResponse> GetRealEstateValuationList(GetRealEstateValuationListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public RealEstateValuationService(IMediator mediator)
        => _mediator = mediator;
}
