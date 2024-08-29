using DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.v1;

[Authorize]
internal sealed class HouseholdService(IMediator _mediator)
    : Contracts.v1.HouseholdService.HouseholdServiceBase
{
    public override async Task<CreateHouseholdResponse> CreateHousehold(CreateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteHousehold(DeleteHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Contracts.Household> GetHousehold(GetHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetHouseholdListResponse> GetHouseholdList(GetHouseholdListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateHousehold(UpdateHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkCustomerOnSAToHousehold(LinkCustomerOnSAToHouseholdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetHouseholdIdByCustomerOnSAIdResponse> GetHouseholdIdByCustomerOnSAId(GetHouseholdIdByCustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateHouseholdIdResponse> ValidateHouseholdId(ValidateHouseholdIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);
}