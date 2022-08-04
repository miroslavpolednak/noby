using _OS = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using Grpc.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.OfferService.Api.Repositories;
using Google.Protobuf;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateMortgageHandler
    : IRequestHandler<Dto.SimulateMortgageMediatrRequest, _OS.SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    private readonly OfferServiceDbContext _dbContext;

    public SimulateMortgageHandler(
        OfferServiceDbContext dbContext,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceAbstraction codebookService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient
        )
    {
        _logger = logger;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
        _dbContext = dbContext;
    }

    #endregion

    public async Task<_OS.SimulateMortgageResponse> Handle(Dto.SimulateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);

        // setup input default values
        var basicParameters = setUpDefaults(request.Request.BasicParameters, request.Request.SimulationInputs.GuaranteeDateFrom);
        var inputs = await setUpDefaults(request.Request.SimulationInputs, cancellation);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest(basicParameters);
        var easSimulationRes = resolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));
        var results = easSimulationRes.ToSimulationResults();
        var additionalResults = easSimulationRes.ToAdditionalSimulationResults();

        // save to DB
        var entity = new Repositories.Entities.Offer
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

        _logger.EntityCreated(nameof(Repositories.Entities.Offer), entity.OfferId);

        // create response
        return new _OS.SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = basicParameters,
            SimulationInputs = inputs,
            SimulationResults = results.AddAdditionalSimulationResults(additionalResults),
        };

    }

    private _OS.BasicParameters setUpDefaults(_OS.BasicParameters parameters, DateTime guaranteeDateFrom)
    {
        parameters = parameters ?? new _OS.BasicParameters();
        parameters.GuaranteeDateTo = guaranteeDateFrom.AddDays(AppDefaults.MaxGuaranteeInDays);
        parameters.StatementTypeId = parameters.StatementTypeId ?? 1;   // Default: 1
        return parameters;
    }

    private async Task<_OS.MortgageSimulationInputs> setUpDefaults(_OS.MortgageSimulationInputs input, CancellationToken cancellation)
    {
        input.ExpectedDateOfDrawing = input.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D

        if (!input.PaymentDay.HasValue)
        {
            input.PaymentDay = (await _codebookService.PaymentDays(cancellation)).FirstOrDefault(i => i.IsDefault)?.PaymentDay ?? throw new CisNotFoundException(99999, $"Default 'PaymentDay' not found.");
        }

        //Sleva z úrokové sazby(dle individuální cenotvorby)
        //Default: Pokud nepřijde na vstup, ukládáme hodnotu nula  "0"
        if (input.InterestRateDiscount == null)
        {
            input.InterestRateDiscount = 0;
        }

        // Typ čerpání
        // DrawingType(SB enum)
        // Default: "0" jednorázové
        if (!input.DrawingType.HasValue)
        {
            input.DrawingType = 0;
        }

        //Je žádáno zaměstnanecké zvýhodnění?
        //Default: False
        input.IsEmployeeBonusRequested = input.IsEmployeeBonusRequested ?? false;

        var defaultFeeSettings = new _OS.FeeSettings() { FeeTariffPurpose = 0, IsStatementCharged = true };

        input.FeeSettings = input.FeeSettings ?? defaultFeeSettings;

        // Určuje za jakým účelem se generuje seznam poplatků.
        // Default: 0 - za účelem nabídky
        input.FeeSettings.FeeTariffPurpose = input.FeeSettings.FeeTariffPurpose.HasValue ? input.FeeSettings.FeeTariffPurpose.Value : defaultFeeSettings.FeeTariffPurpose;

        return input;
    }

    private static ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse resolveRunSimulationHT(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
           ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
           _ => throw new NotImplementedException("RunSimulationHT")
       };
}
