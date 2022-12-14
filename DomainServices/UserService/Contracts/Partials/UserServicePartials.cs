namespace DomainServices.UserService.Contracts;

public partial class GetUserByLoginRequest
    : MediatR.IRequest<User>
{ }

public partial class GetUserRequest
    : MediatR.IRequest<User>
{ }