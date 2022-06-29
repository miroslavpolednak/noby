using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using Grpc.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateMortgageHandler
    : BaseHandler, IRequestHandler<Dto.SimulateMortgageMediatrRequest, SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly Eas.IEasClient _easClient;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public SimulateMortgageHandler(
        Repositories.OfferRepository repository,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient
        ) : base(repository, codebookService)
    {
        _logger = logger;
        _easClient = easClient;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
    }

    #endregion

    public async Task<SimulateMortgageResponse> Handle(Dto.SimulateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);

        // setup input default values
        var basicParameters = SetUpDefaults(request.Request.BasicParameters, request.Request.SimulationInputs.GuaranteeDateFrom);
        var inputs = await SetUpDefaults(request.Request.SimulationInputs, cancellation);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest();
        var easSimulationRes = ResolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));
        var results = easSimulationRes.ToSimulationResults();

        // save to DB
        var entity = await _repository.SaveOffer(resourceProcessId, basicParameters, inputs, results, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = basicParameters,
            SimulationInputs = inputs,
            SimulationResults = results,
        };

    }

    private BasicParameters SetUpDefaults(BasicParameters parameters, DateTime guaranteeDateFrom)
    {
        parameters = parameters ?? new BasicParameters();
        parameters.GuaranteeDateTo = guaranteeDateFrom.AddDays(AppDefaults.MaxGuaranteeInDays);
        return parameters;
    }

    private async Task<SimulationInputs> SetUpDefaults(SimulationInputs input, CancellationToken cancellation)
    {
        input.ExpectedDateOfDrawing = input.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D

        if (!input.PaymentDay.HasValue)
        {
            input.PaymentDay = await GetDefaultPaymentDay(cancellation);
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

        input.FeeSettings = input.FeeSettings ?? new FeeSettings();

        // Určuje za jakým účelem se generuje seznam poplatků.
        // Default: 0 - za účelem nabídky
        input.FeeSettings.FeeTariffPurpose = 0;

        // Nastavení typu výpisů StatementType(CIS_HU_TYP_VYPIS)
        // Default: první hodnota v číselníku(dle Order)
        if (!input.FeeSettings.StatementTypeId.HasValue)
        {
            //var statementType = (await _codebookService.StatementTypes(cancellation)).OrderBy(i => i.Order).First();
            //input.FeeSettings.StatementTypeId = statementType.Id;
            input.FeeSettings.StatementTypeId = 1;  //Default: 1 . . . první hodnota číselníku se zatím odkládá
        }

        return input;
    }

    private static ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse ResolveRunSimulationHT(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
           ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
           _ => throw new NotImplementedException("RunSimulationHT")
       };
}
