using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler
    : IRequestHandler<SimulateMortgageRetentionRequest, SimulateMortgageRetentionResponse>
{
    public async Task<SimulateMortgageRetentionResponse> Handle(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        var interestRate = await _sbWebApi.GetRefixationInterestRate(request.CaseId, DateTime.Now.Date, cancellationToken);

        var mappedInputs = _offerMapper.MapToDataInputs(request.SimulationInputs);
        var mappedBasicParams = _offerMapper.MapToDataBasicParameters(request.BasicParameters);

        // get simulation outputs
        var x = new ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper.SimHu_RetenceHedge_Request
        {
            settings = new ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper.SimRetenceHedgeSettings
            {
                uverId = 1,
                mode = 1,
                s
            }
        };
        var easSimulationRes = await _easSimulationHTClient.RunSimulationRetention(new ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper.SimHu_RetenceHedge_Request, cancellationToken);

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = request.CaseId,
            OfferType = (int)OfferTypes.MortgageRetention
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _offerMapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IEasSimulationHTClient _easSimulationHTClient;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi;
    private readonly Database.OfferServiceDbContext _dbContext;

    public SimulateMortgageRetentionHandler(
        Database.OfferServiceDbContext dbContext,
        IEasSimulationHTClient easSimulationHTClient,
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper offerMapper,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApi)
    {
        _dbContext = dbContext;
        _easSimulationHTClient = easSimulationHTClient;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _sbWebApi = sbWebApi;
    }
}
