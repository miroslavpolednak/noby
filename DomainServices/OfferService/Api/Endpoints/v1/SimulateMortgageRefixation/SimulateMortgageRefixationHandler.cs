using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;
using SharedTypes.GrpcTypes;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler
    : IRequestHandler<SimulateMortgageRefixationRequest, SimulateMortgageRefixationResponse>
{
    public async Task<SimulateMortgageRefixationResponse> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // get simulation outputs
        var easSimulationRes1 = await _easSimulationHTClient.RunSimulationRefixation(request.CaseId, request.SimulationInputs.InterestRate, request.SimulationInputs.InterestRateValidFrom, request.SimulationInputs.FixedRatePeriod, cancellationToken);

        // doc entita
        var documentEntity = new Database.DocumentDataEntities.MortgageRefixationData
        {
            BasicParameters = _offerMapper.MapToDataBasicParameters(request.BasicParameters),
            SimulationInputs = _offerMapper.MapToDataInputs(request.SimulationInputs),
            SimulationOutputs = new()
            {
                LoanPaymentAmount = easSimulationRes1.LoanPaymentAmount,
                LoanPaymentsCount = easSimulationRes1.LoanPaymentsCount,
                MaturityDate = easSimulationRes1.MaturityDate
            }
        };

        // druhy beh simulace se slevou
        if (request.SimulationInputs.InterestRateDiscount != null)
        {
            var easSimulationRes2 = await _easSimulationHTClient.RunSimulationRefixation(request.CaseId, request.SimulationInputs.InterestRate - (decimal)request.SimulationInputs.InterestRateDiscount!, request.SimulationInputs.InterestRateValidFrom, request.SimulationInputs.FixedRatePeriod, cancellationToken);
            documentEntity.SimulationOutputs.LoanPaymentAmountDiscounted = easSimulationRes2.LoanPaymentAmount;
        }

        var result = new SimulateMortgageRefixationResponse
        {
            SimulationInputs = _offerMapper.MapFromDataInputs(documentEntity.SimulationInputs),
            BasicParameters = _offerMapper.MapFromDataBasicParameters(documentEntity.BasicParameters),
            SimulationResults = _offerMapper.MapFromDataOutputs(documentEntity.SimulationOutputs)
        };

        if (request.IsVirtual)
        {
            result.OfferId = request.OfferId.GetValueOrDefault();
        }
        else if (request.OfferId.HasValue)
        {
            result.Created = await updateOffer(request.OfferId.Value, documentEntity, cancellationToken);
            result.OfferId = request.OfferId.Value;
        }
        else
        {
            var (offerId, stamp) = await insertOffer(request.CaseId, request.ValidTo, documentEntity, cancellationToken);
            result.Created = stamp;
            result.OfferId = offerId;
        }

        return result;
    }

    private async Task<(int OfferId, ModificationStamp Stamp)> insertOffer(long caseId, DateTime? validTo, Database.DocumentDataEntities.MortgageRefixationData documentEntity, CancellationToken cancellationToken)
    {
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = caseId,
            OfferType = (int)OfferTypes.MortgageRefixation,
            Origin = (int)OfferOrigins.OfferService,
            Flags = (int)OfferFlagTypes.Current,
            ValidTo = validTo
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data simulace
        await _documentDataStorage.Add(entity.OfferId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        return (entity.OfferId, new ModificationStamp(entity));
    }

    private async Task<ModificationStamp> updateOffer(int offerId, Database.DocumentDataEntities.MortgageRefixationData documentEntity, CancellationToken cancellationToken)
    {
        // kontrola zda offer existuje
        var entity = await _dbContext.Offers
            .Where(t => t.OfferId == offerId)
            .Select(t => new { t.CreatedTime, t.CreatedUserId, t.CreatedUserName })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, offerId);

        await _documentDataStorage.UpdateByEntityId(offerId, documentEntity);

        return new ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime);
    }

    private readonly Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper _offerMapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IEasSimulationHTClient _easSimulationHTClient;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi;
    private readonly Database.OfferServiceDbContext _dbContext;
    private readonly ILogger<SimulateMortgageRefixationHandler> _logger;

    public SimulateMortgageRefixationHandler(IEasSimulationHTClient easSimulationHTClient, ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApi, Database.OfferServiceDbContext dbContext, ILogger<SimulateMortgageRefixationHandler> logger, IDocumentDataStorage documentDataStorage, Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper offerMapper)
    {
        _easSimulationHTClient = easSimulationHTClient;
        _sbWebApi = sbWebApi;
        _dbContext = dbContext;
        _logger = logger;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
    }
}
