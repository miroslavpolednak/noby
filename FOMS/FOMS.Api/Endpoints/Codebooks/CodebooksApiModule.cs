using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.Codebooks;

internal class CodebooksApiModule : IApiEndpointModule
{
    const string _prefix = "/api/codebooks";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci vsechny vyzadane ciselniky
        // q = kody ciselniku oddelene ","
        builder.MapGet(_prefix + "/get-all", async ([FromQuery(Name = "q")] string codebookTypes) => await mediatr.Send(new Dto.GetAllRequest(codebookTypes)))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<Dto.GetAllResponseItem>(StatusCodes.Status200OK);
    }
}
