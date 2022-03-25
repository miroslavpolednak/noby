using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Services;

[Authorize]
internal class HouseholdService : Contracts.v1.HouseholdService.HouseholdServiceBase
{
    private readonly IMediator _mediator;

    public HouseholdService(IMediator mediator)
        => _mediator = mediator;
    
    public override async Task<CreateHouseholdResponse> CreateHousehold(CreateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateHouseholdMediatrRequest(request), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteHousehold(HouseholdIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteHouseholdMediatrRequest(request.HouseholdId), context.CancellationToken);
    
    public override async Task<Household> GetHousehold(HouseholdIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetHouseholdMediatrRequest(request.HouseholdId), context.CancellationToken);
    
    public override async Task<GetHouseholdListResponse> GetHouseholdList(GetHouseholdListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetHouseholdListMediatrRequest(request.SalesArrangementId), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateHousehold(UpdateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateHouseholdMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkCustomerOnSAToHousehold(LinkCustomerOnSAToHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.LinkCustomerOnSAToHouseholdMediatrRequest(request), context.CancellationToken);
}