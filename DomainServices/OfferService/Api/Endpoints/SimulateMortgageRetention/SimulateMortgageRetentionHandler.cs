using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using SharedComponents.DocumentDataStorage;
using SharedTypes.GrpcTypes;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler
    : IRequestHandler<SimulateMortgageRetentionRequest, SimulateMortgageRetentionResponse>
{
    public async Task<SimulateMortgageRetentionResponse> Handle(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        // get simulation outputs
        var easSimulationRes1 = await _easSimulationHTClient.RunSimulationRetention(request.CaseId, request.SimulationInputs.InterestRate, request.SimulationInputs.InterestRateValidFrom, cancellationToken);

        // doc entita
        var documentEntity = new Database.DocumentDataEntities.MortgageRetentionData
        {
            BasicParameters = _offerMapper.MapToDataBasicParameters(request.BasicParameters),
            SimulationInputs = _offerMapper.MapToDataInputs(request.SimulationInputs),
            SimulationOutputs = new()
            {
                LoanPaymentAmount = easSimulationRes1
            }
        };
        
        // druhy beh simulace se slevou
        if (request.SimulationInputs.InterestRateDiscount != null)
        {
            var easSimulationRes2 = await _easSimulationHTClient.RunSimulationRetention(request.CaseId, request.SimulationInputs.InterestRate - (decimal)request.SimulationInputs.InterestRateDiscount!, request.SimulationInputs.InterestRateValidFrom, cancellationToken);
            documentEntity.SimulationOutputs.LoanPaymentAmountDiscounted = easSimulationRes2;
        }

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = request.CaseId,
            OfferType = (int)OfferTypes.MortgageRetention,
            Origin = (int)OfferOrigins.OfferService
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data simulace
        await _documentDataStorage.Add(entity.OfferId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        return new SimulateMortgageRetentionResponse
        {
            OfferId = entity.OfferId,
            Created = new ModificationStamp(entity),
            SimulationInputs = _offerMapper.MapFromDataInputs(documentEntity.SimulationInputs),
            BasicParameters = _offerMapper.MapFromDataBasicParameters(documentEntity.BasicParameters),
            SimulationResults = _offerMapper.MapFromDataOutputs(documentEntity.SimulationOutputs)
        };
    }

    private readonly Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _offerMapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IEasSimulationHTClient _easSimulationHTClient;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi;
    private readonly Database.OfferServiceDbContext _dbContext;
    private readonly ILogger<SimulateMortgageRetentionHandler> _logger;

    public SimulateMortgageRetentionHandler(
        Database.OfferServiceDbContext dbContext,
        IEasSimulationHTClient easSimulationHTClient,
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper offerMapper,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApi,
        ILogger<SimulateMortgageRetentionHandler> logger)
    {
        _dbContext = dbContext;
        _easSimulationHTClient = easSimulationHTClient;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _sbWebApi = sbWebApi;
        _logger = logger;
    }
}
