namespace DomainServices.UserService.Api.Dto;

internal class GetUserMediatrRequest 
    : IRequest<Contracts.User>
{
    public int UserId { get; init; }

    public GetUserMediatrRequest(Contracts.GetUserRequest request)
    {
        UserId = request.UserId;
    }
}
