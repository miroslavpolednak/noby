using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedComponents.DocumentDataStorage;
using SharedTypes.GrpcTypes;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionHandler(
    Database.OfferServiceDbContext _dbContext,
    IEasSimulationHTClient _easSimulationHTClient,
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _offerMapper,
    ILogger<SimulateMortgageRetentionHandler> _logger)
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
                LoanPaymentAmount = easSimulationRes1.LoanPaymentAmount
            }
        };

        // druhy beh simulace se slevou
        if (request.SimulationInputs.InterestRateDiscount != null)
        {
            var easSimulationRes2 = await _easSimulationHTClient.RunSimulationRetention(request.CaseId, request.SimulationInputs.InterestRate - (decimal)request.SimulationInputs.InterestRateDiscount!, request.SimulationInputs.InterestRateValidFrom, cancellationToken);
            documentEntity.SimulationOutputs.LoanPaymentAmountDiscounted = easSimulationRes2.LoanPaymentAmount;
        }

        // save to DB
        var entity = await saveEntity(request.CaseId, documentEntity, cancellationToken);

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

    private async Task<Database.Entities.Offer> saveEntity(long caseId, Database.DocumentDataEntities.MortgageRetentionData documentEntity, CancellationToken cancellationToken)
    {
        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = caseId,
            OfferType = (int)OfferTypes.MortgageRetention,
            Origin = (int)OfferOrigins.OfferService
        };
        _dbContext.Offers.Add(entity);

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                // ulozit json data simulace
                await _documentDataStorage.Add(_dbContext.Database.GetDbConnection(), transaction.GetDbTransaction(), entity.OfferId, documentEntity, cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.DatabaseRollbackInitiated(ex);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            return entity;
        });
    }
}
