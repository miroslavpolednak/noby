using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal class GenerateExtraPaymentDocumentHandler : IRequestHandler<GenerateExtraPaymentDocumentRequest>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly MortgageRefinancingDocumentService _refinancingDocumentService;

    public GenerateExtraPaymentDocumentHandler(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService, MortgageRefinancingDocumentService refinancingDocumentService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _refinancingDocumentService = refinancingDocumentService;
    }

    public async Task Handle(GenerateExtraPaymentDocumentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageExtraPayment, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.OfferId!.Value, cancellationToken);
        
        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(null, null);

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, (int)SalesArrangementStates.InSigning, cancellationToken);

        throw new NotImplementedException();
    }

    private async Task<GetOfferResponse> LoadAndValidateOffer(int offerId, CancellationToken cancellationToken)
    {
        var offer = await _offerService.GetOffer(offerId, cancellationToken);

        //Validations

        return offer;
    }
}