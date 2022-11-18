using DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Services;

[Authorize]
internal class HouseholdService : Contracts.v1.HouseholdService.HouseholdServiceBase
{
    private readonly IMediator _mediator;

    public HouseholdService(IMediator mediator)
        => _mediator = mediator;
    
    public override async Task<CreateHouseholdResponse> CreateHousehold(CreateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.CreateHousehold.CreateHouseholdMediatrRequest(request), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteHousehold(DeleteHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.DeleteHousehold.DeleteHouseholdMediatrRequest(request.HouseholdId, request.HardDelete), context.CancellationToken);
    
    public override async Task<Household> GetHousehold(HouseholdIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.GetHousehold.GetHouseholdMediatrRequest(request.HouseholdId), context.CancellationToken);
    
    public override async Task<GetHouseholdListResponse> GetHouseholdList(GetHouseholdListRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.GetHouseholdList.GetHouseholdListMediatrRequest(request.SalesArrangementId), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateHousehold(UpdateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.UpdateHousehold.UpdateHouseholdMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkCustomerOnSAToHousehold(LinkCustomerOnSAToHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.Household.LinkCustomerOnSAToHousehold.LinkCustomerOnSAToHouseholdMediatrRequest(request), context.CancellationToken);
}