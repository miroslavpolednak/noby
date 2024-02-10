using DomainServices.OfferService.Contracts;
using SharedComponents.DocumentDataStorage;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;

namespace DomainServices.OfferService.Api.Endpoints.GetOffer;

internal sealed class GetOfferHandler
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
        }

        return model;
    }

    private async Task<MortgageOfferBaseData> getMortgageData(int offerId, CancellationToken cancellationToken)
    {
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.MortgageOfferData>(offerId, cancellationToken);
        var data = _offerMapper.MapFromDataToSingle(offerData!.Data!.BasicParameters, offerData.Data.SimulationInputs, offerData.Data.SimulationOutputs);

        return new MortgageOfferBaseData
        {
            SimulationInputs = data.SimulationInputs,
            SimulationResults = data.SimulationResults,
            BasicParameters = data.BasicParameters
        };
    }

    private readonly IMediator _mediator;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper;

    public GetOfferHandler(
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper offerMapper,
        IMediator mediator)
    {
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _mediator = mediator;
    }
}