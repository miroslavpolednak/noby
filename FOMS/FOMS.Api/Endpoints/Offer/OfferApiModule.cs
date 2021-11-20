using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Offer;

internal class OfferApiModule 
    : IApiEndpointModule
{
    const string _prefix = "/offers";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // simulace stavebniho sporeni
        builder.MapPost(_prefix + "/building-savings/simulate", (Dto.SimulateBuildingSavingsRequest request) => mediatr.Send(request))
            .Produces<Dto.OfferInstance>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        // instance nabidky
        builder.MapGet(_prefix + "/{offerInstanceId}", (int offerInstanceId) => mediatr.Send(new Dto.GetBuildingSavingsDataRequest(offerInstanceId)))
            .Produces<Dto.GetBuildingSavingsDataResponse>(StatusCodes.Status200OK);

        // stavebni sporeni - splatkovy kalendar
        builder.MapGet(_prefix + "/{offerInstanceId}/deposit-schedule", (int offerInstanceId) => mediatr.Send(new Dto.GetBuildingSavingsDepositScheduleRequest(offerInstanceId)))
            .Produces<Dto.GetBuildingSavingsDepositScheduleResponse>(StatusCodes.Status200OK);
    }
}
