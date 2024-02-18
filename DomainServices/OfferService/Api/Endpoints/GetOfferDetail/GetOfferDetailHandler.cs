using DomainServices.OfferService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.GetOfferDetail;

internal sealed class GetOfferDetailHandler
    : IRequestHandler<GetOfferDetailRequest, GetOfferDetailResponse>
{
    public async Task<GetOfferDetailResponse> Handle(GetOfferDetailRequest request, CancellationToken cancellationToken)
    {
        var offerInstance = await _mediator.Send(new ValidateOfferIdRequest
        {
            OfferId = request.OfferId,
            ThrowExceptionIfNotFound = true
        }, cancellationToken);

        var model = new GetOfferDetailResponse
        {
            Data = offerInstance.Data
        };

        switch (offerInstance.Data.OfferType)
        {
            case OfferTypes.Mortgage:
                model.MortgageOffer = await getMortgageData(model.Data.OfferId, model.Data.IsCreditWorthinessSimpleRequested, cancellationToken);
                break;
        }

        return model;
    }

    private async Task<MortgageOfferFullData> getMortgageData(int offerId, bool isCreditWorthinessSimpleRequested, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageOfferData>(offerId, cancellationToken);
        var mappedOfferData = _offerMapper.MapFromDataToSingle(offerData!.Data!.BasicParameters, offerData.Data.SimulationInputs, offerData.Data.SimulationOutputs);

        var additionalData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData>(offerId, cancellationToken);
        var mappedAdditionalData = _additionalResultsMapper.MapFromDataToSingle(additionalData!.Data);

        var worthinessData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageCreditWorthinessSimpleData>(offerId, cancellationToken);
        var mappedWorthinessData = _creditWorthinessMapper.MapFromDataToSingle(worthinessData?.Data);

        return new MortgageOfferFullData
        {
            SimulationInputs = mappedOfferData.SimulationInputs,
            SimulationResults = mappedOfferData.SimulationResults,
            BasicParameters = mappedOfferData.BasicParameters,
            AdditionalSimulationResults = mappedAdditionalData,
            IsCreditWorthinessSimpleRequested = isCreditWorthinessSimpleRequested,
            CreditWorthinessSimpleInputs = mappedWorthinessData.Inputs
        };
    }

    private readonly IMediator _mediator;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageAdditionalSimulationResultsDataMapper _additionalResultsMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper _creditWorthinessMapper;

    public GetOfferDetailHandler(
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.MortgageAdditionalSimulationResultsDataMapper additionalResultsMapper,
        Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper creditWorthinessMapper,
        Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper offerMapper,
        IMediator mediator)
    {
        _additionalResultsMapper = additionalResultsMapper;
        _creditWorthinessMapper = creditWorthinessMapper;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _mediator = mediator;
    }
}