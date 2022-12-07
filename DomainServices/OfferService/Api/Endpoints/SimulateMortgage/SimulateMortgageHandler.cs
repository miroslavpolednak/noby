using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.OfferService.Api.Database;
using Google.Protobuf;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgage;

internal sealed class SimulateMortgageHandler
    : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly ICodebookServiceClients _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    private readonly OfferServiceDbContext _dbContext;

    public SimulateMortgageHandler(
        OfferServiceDbContext dbContext,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceClients codebookService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient
        )
    {
        _logger = logger;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
        _dbContext = dbContext;
    }

    #endregion

    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellation)
    {
        var resourceProcessId = Guid.Parse(request.ResourceProcessId);

        // setup input default values
        var basicParameters = setUpDefaults(request.BasicParameters, request.SimulationInputs.GuaranteeDateFrom);
        var inputs = await setUpDefaults(request.SimulationInputs, cancellation);

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellation)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellation)).ToDictionary(i => i.Id);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest(basicParameters, drawingDurationsById, drawingTypeById);
        var easSimulationRes = resolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));
        var results = easSimulationRes.ToSimulationResults();
        var additionalResults = easSimulationRes.ToAdditionalSimulationResults();

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = resourceProcessId,
            BasicParameters = Newtonsoft.Json.JsonConvert.SerializeObject(basicParameters),
            SimulationInputs = Newtonsoft.Json.JsonConvert.SerializeObject(inputs),
            SimulationResults = Newtonsoft.Json.JsonConvert.SerializeObject(results),
            AdditionalSimulationResults = Newtonsoft.Json.JsonConvert.SerializeObject(additionalResults),
            BasicParametersBin = basicParameters.ToByteArray(),
            SimulationInputsBin = inputs.ToByteArray(),
            SimulationResultsBin = results.ToByteArray(),
            AdditionalSimulationResultsBin = additionalResults.ToByteArray()
        };
        _dbContext.Offers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = basicParameters,
            SimulationInputs = inputs,
            SimulationResults = results,
            AdditionalSimulationResults = additionalResults
        };

    }

    private BasicParameters setUpDefaults(BasicParameters parameters, DateTime guaranteeDateFrom)
    {
        parameters = parameters ?? new BasicParameters();
        parameters.GuaranteeDateTo = guaranteeDateFrom.AddDays(AppDefaults.MaxGuaranteeInDays);
        parameters.StatementTypeId = parameters.StatementTypeId ?? 1;   // Default: 1
        return parameters;
    }

    private async Task<MortgageSimulationInputs> setUpDefaults(MortgageSimulationInputs input, CancellationToken cancellation)
    {
        input.ExpectedDateOfDrawing = input.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D

        if (!input.PaymentDay.HasValue)
        {
            input.PaymentDay = (await _codebookService.PaymentDays(cancellation)).FirstOrDefault(i => i.IsDefault)?.PaymentDay ?? throw new CisNotFoundException(10002, $"Default 'PaymentDay' not found.");
        }

        //Sleva z úrokové sazby(dle individuální cenotvorby)
        //Default: Pokud nepřijde na vstup, ukládáme hodnotu nula  "0"
        if (input.InterestRateDiscount == null)
        {
            input.InterestRateDiscount = 0;
        }

        // Typ čerpání
        // DrawingType(SB enum)
        // Default: "2" jednorázové
        if (!input.DrawingTypeId.HasValue)
        {
            input.DrawingTypeId = (int)CIS.Foms.Enums.DrawingTypes.Disposable;
        }

        //Je žádáno zaměstnanecké zvýhodnění?
        //Default: False
        input.IsEmployeeBonusRequested = input.IsEmployeeBonusRequested ?? false;

        var defaultFeeSettings = new FeeSettings() { FeeTariffPurpose = 0, IsStatementCharged = true };

        input.FeeSettings = input.FeeSettings ?? defaultFeeSettings;

        // Určuje za jakým účelem se generuje seznam poplatků.
        // Default: 0 - za účelem nabídky
        input.FeeSettings.FeeTariffPurpose = input.FeeSettings.FeeTariffPurpose.HasValue ? input.FeeSettings.FeeTariffPurpose.Value : defaultFeeSettings.FeeTariffPurpose;

        // Rizikové životní pojištění
        if (input.RiskLifeInsurance != null)
        {
            input.RiskLifeInsurance.Frequency = input.RiskLifeInsurance.Frequency ?? 1;         // Default: 1
        }

        // Pojištění nemovitosti
        if (input.RealEstateInsurance != null)
        {
            input.RealEstateInsurance.Frequency = input.RealEstateInsurance.Frequency ?? 12;    // Default: 12
        }

        return input;
    }

    private static EasSimulationHT.EasSimulationHTWrapper.SimulationHTResponse resolveRunSimulationHT(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<EasSimulationHT.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
           _ => throw new NotImplementedException("RunSimulationHT")
       };
}
