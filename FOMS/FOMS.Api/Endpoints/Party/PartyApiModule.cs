using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Party;

internal class PartyApiModule : IApiEndpointModule
{
    const string _prefix = "/api/party";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci klienta podle tokenu z externi app
        builder.MapGet(_prefix + "/from-context", async (string token) => await mediatr.Send(new Dto.GetFromContextRequest(token)))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetFromContextResponse>(StatusCodes.Status200OK);
    }
}
