using DomainServices.UserService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.UserService.Api.Endpoints.v1;

[Authorize]
internal sealed class UserService(IMediator _mediator)
    : Contracts.v1.UserService.UserServiceBase
{
    public override async Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetUserBasicInfoResponse> GetUserBasicInfo(GetUserBasicInfoRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetUserPermissionsResponse> GetUserPermissions(GetUserPermissionsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<UserRIPAttributes> GetUserRIPAttributes(GetUserRIPAttributesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

	public override async Task<GetUserMortgageSpecialistResponse> GetUserMortgageSpecialist(GetUserMortgageSpecialistRequest request, ServerCallContext context)
		=> await _mediator.Send(request, context.CancellationToken);
}
