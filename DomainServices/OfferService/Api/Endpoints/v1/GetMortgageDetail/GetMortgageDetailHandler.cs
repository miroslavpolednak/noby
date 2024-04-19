using DomainServices.OfferService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetMortgageDetail;

internal sealed class GetMortgageDetailHandler(
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.MortgageAdditionalSimulationResultsDataMapper _additionalResultsMapper,
    Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper _creditWorthinessMapper,
    Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper,
    IMediator _mediator)
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
}