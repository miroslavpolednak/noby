﻿using DomainServices.OfferService.Contracts;
using ExternalServices.EasSimulationHT.V1;
using SharedComponents.DocumentDataStorage;
using SharedTypes.GrpcTypes;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler
    : IRequestHandler<SimulateMortgageRefixationRequest, SimulateMortgageRefixationResponse>
{
    public async Task<SimulateMortgageRefixationResponse> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // ziskat urokovou sazbu
        var interestRate = await _sbWebApi.GetRefixationInterestRate(request.CaseId, DateTime.Now(), cancellationToken);

        // get simulation outputs
        var easSimulationRes1 = await _easSimulationHTClient.RunSimulationRefixation(request.CaseId, interestRate.InterestRate, request.SimulationInputs.FixedRatePeriod, request.SimulationInputs.FutureInterestRateValidTo, cancellationToken);

        // doc entita
        var documentEntity = new Database.DocumentDataEntities.MortgageRefixationData
        {
            BasicParameters = _offerMapper.MapToDataBasicParameters(request.BasicParameters),
            SimulationInputs = _offerMapper.MapToDataInputs(request.SimulationInputs, interestRate.InterestRate),
            SimulationOutputs = new()
            {
                LoanPaymentAmount = easSimulationRes1
            }
        };

        // druhy beh simulace se slevou
        if (request.SimulationInputs.InterestRateDiscount != null)
        {
            var easSimulationRes2 = await _easSimulationHTClient.RunSimulationRefixation(request.CaseId, interestRate.InterestRate - (decimal)request.SimulationInputs.InterestRateDiscount!, request.SimulationInputs.FixedRatePeriod, request.SimulationInputs.FutureInterestRateValidTo, cancellationToken);
            documentEntity.SimulationOutputs.LoanPaymentAmountDiscounted = easSimulationRes2;
        }

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.NewGuid(),
            CaseId = request.CaseId,
            OfferType = (int)OfferTypes.MortgageRefixation
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

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IEasSimulationHTClient _easSimulationHTClient;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi;
    private readonly Database.OfferServiceDbContext _dbContext;
    private readonly ILogger<SimulateMortgageRefixationHandler> _logger;

    public SimulateMortgageRefixationHandler(IEasSimulationHTClient easSimulationHTClient, ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApi, Database.OfferServiceDbContext dbContext, ILogger<SimulateMortgageRefixationHandler> logger, IDocumentDataStorage documentDataStorage)
    {
        _easSimulationHTClient = easSimulationHTClient;
        _sbWebApi = sbWebApi;
        _dbContext = dbContext;
        _logger = logger;
        _documentDataStorage = documentDataStorage;
    }
}
