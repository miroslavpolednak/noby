﻿using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
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

        // KBModel
        var kbmodelRequest = new ExternalServices.LuxpiService.V1.Contracts.KBModelRequest
        {
            TechnicalState = request.BuildingTechnicalStateCode,
            MaterialStructure = request.BuildingMaterialStructureCode,
            FlatSchema = request.FlatSchemaCode,
            FlatArea = request.FlatArea,
            DealNumber = request.ContractNumber,
            Leased = houseAndFlat?.FinishedHouseAndFlatDetails?.Leased,
            AgeOfBuilding = request.BuildingAgeCode,
            ActualPurchasePrice = request.CollateralAmount,
            IsDealSubject = realEstate.IsLoanRealEstate,
            LoanAmount = request.LoanAmount
        };
        var kbmodelReponse = await _luxpiServiceClient.CreateKbmodelFlat(kbmodelRequest, 11010525, cancellationToken);

        // revaluation check
        var revaluationRequest = new ExternalServices.PreorderService.V1.Contracts.OnlineRevaluationCheckRequestDTO
        {
        };
        var revaluationResponse = await _preorderService.RevaluationCheck(revaluationRequest, cancellationToken);

        // update databaze hlavni entity
        realEstate.ValuationResultCurrentPrice = kbmodelReponse.ResultPrice;
        realEstate.PreorderId = kbmodelReponse.ValuationId;
        realEstate.ValuationTypeId = 1;
        realEstate.IsRevaluationRequired = true;
        realEstate.ValuationStateId = (int)RealEstateValuationStates.DoplneniDokumentu;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
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
