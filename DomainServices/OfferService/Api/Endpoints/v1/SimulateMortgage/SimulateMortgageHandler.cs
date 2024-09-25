using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using SharedTypes.GrpcTypes;
using DomainServices.OfferService.Api.Database;
using DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using ExternalServices.EasSimulationHT.V1;
using System.Globalization;
using SharedComponents.DocumentDataStorage;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;
using DomainServices.OfferService.Api.Database.DocumentDataEntities;
using SharedTypes.Extensions;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgage;

internal sealed class SimulateMortgageHandler(
    Database.DocumentDataEntities.Mappers.MortgageCreditWorthinessSimpleDataMapper _worthinessMapper,
    Database.DocumentDataEntities.Mappers.MortgageAdditionalSimulationResultsDataMapper _additionalDataMapper,
    Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper,
    IDocumentDataStorage _documentDataStorage,
    OfferServiceDbContext _dbContext,
    ILogger<SimulateMortgageHandler> _logger,
    ICodebookServiceClient _codebookService,
    IEasSimulationHTClient _easSimulationHTClient,
    ICreditWorthinessServiceClient _creditWorthinessService
    )
        : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        // setup input default values
        await setUpDefaults(request, cancellationToken);

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellationToken)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellationToken)).ToDictionary(i => i.Id);

        var mappedInputs = _offerMapper.MapToDataInputs(request.SimulationInputs);
        var mappedBasicParams = _offerMapper.MapToDataBasicParameters(request.BasicParameters);

        // get simulation outputs
        var easSimulationReq = mappedInputs.ToEasSimulationRequest(mappedBasicParams, drawingDurationsById, drawingTypeById);
        var easSimulationRes = await _easSimulationHTClient.RunSimulationHT(easSimulationReq, cancellationToken);

        // save to DB
        var entity = new Database.Entities.Offer
        {
            ResourceProcessId = Guid.Parse(request.ResourceProcessId),
            IsCreditWorthinessSimpleRequested = request.IsCreditWorthinessSimpleRequested,
            ValidTo = request.ValidTo,
            SalesArrangementId = request.SalesArrangementId,
            CaseId = request.CaseId,
            OfferType = (int)OfferTypes.Mortgage,
            Origin = (int)OfferOrigins.OfferService
        };
        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit json data simulace
        var documentEntity = new MortgageOfferData
        {
            SimulationInputs = mappedInputs,
            BasicParameters = mappedBasicParams,
            SimulationOutputs = _offerMapper.MapToDataOutputs(easSimulationRes),
        };
        await _documentDataStorage.Add(entity.OfferId, documentEntity, cancellationToken);

        // additional data
        var documentEntityAdditionalData = _additionalDataMapper.MapToData(easSimulationRes);
        await _documentDataStorage.Add(entity.OfferId, documentEntityAdditionalData, cancellationToken);

        var response = new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = request.BasicParameters,
            SimulationInputs = request.SimulationInputs,
            SimulationResults = _offerMapper.MapToSimulationResults(documentEntity.SimulationOutputs),
            AdditionalSimulationResults = _additionalDataMapper.MapFromDataToSingle(documentEntityAdditionalData)
        };

        // bonita
        if (request.IsCreditWorthinessSimpleRequested)
        {
            var worthinessResult = await calculateCreditWorthinessSimple(entity.OfferId, request, easSimulationRes, cancellationToken);
            var (_, worthinessOutputs) = _worthinessMapper.MapFromDataToSingle(worthinessResult);
            response.CreditWorthinessSimpleResults = worthinessOutputs;
        }

        _logger.EntityCreated(nameof(Database.Entities.Offer), entity.OfferId);

        return response;
    }

    private async Task setUpDefaults(SimulateMortgageRequest request, CancellationToken cancellation)
    {
        // basic params
        request.BasicParameters ??= new();
        request.BasicParameters.GuaranteeDateTo = ((DateTime)request.SimulationInputs.GuaranteeDateFrom).AddDays(AppDefaults.MaxGuaranteeInDays);
        request.BasicParameters.StatementTypeId ??= 1;   // Default: 1

        // inputs
        request.SimulationInputs.ExpectedDateOfDrawing ??= DateTime.Now.AddDays(1); //currentDate + 1D
        request.SimulationInputs.PaymentDay ??= (await _codebookService.PaymentDays(cancellation)).FirstOrDefault(i => i.IsDefault)?.PaymentDay ?? throw new CisNotFoundException(10002, $"Default 'PaymentDay' not found.");

        //Sleva z úrokové sazby(dle individuální cenotvorby)
        //Default: Pokud nepřijde na vstup, ukládáme hodnotu nula  "0"
        request.SimulationInputs.InterestRateDiscount ??= 0;

        // Typ čerpání
        // DrawingType(SB enum)
        // Default: "2" jednorázové
        request.SimulationInputs.DrawingTypeId ??= (int)DrawingTypes.Disposable;

        //Je žádáno zaměstnanecké zvýhodnění?
        //Default: False
        request.SimulationInputs.IsEmployeeBonusRequested ??= false;

        var defaultFeeSettings = new FeeSettings() { FeeTariffPurpose = 0, IsStatementCharged = true };
        request.SimulationInputs.FeeSettings = defaultFeeSettings;

        // Určuje za jakým účelem se generuje seznam poplatků.
        // Default: 0 - za účelem nabídky
        request.SimulationInputs.FeeSettings.FeeTariffPurpose ??= defaultFeeSettings.FeeTariffPurpose;

        // Rizikové životní pojištění
        if (request.SimulationInputs.RiskLifeInsurance != null)
        {
            request.SimulationInputs.RiskLifeInsurance.Frequency ??= 1;         // Default: 1
        }

        // Pojištění nemovitosti
        if (request.SimulationInputs.RealEstateInsurance != null)
        {
            request.SimulationInputs.RealEstateInsurance.Frequency ??= 12;    // Default: 12
        }
    }

    private async Task<MortgageCreditWorthinessSimpleData> calculateCreditWorthinessSimple(int offerId, SimulateMortgageRequest request, SimulationHTResponse simulationResults, CancellationToken cancellationToken)
    {
        var kbCustomerIdentity = request.Identities.GetKbIdentityOrDefault();

        CreditWorthinessSimpleCalculateResponse? result = null;

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var creditWorthinessRequest = new CreditWorthinessSimpleCalculateRequest
            {
                PrimaryCustomerId = kbCustomerIdentity?.IdentityId.ToString(CultureInfo.InvariantCulture),
                ResourceProcessId = request.ResourceProcessId,
                ChildrenCount = request.CreditWorthinessSimpleInputs.ChildrenCount ?? 0,
                TotalMonthlyIncome = request.CreditWorthinessSimpleInputs.TotalMonthlyIncome,
                ExpensesSummary = request.CreditWorthinessSimpleInputs.ExpensesSummary is null
                ? null
                : new CreditWorthinessSimpleExpensesSummary
                {
                    Rent = request.CreditWorthinessSimpleInputs.ExpensesSummary.Rent,
                    Other = request.CreditWorthinessSimpleInputs.ExpensesSummary.Other
                },
                ObligationsSummary = request.CreditWorthinessSimpleInputs.ObligationsSummary is null
                ? null
                : new CreditWorthinessSimpleObligationsSummary
                {
                    CreditCardsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.CreditCardsAmount,
                    AuthorizedOverdraftsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.AuthorizedOverdraftsTotalAmount,
                    LoansInstallmentsAmount = request.CreditWorthinessSimpleInputs.ObligationsSummary.LoansInstallmentsAmount,
                },
                Product = new CreditWorthinessProduct
                {
                    ProductTypeId = request.SimulationInputs.ProductTypeId,
                    LoanDuration = simulationResults.uverVysledky.splatnostUveru,
                    LoanInterestRate = simulationResults.uverVysledky.sazbaPoskytnuta,
                    LoanAmount = (int)simulationResults.uverVysledky.vyseUveru,
                    LoanPaymentAmount = (int)simulationResults.uverVysledky.splatkaUveru,
                    FixedRatePeriod = request.SimulationInputs.FixedRatePeriod ?? 0
                }
            };
            result = await _creditWorthinessService.SimpleCalculate(creditWorthinessRequest, cancellationToken);
        }
        catch { }
#pragma warning restore CA1031 // Do not catch general exception types

        var documentEntity = _worthinessMapper.MapToData(request.CreditWorthinessSimpleInputs, result);
        await _documentDataStorage.Add(offerId, documentEntity, cancellationToken);

        return documentEntity;
    }
}
