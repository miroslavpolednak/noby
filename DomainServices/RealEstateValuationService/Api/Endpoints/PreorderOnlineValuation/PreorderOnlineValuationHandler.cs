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
        var realEstate = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // deed of ownership
        var deedOfOwnerships = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new { t.AddressPointId, t.RealEstateIds })
            .ToListAsync(cancellationToken);
        
        // realestateids
        var realEstateIds = deedOfOwnerships
            .Where(t => !string.IsNullOrEmpty(t.RealEstateIds))
            .SelectMany(t =>
            {
                return System.Text.Json.JsonSerializer.Deserialize<long[]>(t.RealEstateIds!)!;
            })
            .ToArray();

        // id pro preorder
        long createKbmodelId = deedOfOwnerships
            .FirstOrDefault(t => t.AddressPointId.HasValue)
            ?.AddressPointId
            ?? throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AddressPointIdNotFound);

        SpecificDetailHouseAndFlatObject? houseAndFlat = null;
        if (realEstate.SpecificDetailBin is not null)
        {
            switch (Helpers.GetRealEstateType(realEstate))
            {
                case CIS.Foms.Types.Enums.RealEstateTypes.Hf:
                case CIS.Foms.Types.Enums.RealEstateTypes.Hff:
                    houseAndFlat = SpecificDetailHouseAndFlatObject.Parser.ParseFrom(realEstate.SpecificDetailBin);
                    break;
            }
        }

        // kontrola dat
        dataValidation(realEstate);

        // KBModel
        var kbmodelRequest = new ExternalServices.LuxpiService.V1.Contracts.KBModelRequest
        {
            TechnicalState = request.Data.BuildingTechnicalStateCode,
            MaterialStructure = request.Data.BuildingMaterialStructureCode,
            FlatSchema = request.Data.FlatSchemaCode,
            FlatArea = request.Data.FlatArea,
            AgeOfBuilding = request.Data.BuildingAgeCode,
            DealNumber = request.ContractNumber,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            ActualPurchasePrice = request.CollateralAmount,
            IsDealSubject = realEstate.IsLoanRealEstate,
            LoanAmount = request.LoanAmount
        };
        var kbmodelReponse = await _luxpiServiceClient.CreateKbmodelFlat(kbmodelRequest, createKbmodelId, cancellationToken);

        // revaluation check
        var revaluationRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineRevaluationCheckRequestDTO
        {
            ValuationType = "OCENENI",
            LeasibilityRequired = houseAndFlat?.FinishedHouseAndFlatDetails?.LeaseApplicable,
            RealEstateType = realEstate.ACVRealEstateTypeId,
            TotalArea = request.Data.FlatArea,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            RealEstateIds = realEstateIds
        };
        var revaluationResponse = await _preorderService.RevaluationCheck(revaluationRequest, cancellationToken);

        // update databaze hlavni entity
        realEstate.ValuationResultCurrentPrice = kbmodelReponse.ResultPrice;
        realEstate.PreorderId = kbmodelReponse.ValuationId;
        realEstate.ValuationTypeId = 1;
        realEstate.IsRevaluationRequired = true;
        realEstate.ValuationStateId = (int)RealEstateValuationStates.DoplneniDokumentu;

        // vlozeni nove order
        var order = new Database.Entities.RealEstateValuationOrder
        {
            RealEstateValuationId = realEstate.RealEstateValuationId,
            RealEstateValuationOrderType = RealEstateValuationOrderTypes.OnlinePreorder,
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(request.Data),
            DataBin = request.Data.ToByteArray()
        };
        _dbContext.RealEstateValuationOrders.Add(order);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private static void dataValidation(Database.Entities.RealEstateValuation realEstate)
    {
        if (string.IsNullOrEmpty(realEstate.ACVRealEstateTypeId))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.PreorderOnlineDataValidation, nameof(realEstate.ACVRealEstateTypeId));
        }
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly IPreorderServiceClient _preorderService;
    private readonly ILuxpiServiceClient _luxpiServiceClient;

    public PreorderOnlineValuationHandler(ILuxpiServiceClient luxpiServiceClient, IPreorderServiceClient preorderService, RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _preorderService = preorderService;
        _luxpiServiceClient = luxpiServiceClient;
    }
}
