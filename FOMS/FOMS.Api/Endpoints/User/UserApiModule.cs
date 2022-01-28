using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.User;

internal class UserApiModule : IApiEndpointModule
{
    const string _prefix = "/api/users";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // instance prihlaseneho uzivatele
        builder.MapGet(_prefix + "", async () => await mediatr.Send(new Dto.GetCurrentUserRequest()))
            .WithTags("User Module")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetCurrentUserResponse>(StatusCodes.Status200OK);

        // docasna autentizace
        builder.MapPost(_prefix + "/signin", async (Dto.SignInRequest request) => await mediatr.Send(request))
            .WithTags("User Module")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetCurrentUserResponse>(StatusCodes.Status200OK);
    }
}