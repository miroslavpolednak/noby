using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler(
    RealEstateValuationDataMapper _mapperValuation,
    ILogger<PreorderOnlineValuationHandler> _logger,
    Services.OrderAggregate _aggregate,
    ILuxpiServiceClient _luxpiServiceClient,
    IPreorderServiceClient _preorderService,
    RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<PreorderOnlineValuationRequest, Empty>
{
    public async Task<Empty> Handle(PreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var (entity, revDetailData, realEstateIds, _, caseInstance, addressPointId) = await _aggregate.GetAggregatedData(request.RealEstateValuationId, cancellationToken);
        if (!addressPointId.HasValue) 
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AddressPointIdNotFound);
        }

        // house and flat structure
        var houseAndFlat = _mapperValuation.MapFromDataToSingle(revDetailData).HouseAndFlatDetails;
        
        // get LUXPI response
        var kbmodelReponse = await getKbModel(request, entity, caseInstance, houseAndFlat, addressPointId, cancellationToken);

        // byl diskvalifikovan z online. Ulozit informaci a vyhodit chybu.
        if (kbmodelReponse.NoPriceAvailable)
        {
            await disqualifyFromOnlineAndUpdateEntity(entity, cancellationToken);
        }

        // revaluation check
        var revaluationRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineRevaluationCheckRequestDTO
        {
            ValuationType = "OCENENI",
            LeasibilityRequired = houseAndFlat?.FinishedHouseAndFlatDetails?.LeaseApplicable,
            RealEstateType = entity.ACVRealEstateTypeId,
            TotalArea = Convert.ToDouble((decimal)request.OnlinePreorderDetails.FlatArea!),
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            RealEstateIds = realEstateIds
        };
        bool revaluationRequired = await _preorderService.RevaluationCheck(revaluationRequest, cancellationToken);
        _logger.RevaluationFinished(revaluationRequired);

        // update databaze hlavni entity
        await updateEntity(entity, kbmodelReponse, revaluationRequired, cancellationToken);

        // ulozit data objednavky
        await _aggregate.UpdateOnlinePreorderDetailsOnly(request.RealEstateValuationId, request.OnlinePreorderDetails, revDetailData, cancellationToken);

        return new Empty();
    }

    private async Task<ExternalServices.LuxpiService.Dto.CreateKbmodelFlatResponse> getKbModel(
        PreorderOnlineValuationRequest request, 
        Database.Entities.RealEstateValuation entity,
        CaseService.Contracts.Case caseInstance, 
        SpecificDetailHouseAndFlatObject? houseAndFlat,
        long? addressPointId,
        CancellationToken cancellationToken)
    {
        // info o produktu
        var (collateralAmount, loanAmount, _, _) = await _aggregate.GetProductProperties(caseInstance.State, caseInstance.CaseId, cancellationToken);

        _ = int.TryParse(request.OnlinePreorderDetails.BuildingAgeCode, out int ageCode);

        // KBModel
        var kbmodelRequest = new ExternalServices.LuxpiService.V1.Contracts.KBModelRequest
        {
            TechnicalState = request.OnlinePreorderDetails.BuildingTechnicalStateCode,
            MaterialStructure = request.OnlinePreorderDetails.BuildingMaterialStructureCode,
            FlatSchema = request.OnlinePreorderDetails.FlatSchemaCode,
            FlatArea = Convert.ToDouble((decimal)request.OnlinePreorderDetails.FlatArea!),
            AgeOfBuilding = ageCode,
            DealNumber = caseInstance.Data.ContractNumber,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            IsDealSubject = entity.IsLoanRealEstate
        };
        if (collateralAmount.HasValue)
        {
            kbmodelRequest.ActualPurchasePrice = Convert.ToDouble(collateralAmount.GetValueOrDefault(), CultureInfo.InvariantCulture);
        }

        if (loanAmount.HasValue)
        {
            kbmodelRequest.LoanAmount = Convert.ToDouble(loanAmount.GetValueOrDefault(), CultureInfo.InvariantCulture);
        }

        var kbmodelReponse = await _luxpiServiceClient.CreateKbmodelFlat(kbmodelRequest, addressPointId.GetValueOrDefault(), cancellationToken);
        _logger.CreateKbmodelFlat(kbmodelReponse.NoPriceAvailable, kbmodelReponse.ValuationId, kbmodelReponse.ResultPrice);

        return kbmodelReponse;
    }

    private async Task updateEntity(
        Database.Entities.RealEstateValuation entity, 
        ExternalServices.LuxpiService.Dto.CreateKbmodelFlatResponse kbmodelReponse, 
        bool revaluationRequired, 
        CancellationToken cancellationToken)
    {
        entity.ValuationStateId = (int)RealEstateValuationStates.DoplneniDokumentu;
        entity.ValuationTypeId = (int)EnumRealEstateValuationTypes.Online;
        entity.PreorderId = kbmodelReponse.ValuationId;
        entity.IsRevaluationRequired = revaluationRequired;
        entity.Prices = new(1)
        {
            new() { Price = kbmodelReponse.ResultPrice, PriceSourceType = "Online" }
        };

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task disqualifyFromOnlineAndUpdateEntity(
        Database.Entities.RealEstateValuation entity, 
        CancellationToken cancellationToken)
    {
        entity.IsOnlineDisqualified = true;
        entity.ValuationTypeId = 0;

        // adjust PossibleValuationTypeId
        var possibleTypes = entity.PossibleValuationTypeId?.ToArray() ?? Array.Empty<int>();
        if (possibleTypes.Contains(1) && possibleTypes.Length == 1)
        {
            entity.PossibleValuationTypeId = null;
        }
        else if (possibleTypes.Contains(1))
        {
            entity.PossibleValuationTypeId = possibleTypes.Where(t => t != 1).ToList();
            if (entity.PossibleValuationTypeId.Count == 1)
            {
                entity.ValuationTypeId = entity.PossibleValuationTypeId[0];
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.LuxpiKbModelStatusFailed);
    }
}
