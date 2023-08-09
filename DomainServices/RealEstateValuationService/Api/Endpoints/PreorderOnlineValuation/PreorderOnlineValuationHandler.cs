using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

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
        var houseAndFlat = Services.OrderAggregate.GetHouseAndFlat(entity);

        // KBModel
        var kbmodelRequest = new ExternalServices.LuxpiService.V1.Contracts.KBModelRequest
        {
            TechnicalState = request.Data.BuildingTechnicalStateCode,
            MaterialStructure = request.Data.BuildingMaterialStructureCode,
            FlatSchema = request.Data.FlatSchemaCode,
            FlatArea = Convert.ToDouble((decimal)request.Data.FlatArea),
            AgeOfBuilding = request.Data.BuildingAgeCode,
            DealNumber = request.ContractNumber,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            ActualPurchasePrice = request.CollateralAmount,
            IsDealSubject = entity.IsLoanRealEstate,
            LoanAmount = request.LoanAmount
        };
        var kbmodelReponse = await _luxpiServiceClient.CreateKbmodelFlat(kbmodelRequest, addressPointId.Value, cancellationToken);

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
        if (!(await _preorderService.RevaluationCheck(revaluationRequest, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.RevaluationFailed);
        }

        // update databaze hlavni entity
        entity.ValuationResultCurrentPrice = kbmodelReponse.ResultPrice;
        entity.PreorderId = kbmodelReponse.ValuationId;
        entity.ValuationTypeId = (int)RealEstateValuationTypes.Online;
        entity.IsRevaluationRequired = true;
        entity.ValuationStateId = (int)RealEstateValuationStates.DoplneniDokumentu;

        // vlozeni nove order
        var order = new Database.Entities.RealEstateValuationOrder
        {
            RealEstateValuationId = entity.RealEstateValuationId,
            RealEstateValuationOrderType = RealEstateValuationOrderTypes.OnlinePreorder,
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(request.Data),
            DataBin = request.Data.ToByteArray()
        };
        _dbContext.RealEstateValuationOrders.Add(order);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private readonly Services.OrderAggregate _aggregate;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly IPreorderServiceClient _preorderService;
    private readonly ILuxpiServiceClient _luxpiServiceClient;

    public PreorderOnlineValuationHandler(
        Services.OrderAggregate aggregate,
        ILuxpiServiceClient luxpiServiceClient, 
        IPreorderServiceClient preorderService, 
        RealEstateValuationServiceDbContext dbContext)
    {
        _aggregate = aggregate;
        _dbContext = dbContext;
        _preorderService = preorderService;
        _luxpiServiceClient = luxpiServiceClient;
    }
}
