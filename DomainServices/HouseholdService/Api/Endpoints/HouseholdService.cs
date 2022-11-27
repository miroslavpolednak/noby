using _C = DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Endpoints;

[Authorize]
internal class HouseholdService 
    : _C.v1.HouseholdService.HouseholdServiceBase
{
    public override async Task<_C.CreateHouseholdResponse> CreateHousehold(_C.CreateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.CreateHousehold.CreateHouseholdMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteHousehold(_C.DeleteHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.DeleteHousehold.DeleteHouseholdMediatrRequest(request.HouseholdId, request.HardDelete), context.CancellationToken);

    public override async Task<_C.Household> GetHousehold(_C.HouseholdIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.GetHousehold.GetHouseholdMediatrRequest(request.HouseholdId), context.CancellationToken);

    public override async Task<_C.GetHouseholdListResponse> GetHouseholdList(_C.GetHouseholdListRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.GetHouseholdList.GetHouseholdListMediatrRequest(request.SalesArrangementId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateHousehold(_C.UpdateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.UpdateHousehold.UpdateHouseholdMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkCustomerOnSAToHousehold(_C.LinkCustomerOnSAToHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Household.LinkCustomerOnSAToHousehold.LinkCustomerOnSAToHouseholdMediatrRequest(request), context.CancellationToken);

    private readonly IMediator _mediator;

    public HouseholdService(IMediator mediator)
        => _mediator = mediator;
}