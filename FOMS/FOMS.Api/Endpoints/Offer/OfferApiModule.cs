using CIS.Infrastructure.WebApi;

namespace FOMS.Api.Endpoints.Offer;

internal class OfferApiModule : IApiEndpointModule
{
    const string _prefix = "/api/offer";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // hypoteka - simulace
        builder
            .MapPost(_prefix + "/mortgage", async (Dto.SimulateMortgageRequest request) => await mediatr.Send(request))
            .WithTags("Offer Module")
            .Produces<Dto.SimulateMortgageResponse>(StatusCodes.Status200OK);
        
        // hypoteka - get
        builder
            .MapGet(_prefix + "/mortgage/{offerInstanceId:int}", async (int offerInstanceId) => await mediatr.Send(new Dto.GetMortgageRequest(offerInstanceId)))
            .WithTags("Offer Module")
            .Produces<Dto.GetMortgageResponse>(StatusCodes.Status200OK);
        
        // hypoteka - get for SA
        builder
            .MapGet(_prefix + "/mortgage/sales-arrangement/{salesArrangementId:int}", async (int salesArrangementId) => await mediatr.Send(new Dto.GetMortgageBySalesArrangementRequest(salesArrangementId)))
            .WithTags("Offer Module")
            .Produces<Dto.GetMortgageResponse>(StatusCodes.Status200OK);
    }
}
