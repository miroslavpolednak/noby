using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Api.Database.DocumentDataEntities;
using DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.OfferService.Contracts;
using ExternalServices.Eas.Dto;
using ExternalServices.Eas.V1;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateBuildingSavings;

internal sealed class SimulateBuildingSavingsHandler : IRequestHandler<SimulateBuildingSavingsRequest, SimulateBuildingSavingsResponse>
{
    private readonly OfferServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IEasClient _easClient;
    private readonly BuildingSavingsDataMapper _dataMapper;
    private readonly ILogger<SimulateBuildingSavingsHandler> _logger;

    public SimulateBuildingSavingsHandler(
        OfferServiceDbContext dbContext, 
        IDocumentDataStorage documentDataStorage,
        IEasClient easClient, 
        BuildingSavingsDataMapper dataMapper,
        ILogger<SimulateBuildingSavingsHandler> logger)
    {
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _easClient = easClient;
        _dataMapper = dataMapper;
        _logger = logger;
    }

    public async Task<SimulateBuildingSavingsResponse> Handle(SimulateBuildingSavingsRequest request, CancellationToken cancellationToken)
    {
        var simulationResult = await _easClient.SimulateBuildingSavings(MapToEasBuildingSavingsRequest(request.SimulationInputs), cancellationToken);
        var domainResults = MapToDomainBuildingSavingsResults(simulationResult);

        var offer = new Database.Entities.Offer
        {
            OfferType = (int)OfferTypes.BuildingSavings,
            Origin = (int)OfferOrigins.OfferService
        };

        _dbContext.Offers.Add(offer);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var documentBuildingSavingsData = new BuildingSavingsData
        {
            SimulationInputs = _dataMapper.MapToDataInputs(request.SimulationInputs),
            SimulationOutputs = _dataMapper.MapToDataOutputs(domainResults),
        };

        await _documentDataStorage.Add(offer.OfferId, documentBuildingSavingsData, cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.Offer), offer.OfferId);

        return new SimulateBuildingSavingsResponse
        {
            OfferId = offer.OfferId,
            SimulationResults = domainResults
        };
    }

    private static BuildingSavingsRequest MapToEasBuildingSavingsRequest(BuildingSavingsSimulationInputs inputs)
    {
        return new BuildingSavingsRequest
        {
            MarketingActionCode = inputs.MarketingActionCode,
            TargetAmount = inputs.TargetAmount,
            MinimumMonthlyDeposit = inputs.MinimumMonthlyDeposit,
            ContractStartDate = inputs.ContractStartDate,
            SimulateUntilBindingPeriod = inputs.SimulateUntilBindingPeriod,
            ContractTerminationDate = inputs.ContractTerminationDate,
            AnnualStatementRequired = inputs.AnnualStatementRequired,
            StateSubsidyRequired = inputs.StateSubsidyRequired,
            IsClientSVJ = inputs.IsClientSVJ,
            IsClientJuridicalPerson = inputs.IsClientJuridicalPerson,
            ClientDateOfBirth = inputs.ClientDateOfBirth,
            ExtraDeposits = inputs.ExtraDeposits.Select(e => new BuildingSavingsRequest.ExtraDeposit
            {
                Date = e.Date,
                Amount = e.Amount
            }).ToList()
        };
    }

    private static BuildingSavingsSimulationResults MapToDomainBuildingSavingsResults(BuildingSavingsResponse response)
    {
        return new BuildingSavingsSimulationResults
        {
            SavingsLengthInMonths = response.SavingsLengthInMonths,
            InterestRate = response.InterestRate,
            SavingsSum = response.SavingsSum,
            DepositsSum = response.DepositsSum,
            InterestsSum = response.InterestsSum,
            FeesSum = response.FeesSum,
            BonusInterestRate = response.BonusInterestRate,
            StateSubsidySum = response.StateSubsidySum,
            InterestBenefitAmount = response.InterestBenefitAmount,
            InterestBenefitTax = response.InterestBenefitTax
        };
    }
}