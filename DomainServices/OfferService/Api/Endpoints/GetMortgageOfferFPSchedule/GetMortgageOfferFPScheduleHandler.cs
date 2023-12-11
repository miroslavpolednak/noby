using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferFPSchedule;

internal sealed class GetMortgageOfferFPScheduleHandler
    : IRequestHandler<GetMortgageOfferFPScheduleRequest, GetMortgageOfferFPScheduleResponse>
{
    public async Task<GetMortgageOfferFPScheduleResponse> Handle(GetMortgageOfferFPScheduleRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellationToken)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellationToken)).ToDictionary(i => i.Id);

        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.OfferData>(request.OfferId, cancellationToken);
        var mappedOfferData = _offerMapper.MapFromDataToSingle(offerData!.Data!.BasicParameters, offerData.Data.SimulationInputs, offerData.Data.SimulationOutputs);
        
        // get simulation outputs
        var easSimulationReq = mappedOfferData.SimulationInputs.ToEasSimulationRequest(mappedOfferData.BasicParameters, drawingDurationsById, drawingTypeById).ToEasSimulationFullPaymentScheduleRequest();
        var easSimulationRes = await _easSimulationHTClient.RunSimulationHT(easSimulationReq, cancellationToken);

        var fullPaymentSchedule = easSimulationRes.ToFullPaymentSchedule();

        var model = new GetMortgageOfferFPScheduleResponse { };
        model.PaymentScheduleFull.Add(fullPaymentSchedule);

        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.OfferDataMapper _offerMapper;
    private readonly Database.OfferServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public GetMortgageOfferFPScheduleHandler(
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.OfferDataMapper offerMapper,
        Database.OfferServiceDbContext dbContext, 
        ICodebookServiceClient codebookService, 
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient)
    {
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _dbContext = dbContext;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
    }
}