using SharedTypes.Enums;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler
    : IRequestHandler<PreorderOnlineValuationRequest, Empty>
{
    public async Task<Empty> Handle(PreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, realEstateIds, _, caseInstance, addressPointId) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);
        if (!addressPointId.HasValue) 
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AddressPointIdNotFound);
        }
        
        var houseAndFlat = await _aggregate.GetHouseAndFlat(request.RealEstateValuationId, cancellationToken);
        // info o produktu
        var (collateralAmount, loanAmount, _, _) = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);
        _ = int.TryParse(request.Data.BuildingAgeCode, out int ageCode);
        
        // KBModel
        var kbmodelRequest = new ExternalServices.LuxpiService.V1.Contracts.KBModelRequest
        {
            TechnicalState = request.Data.BuildingTechnicalStateCode,
            MaterialStructure = request.Data.BuildingMaterialStructureCode,
            FlatSchema = request.Data.FlatSchemaCode,
            FlatArea = Convert.ToDouble((decimal)request.Data.FlatArea),
            AgeOfBuilding = ageCode,
            DealNumber = caseInstance.Data.ContractNumber,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            IsDealSubject = entity.IsLoanRealEstate    
        };
        if (collateralAmount.HasValue)
            kbmodelRequest.ActualPurchasePrice = Convert.ToDouble(collateralAmount.GetValueOrDefault(), CultureInfo.InvariantCulture);
        if (loanAmount.HasValue)
            kbmodelRequest.LoanAmount = Convert.ToDouble(loanAmount.GetValueOrDefault(), CultureInfo.InvariantCulture);
        
        var kbmodelReponse = await _luxpiServiceClient.CreateKbmodelFlat(kbmodelRequest, addressPointId.Value, cancellationToken);
        _logger.CreateKbmodelFlat(kbmodelReponse.NoPriceAvailable, kbmodelReponse.ValuationId, kbmodelReponse.ResultPrice);

        // byl diskvalifikovan z online. Ulozit informaci a vyhodit chybu.
        if (kbmodelReponse.NoPriceAvailable)
        {
            entity.IsOnlineDisqualified = true;
            await _dbContext.SaveChangesAsync(cancellationToken);

            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.LuxpiKbModelStatusFailed);
        }

        // revaluation check
        var revaluationRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineRevaluationCheckRequestDTO
        {
            ValuationType = "OCENENI",
            LeasibilityRequired = houseAndFlat?.FinishedHouseAndFlatDetails?.LeaseApplicable,
            RealEstateType = entity.ACVRealEstateTypeId,
            TotalArea = Convert.ToDouble((decimal)request.Data.FlatArea),
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            RealEstateIds = realEstateIds
        };
        bool revaluationRequired = await _preorderService.RevaluationCheck(revaluationRequest, cancellationToken);
        _logger.RevaluationFinished(revaluationRequired);

        // update databaze hlavni entity
        entity.ValuationResultCurrentPrice = kbmodelReponse.ResultPrice;
        entity.PreorderId = kbmodelReponse.ValuationId;
        entity.ValuationTypeId = (int)RealEstateValuationTypes.Online;
        entity.IsRevaluationRequired = revaluationRequired;
        entity.ValuationStateId = (int)RealEstateValuationStates.DoplneniDokumentu;

        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit data objednavky
        var orderData = _mapper.MapToData(request.Data);
        await _documentDataStorage.Add(entity.RealEstateValuationId, orderData, cancellationToken);

        return new Empty();
    }

    private readonly Database.DocumentDataEntities.Mappers.RealEstateValuationOrderDataMapper _mapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Services.OrderAggregate _aggregate;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly IPreorderServiceClient _preorderService;
    private readonly ILuxpiServiceClient _luxpiServiceClient;
    private readonly ILogger<PreorderOnlineValuationHandler> _logger;

    public PreorderOnlineValuationHandler(
        Database.DocumentDataEntities.Mappers.RealEstateValuationOrderDataMapper mapper,
        IDocumentDataStorage documentDataStorage,
        ILogger<PreorderOnlineValuationHandler> logger,
        Services.OrderAggregate aggregate,
        ILuxpiServiceClient luxpiServiceClient, 
        IPreorderServiceClient preorderService, 
        RealEstateValuationServiceDbContext dbContext)
    {
        _mapper = mapper;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
        _aggregate = aggregate;
        _dbContext = dbContext;
        _preorderService = preorderService;
        _luxpiServiceClient = luxpiServiceClient;
    }
}
