using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using NOBY.Dto.RealEstateValuation;
using NOBY.Dto.RealEstateValuation.SpecificDetails;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

internal class GetRealEstateValuationDetailHandler : IRequestHandler<GetRealEstateValuationDetailRequest, GetRealEstateValuationDetailResponse>
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

    public async Task<GetRealEstateValuationDetailResponse> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);
        var deeds = await _realEstateValuationService.GetDeedOfOwnershipDocuments(request.RealEstateValuationId, cancellationToken);

        if (valuationDetail.CaseId != request.CaseId)
            throw new CisAuthorizationException("The requested RealEstateValuation is not assigned to the requested Case");

        var state = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken)).First(x => x.Id == valuationDetail.ValuationStateId);
        var categories = await _codebookService.AcvAttachmentCategories(cancellationToken);

        return new GetRealEstateValuationDetailResponse
        {
            RealEstateValuationListItem = new RealEstateValuationListItem
            {
                RealEstateValuationId = valuationDetail.RealEstateValuationId,
                OrderId = valuationDetail.OrderId,
                CaseId = valuationDetail.CaseId,
                RealEstateTypeId = valuationDetail.RealEstateTypeId,
                RealEstateTypeIcon = __Contracts.Helpers.GetRealEstateTypeIcon(valuationDetail.RealEstateTypeId),
                ValuationStateId = valuationDetail.ValuationStateId,
                ValuationStateIndicator = (ValuationStateIndicators)state.Indicator,
                ValuationStateName = state.Name,
                IsLoanRealEstate = valuationDetail.IsLoanRealEstate,
                RealEstateStateId = valuationDetail.RealEstateStateId,
                ValuationTypeId = valuationDetail.ValuationTypeId,
                Address = valuationDetail.Address,
                ValuationSentDate = valuationDetail.ValuationSentDate,
                ValuationResultCurrentPrice = valuationDetail.ValuationResultCurrentPrice,
                ValuationResultFuturePrice = valuationDetail.ValuationResultFuturePrice,
                IsRevaluationRequired = valuationDetail.IsRevaluationRequired,
                DeveloperAllowed = valuationDetail.DeveloperAllowed,
                DeveloperApplied = valuationDetail.DeveloperApplied
            },
            RealEstateValuationDetail = new RealEstateValuationDetail
            {
                CaseInProgress = caseInstance.State == (int)CaseStates.InProgress,
                RealEstateVariant = GetRealEstateVariant(valuationDetail.RealEstateTypeId),
                RealEstateSubtypeId = valuationDetail.RealEstateSubtypeId,
                LoanPurposeDetails = valuationDetail.LoanPurposeDetails is null ? null : new LoanPurposeDetail { LoanPurposes = valuationDetail.LoanPurposeDetails.LoanPurposes.ToList() },
                SpecificDetails = GetSpecificDetailsObject(valuationDetail)
            },
            Attachments = valuationDetail.Attachments?.Select(t => new RealEstateValuationAttachment
            {
                RealEstateValuationAttachmentId = t.RealEstateValuationAttachmentId,
                Title = t.Title,
                FileName = t.FileName,
                AcvAttachmentCategoryId = t.AcvAttachmentCategoryId,
                AcvAttachmentCategoryName = categories.FirstOrDefault(x => x.Id == t.AcvAttachmentCategoryId)?.Name ?? ""
            }).ToList(),
            DeedOfOwnershipDocuments = deeds?.Select(t => new Dto.RealEstateValuation.DeedOfOwnershipDocumentWithId
            {
                DeedOfOwnershipDocumentId = t.DeedOfOwnershipDocumentId,
                DeedOfOwnershipDocument = new()
                {
                    Address = t.Address,
                    CremDeedOfOwnershipDocumentId = t.CremDeedOfOwnershipDocumentId,
                    DeedOfOwnershipId = t.DeedOfOwnershipId,
                    DeedOfOwnershipNumber = t.DeedOfOwnershipNumber,
                    KatuzId = t.KatuzId,
                    KatuzTitle = t.KatuzTitle,
                    AddressPointId = t.AddressPointId,
                    RealEstateIds = t.RealEstateIds?.Select(t => t).ToList()
                }
            }).ToList()
        };
    }

    private static string GetRealEstateVariant(int realEstateTypeId)
    {
        return RealEstateVariantHelper.GetRealEstateVariant(realEstateTypeId) switch
        {
            RealEstateVariants.HouseAndFlat => "HF",
            RealEstateVariants.OnlyFlat => "HF+F",
            RealEstateVariants.Parcel => "P",
            RealEstateVariants.Other => "O",
            _ => throw new NotImplementedException()
        };
    }

    private static object? GetSpecificDetailsObject(__Contracts.RealEstateValuationDetail valuationDetail)
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