using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using SharedComponents.DocumentDataStorage;
using SharedTypes.GrpcTypes;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler(
    IRollbackBag _bag,
    Database.OfferServiceDbContext _dbContext,
    IEasSimulationHTClient _easSimulationHTClient,
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.MortgageExtraPaymentDataMapper _offerMapper,
    ILogger<SimulateMortgageExtraPaymentHandler> _logger)
    : IRequestHandler<SimulateMortgageExtraPaymentRequest, SimulateMortgageExtraPaymentResponse>
{
    public async Task<SimulateMortgageExtraPaymentResponse> Handle(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        // get simulation outputs
        var easSimulationRes = await _easSimulationHTClient.RunSimulationExtraPayment(request.CaseId, request.SimulationInputs.ExtraPaymentDate, request.SimulationInputs.ExtraPaymentAmount, request.SimulationInputs.ExtraPaymentReasonId, request.SimulationInputs.IsExtraPaymentFullyRepaid, cancellationToken);

        // doc entita
        var documentEntity = new Database.DocumentDataEntities.MortgageExtraPaymentData
        {
            BasicParameters = _offerMapper.MapToDataBasicParameters(request.BasicParameters),
            SimulationInputs = _offerMapper.MapToDataInputs(request.SimulationInputs),
            SimulationOutputs = _offerMapper.MapToDataOutputs(easSimulationRes, request.SimulationInputs)
        };

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = request.CaseId,
            OfferType = (int)OfferTypes.MortgageExtraPayment,
            Origin = (int)OfferOrigins.OfferService
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
        _bag.Add(SimulateMortgageExtraPaymentRollback.BagKeyOfferId, entity.OfferId);

        // ulozit json data simulace
        await _documentDataStorage.Add(entity.OfferId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        return new SimulateMortgageExtraPaymentResponse
        {
            OfferId = entity.OfferId,
            Created = new ModificationStamp(entity),
            SimulationInputs = _offerMapper.MapFromDataInputs(documentEntity.SimulationInputs),
            BasicParameters = _offerMapper.MapFromDataBasicParameters(documentEntity.BasicParameters),
            SimulationResults = _offerMapper.MapFromDataOutputs(documentEntity.SimulationOutputs)
        };
    }
}
