namespace DomainServices.UserService.Api.Dto;

internal class GetUserByLoginMediatrRequest 
    : IRequest<Contracts.User>
{
    public string Login { get; init; }

    public GetUserByLoginMediatrRequest(Contracts.GetUserByLoginRequest request)
    {
        Login = request.Login;
    }
}
