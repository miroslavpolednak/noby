using DomainServices.UserService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.UserService.Api.Endpoints;

[Authorize]
internal class UserService 
    : Contracts.v1.UserService.UserServiceBase
{
    public override async Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetUserPermissionsResponse> GetUserPermissions(GetUserPermissionsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<UserRIPAttributes> GetUserRIPAttributes(GetUserRIPAttributesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public UserService(IMediator mediator)
        => _mediator = mediator;
}
