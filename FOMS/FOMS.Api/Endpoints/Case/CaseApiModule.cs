using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Case;

internal class CaseApiModule : IApiEndpointModule
{
    const string _prefix = "/api/case";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        builder
            .MapGet(_prefix + "/{caseId}", async (long caseId) => await mediatr.Send(new Dto.GetCaseRequest(caseId)))
            .Produces<Dto.GetCaseResponse>(StatusCodes.Status200OK);
    }
}
