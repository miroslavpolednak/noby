using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class OfferApiModule 
    : IApiEndpointModule
{
    const string _prefix = "/api/savings/offers";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // simulace stavebniho sporeni
        builder
            .MapPost(_prefix, async ([FromBody] Dto.SimulateRequest request) => await mediatr.Send(request))
            .Produces<Dto.OfferInstance>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        // vytvoreni case z modelace - verze Ulozit a zavrit
        builder
            .MapPost(_prefix + "/{offerInstanceId}/create-case", async ([FromRoute] int offerInstanceId, [FromBody] Dto.CreateCase request) => await mediatr.Send(new Dto.CreateCaseRequest(offerInstanceId, request, false)))
            .Produces<int>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        // vytvoreni case z modelace - verze se zalozenim produktu
        builder
            .MapPost(_prefix + "/{offerInstanceId}/create-product", async ([FromRoute] int offerInstanceId, [FromBody] Dto.CreateCase request) => await mediatr.Send(new Dto.CreateCaseRequest(offerInstanceId, request, true)))
            .Produces<int>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        // instance nabidky
        builder
            .MapGet(_prefix + "/{offerInstanceId}", async (int offerInstanceId) => await mediatr.Send(new Dto.GetDataRequest(offerInstanceId)))
            .Produces<Dto.GetDataResponse>(StatusCodes.Status200OK);

        // stavebni sporeni - splatkovy kalendar
        builder
            .MapGet(_prefix + "/{offerInstanceId}/schedule", async ([FromRoute] int offerInstanceId, [FromQuery] int type) => await mediatr.Send(new Dto.GetScheduleRequest(offerInstanceId, type)))
            .Produces<Dto.GetScheduleResponse>(StatusCodes.Status200OK);
    }
}
