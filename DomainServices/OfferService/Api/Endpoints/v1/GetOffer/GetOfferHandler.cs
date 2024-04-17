using DomainServices.OfferService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOffer;

internal sealed class GetOfferHandler(
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper,
    IMediator _mediator,
    Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _retentionMapper,
    Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper _refixationMapper,
    Database.DocumentDataEntities.Mappers.MortgageExtraPaymentDataMapper _extraPaymentDataMapper)
        : IRequestHandler<GetOfferRequest, GetOfferResponse>
{
    public async Task<GetOfferResponse> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        var offerInstance = await _mediator.Send(new ValidateOfferIdRequest
        {
            OfferId = request.OfferId,
            ThrowExceptionIfNotFound = true
        }, cancellationToken);

        var model = new GetOfferResponse
        {
            Data = offerInstance.Data
        };

        switch (offerInstance.Data.OfferType)
        {
            case OfferTypes.Mortgage:
                model.MortgageOffer = await getMortgageData(model.Data.OfferId, cancellationToken);
                break;

            case OfferTypes.MortgageRetention:
                model.MortgageRetention = await getRetentionData(model.Data.OfferId, cancellationToken);
                break;

            case OfferTypes.MortgageRefixation:
                model.MortgageRefixation = await getRefixationData(model.Data.OfferId, cancellationToken);
                break;

            case OfferTypes.MortgageExtraPayment:
                model.MortgageExtraPayment = await getExtraPaymentData(model.Data.OfferId, cancellationToken);
                break;
        }

        return model;
    }

    private async Task<MortgageRefixationFullData> getRefixationData(int offerId, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageRefixationData>(offerId, cancellationToken);
        return _refixationMapper.MapToFullData(offerData!.Data!);
    }

    private async Task<MortgageExtraPaymentFullData> getExtraPaymentData(int offerId, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageExtraPaymentData>(offerId, cancellationToken);
        return _extraPaymentDataMapper.MapToFullData(offerData!.Data!);
    }

    private async Task<MortgageRetentionFullData> getRetentionData(int offerId, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageRetentionData>(offerId, cancellationToken);
        return _retentionMapper.MapToFullData(offerData!.Data!);
    }

    private async Task<MortgageOfferFullData> getMortgageData(int offerId, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageOfferData>(offerId, cancellationToken);
        return _offerMapper.MapToFullData(offerData!.Data!);

    }
}