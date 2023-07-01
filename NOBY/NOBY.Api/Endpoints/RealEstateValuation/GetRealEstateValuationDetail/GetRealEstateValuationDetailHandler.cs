using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using NOBY.Dto.RealEstateValuation;
using NOBY.Dto.RealEstateValuation.RealEstateValuationDetailDto;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

internal class GetRealEstateValuationDetailHandler : IRequestHandler<GetRealEstateValuationDetailRequest, RealEstateValuationDetail>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetRealEstateValuationDetailHandler(ICaseServiceClient caseService, IRealEstateValuationServiceClient realEstateValuationService)
    {
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }

    public async Task<RealEstateValuationDetail> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (valuationDetail.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
            throw new NobyValidationException("The requested RealEstateValuation is not assigned to the requested Case");

        return new RealEstateValuationDetail
        {
            CaseInProgress = caseInstance.State == (int)CaseStates.InProgress,
            RealEstateVariant = GetRealEstateVariant(valuationDetail.RealEstateValuationGeneralDetails.RealEstateTypeId),
            RealEstateSubtypeId = valuationDetail.RealEstateSubtypeId,
            LoanPurposeDetails = new LoanPurposeDetail { LoanPurposes = valuationDetail.LoanPurposeDetails.LoanPurposes },
            SpecificDetails = GetSpecificDetailsObject(valuationDetail)
        };
    }

    private static string GetRealEstateVariant(int realEstateTypeId)
    {
        return realEstateTypeId switch
        {
            1 or 5 or 6 => "HF",
            2 or 3 or 9 => "HF+F",
            4 or 7 => "P",
            _ => "O"
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