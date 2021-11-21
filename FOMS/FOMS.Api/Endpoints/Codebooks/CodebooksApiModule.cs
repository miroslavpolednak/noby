using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Codebooks;

internal class CodebooksApiModule : IApiEndpointModule
{
    const string _prefix = "/codebooks";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci vsechny vyzadane ciselniky
        // q = kody ciselniku oddelene ","
        builder.MapGet(_prefix + "/get-all", (string q) => mediatr.Send(new Dto.GetAllRequest(q)))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetAllResponse>(StatusCodes.Status200OK);
    }
}
