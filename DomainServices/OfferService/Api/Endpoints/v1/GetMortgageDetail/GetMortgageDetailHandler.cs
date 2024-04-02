using DomainServices.OfferService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetMortgageDetail;

internal sealed class GetMortgageDetailHandler
    : IRequestHandler<GetMortgageDetailRequest, GetMortgageDetailResponse>
{
    public async Task<GetMortgageDetailResponse> Handle(GetMortgageDetailRequest request, CancellationToken cancellationToken)
    {
        var offerInstance = await _mediator.Send(new ValidateOfferIdRequest
        {
            OfferId = request.OfferId,
            ThrowExceptionIfNotFound = true
        }, cancellationToken);

        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageOfferData>(request.OfferId, cancellationToken);
        var data = _offerMapper.MapToFullData(offerData!.Data!);

        var additionalData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageAdditionalSimulationResultsData>(request.OfferId, cancellationToken);
        var mappedAdditionalData = _additionalResultsMapper.MapFromDataToSingle(additionalData!.Data);

        var worthinessData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageCreditWorthinessSimpleData>(request.OfferId, cancellationToken);
        var mappedWorthinessData = _creditWorthinessMapper.MapFromDataToSingle(worthinessData?.Data);

        return new GetMortgageDetailResponse
        {
            Data = offerInstance.Data,
            SimulationInputs = data.SimulationInputs,
            SimulationResults = data.SimulationResults,
            BasicParameters = data.BasicParameters,
            AdditionalSimulationResults = mappedAdditionalData,
            CreditWorthinessSimpleInputs = mappedWorthinessData.Inputs
        };
    }

    private readonly IMediator _mediator;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageAdditionalSimulationResultsDataMapper _additionalResultsMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper _creditWorthinessMapper;

    public GetMortgageDetailHandler(
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