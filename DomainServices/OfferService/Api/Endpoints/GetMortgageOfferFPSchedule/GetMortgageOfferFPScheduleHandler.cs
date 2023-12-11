using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferFPSchedule;

internal sealed class GetMortgageOfferFPScheduleHandler
    : IRequestHandler<GetMortgageOfferFPScheduleRequest, GetMortgageOfferFPScheduleResponse>
{
    public async Task<GetMortgageOfferFPScheduleResponse> Handle(GetMortgageOfferFPScheduleRequest request, CancellationToken cancellationToken)
    {
        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellationToken)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellationToken)).ToDictionary(i => i.Id);

        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.OfferData>(request.OfferId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);
        
        // get simulation outputs
        var easSimulationReq = offerData!.Data!.SimulationInputs.ToEasSimulationRequest(offerData.Data.BasicParameters, drawingDurationsById, drawingTypeById);
        easSimulationReq.settings.enableResponseSplatkovyKalendarJednoduchy = 0;
        easSimulationReq.settings.enableResponseSplatkovyKalendarPlny = 1;
        var easSimulationRes = await _easSimulationHTClient.RunSimulationHT(easSimulationReq, cancellationToken);

        var fullPaymentSchedule = easSimulationRes.ToFullPaymentSchedule();

        var model = new GetMortgageOfferFPScheduleResponse { };
        model.PaymentScheduleFull.Add(fullPaymentSchedule);

        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ICodebookServiceClient _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public GetMortgageOfferFPScheduleHandler(
        IDocumentDataStorage documentDataStorage,
        ICodebookServiceClient codebookService, 
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient)
    {
        _documentDataStorage = documentDataStorage;
            _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
    }
}