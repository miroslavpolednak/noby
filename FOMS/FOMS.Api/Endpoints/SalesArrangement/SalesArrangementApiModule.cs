using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.SalesArrangement;

internal class SalesArrangementApiModule : IApiEndpointModule
{
    const string _prefix = "/api/sales-arrangements";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci cast SA pro zobrazeni formulare na UI
        builder
            .MapGet(_prefix + "/{salesArrangementId}/part/{partId}", async (int salesArrangementId, int partId) => await mediatr.Send(new Dto.GetPartRequest(salesArrangementId, partId)))
            .Produces<object>(StatusCodes.Status200OK);

        // ulozi cast SA z UI
        builder
            .MapPut(_prefix + "/{salesArrangementId}/part/{partId}", async ([FromRoute] int salesArrangementId, [FromRoute] int partId, [FromBody] object data) => await mediatr.Send(new Dto.SavePartRequest(salesArrangementId, partId, data)))
            .Produces(StatusCodes.Status200OK);

        // vraci detail SA bez dat
        builder
            .MapGet(_prefix + "/{salesArrangementId}", async (int salesArrangementId) => await mediatr.Send(new Dto.GetDetailRequest(salesArrangementId)))
            .Produces<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse>(StatusCodes.Status200OK);

        // GET /forms/{caseid}/{formid}/structure - vrati strukturu formu do wizarda
        // GET /forms/{said}/{formid}/{partid} - vrati template dane casti formu
        // PUT /forms/{said}/{formid}/{partid} - ulozi cast formu
    }
}
