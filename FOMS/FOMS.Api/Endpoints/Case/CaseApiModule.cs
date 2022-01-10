using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Case;

internal class CaseApiModule : IApiEndpointModule
{
    const string _prefix = "/api/case";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // detail case
        builder
            .MapGet(_prefix + "/{caseId:long}", async (long caseId) => await mediatr.Send(new Dto.GetByIdRequest(caseId)))
            .WithTags("Case Module")
            .Produces<Dto.CaseModel>(StatusCodes.Status200OK);

        // dashboard
        builder
            .MapPost(_prefix + "/search", async (Dto.SearchRequest request) => await mediatr.Send(request))
            .WithTags("Case Module")
            .Produces<Dto.SearchResponse>(StatusCodes.Status200OK);
    }
}
