using DomainServices.UserService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.UserService.Api.Services;

[Authorize]
internal class UserService : Contracts.v1.UserService.UserServiceBase
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<User> GetUserByLogin(GetUserByLoginRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetUserByLoginMediatrRequest(request), context.CancellationToken);

    public override async Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetUserMediatrRequest(request), context.CancellationToken);
}
