using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.User;

internal class UserApiModule : IApiEndpointModule
{
    const string _prefix = "/api/users";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // instance prihlaseneho uzivatele
        builder.MapGet(_prefix + "", () => mediatr.Send(new Dto.GetCurrentUserRequest()))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetCurrentUserResponse>(StatusCodes.Status200OK);
    }
}