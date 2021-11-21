namespace FOMS.Api.Endpoints.User.Handlers;

internal class GetCurrentUserHandler
    : IRequestHandler<Dto.GetCurrentUserRequest, Dto.GetCurrentUserResponse>
{
    public Task<Dto.GetCurrentUserResponse> Handle(Dto.GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var instance = new Dto.GetCurrentUserResponse()
        {
            Id = 1,
            Name = "John Doe"
        };

        return Task.FromResult(instance);
    }
}