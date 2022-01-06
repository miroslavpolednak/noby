using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Test;

internal class TestApiModule : IApiEndpointModule
{
    const string _prefix = "/api/test";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // test string 200
        builder.MapGet(_prefix, () => mediatr.Send(new Dto.SimpleStringRequest()))
            .Produces<string>(StatusCodes.Status200OK)
            .ExcludeFromDescription();

        // test exception 400 - bad request (inline)
        builder.MapGet(_prefix + "/validation-problem", () => { throw new CIS.Core.Exceptions.CisValidationException("pekna chyba"); })
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription();

        // test exception 400 - bad request (IValidator)
        builder.MapPost(_prefix + "/validation-problem-2", (Dto.BadRequestRequest request) => mediatr.Send(request))
           .Produces<string>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .ExcludeFromDescription();

        // test exception 500
        builder.MapGet(_prefix + "/server-error", () => { throw new Exception("my test exception"); })
            .Produces(StatusCodes.Status500InternalServerError)
            .ExcludeFromDescription();
    }
}
