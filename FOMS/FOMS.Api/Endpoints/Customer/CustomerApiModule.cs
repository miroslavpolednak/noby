using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.Customer;

internal class CustomerApiModule : IApiEndpointModule
{
    const string _prefix = "/api/customer";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci klienta podle tokenu z externi app
        builder.MapGet(_prefix + "/from-context", async ([FromQuery] string token) => await mediatr.Send(new Dto.GetFromContextRequest(token)))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetFromContextResponse>(StatusCodes.Status200OK);

        // vyhledani klienta
        builder.MapPost(_prefix + "/search", async ([FromBody] Dto.SearchRequest request) => await mediatr.Send(request))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetFromContextResponse>(StatusCodes.Status200OK);
    }
}
