using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using NOBY.Api.Endpoints.RealEstateValuation.Shared;
using NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

internal class GetRealEstateValuationDetailHandler : IRequestHandler<GetRealEstateValuationDetailRequest, RealEstateValuationDetail>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;
    private readonly ICodebookServiceClient _codebookService;

    public GetRealEstateValuationDetailHandler(ICaseServiceClient caseService, IRealEstateValuationServiceClient realEstateValuationService, ICodebookServiceClient codebookService)
    {
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
        _codebookService = codebookService;
    }

    public async Task<RealEstateValuationDetail> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (valuationDetail.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
            throw new CisAuthorizationException("The requested RealEstateValuation is not assigned to the requested Case");

        var states = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);

        var detail = valuationDetail.RealEstateValuationGeneralDetails.MapToApiResponse<RealEstateValuationDetail>(states);

        detail.CaseInProgress = caseInstance.State == (int)CaseStates.InProgress;
        detail.RealEstateVariant = GetRealEstateVariant(valuationDetail.RealEstateValuationGeneralDetails.RealEstateTypeId);
        detail.RealEstateSubtypeId = valuationDetail.RealEstateSubtypeId;
        detail.LoanPurposeDetails = valuationDetail.LoanPurposeDetails is null ? null : new LoanPurposeDetail { LoanPurposes = valuationDetail.LoanPurposeDetails.LoanPurposes };
        detail.SpecificDetails = GetSpecificDetailsObject(valuationDetail);

        return detail;
    }

    private static string GetRealEstateVariant(int realEstateTypeId)
    {
        return RealEstateVariantHelper.GetRealEstateVariant(realEstateTypeId) switch
        {
            RealEstateVariant.HouseAndFlat => "HF",
            RealEstateVariant.OnlyFlat => "HF+F",
            RealEstateVariant.Parcel => "P",
            RealEstateVariant.Other => "O",
            _ => throw new NotImplementedException()
        };
    }

    private static ISpecificDetails? GetSpecificDetailsObject(__Contracts.RealEstateValuationDetail valuationDetail)
    {
        return valuationDetail.SpecificDetailCase switch
        {
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.HouseAndFlatDetails => CreateHouseAndFlatDetails(valuationDetail.HouseAndFlatDetails),
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.ParcelDetails => CreateParcelDetails(valuationDetail.ParcelDetails),
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.None => default,
            _ => throw new NotImplementedException()
        };
    }

    private static HouseAndFlatDetails CreateHouseAndFlatDetails(__Contracts.SpecificDetailHouseAndFlatObject houseAndFlat)
    {
        return new HouseAndFlatDetails
        {
            PoorCondition = houseAndFlat.PoorCondition,
            OwnershipRestricted = houseAndFlat.OwnershipRestricted,
            FlatOnlyDetails = houseAndFlat.FlatOnlyDetails is null
                ? default
                : new HouseAndFlatDetails.FlatOnlyDetailsDto
                {
                    SpecialPlacement = houseAndFlat.FlatOnlyDetails.SpecialPlacement,
                    Basement = houseAndFlat.FlatOnlyDetails.Basement
                },
            FinishedHouseAndFlatDetails = houseAndFlat.FinishedHouseAndFlatDetails is null
                ? default
                : new HouseAndFlatDetails.FinishedHouseAndFlatDetailsDto
                {
                    Leased = houseAndFlat.FinishedHouseAndFlatDetails.Leased,
                    LeaseApplicable = houseAndFlat.FinishedHouseAndFlatDetails.LeaseApplicable
                }
        };
    }

    private static ParcelDetails CreateParcelDetails(__Contracts.SpecificDetailParcelObject parcel)
    {
        return new ParcelDetails
        {
            ParcelNumber = parcel.ParcelNumber
        };
    }
}