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
            .MapGet(_prefix + "/mortgage/{offerId:int}", async (int offerId) => await mediatr.Send(new Dto.GetMortgageRequest(offerId)))
            .WithTags("Offer Module")
            .Produces<Dto.GetMortgageResponse>(StatusCodes.Status200OK);
        
        // hypoteka - get for SA
        builder
            .MapGet(_prefix + "/mortgage/sales-arrangement/{salesArrangementId:int}", async (int salesArrangementId) => await mediatr.Send(new Dto.GetMortgageBySalesArrangementRequest(salesArrangementId)))
            .WithTags("Offer Module")
            .Produces<Dto.GetMortgageResponse>(StatusCodes.Status200OK);
        
        // hypoteka - zalozeni case
        builder
            .MapPost(_prefix + "/mortgage/create-case", async (Dto.CreateCaseRequest request) => await mediatr.Send(request))
            .WithTags("Offer Module")
            .Produces<Dto.CreateCaseResponse>(StatusCodes.Status200OK);
    }
}
