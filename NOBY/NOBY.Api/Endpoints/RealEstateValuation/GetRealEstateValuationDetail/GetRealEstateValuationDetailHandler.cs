using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.RealEstateValuationService.Clients;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Dto.Documents;
using NOBY.Dto.RealEstateValuation;
using NOBY.Dto.RealEstateValuation.SpecificDetails;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

internal sealed class GetRealEstateValuationDetailHandler : IRequestHandler<GetRealEstateValuationDetailRequest, GetRealEstateValuationDetailResponse>
{
    private readonly IMediator _mediator;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;
    private readonly ICodebookServiceClient _codebookService;

    public GetRealEstateValuationDetailHandler(IMediator mediator, ICaseServiceClient caseService, IRealEstateValuationServiceClient realEstateValuationService, ICodebookServiceClient codebookService)
    {
        _mediator = mediator;
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
        {
            throw new NobyValidationException(90032);
        }

        var state = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken)).First(x => x.Id == valuationDetail.ValuationStateId);
        var categories = await _codebookService.AcvAttachmentCategories(cancellationToken);
        var priceTypes = await _codebookService.RealEstatePriceTypes(cancellationToken);

        return new GetRealEstateValuationDetailResponse
        {
            RealEstateValuationListItem = getListItem(valuationDetail, state, priceTypes),
            RealEstateValuationDetail = new RealEstateValuationDetail
            {
                CaseInProgress = caseInstance.State == (int)CaseStates.InProgress,
                RealEstateVariant = GetRealEstateVariant(valuationDetail.RealEstateTypeId),
                RealEstateSubtypeId = valuationDetail.RealEstateSubtypeId,
                LoanPurposeDetails = valuationDetail.LoanPurposeDetails is null ? null : new LoanPurposeDetail { LoanPurposes = valuationDetail.LoanPurposeDetails.LoanPurposes.ToList() },
                SpecificDetails = GetSpecificDetailsObject(valuationDetail)
            },
            LocalSurveyDetails = valuationDetail.LocalSurveyDetails is null ? null : new LocalSurveyData
            {
                FirstName = valuationDetail.LocalSurveyDetails?.FirstName ?? "",
                LastName = valuationDetail.LocalSurveyDetails?.LastName ?? "",
                FunctionCode  = valuationDetail.LocalSurveyDetails?.RealEstateValuationLocalSurveyFunctionCode ?? "",
                EmailAddress = new Dto.EmailAddressDto
                {
                    EmailAddress = valuationDetail.LocalSurveyDetails?.Email ?? ""
                },
                MobilePhone = new Dto.PhoneNumberDto
                {
                    PhoneIDC = valuationDetail.LocalSurveyDetails?.PhoneIDC ?? "",
                    PhoneNumber = valuationDetail.LocalSurveyDetails?.PhoneNumber ?? ""
                }
            },
            Attachments = getAttachments(valuationDetail.Attachments, categories),
            DeedOfOwnershipDocuments = getDeedOfOwnerships(deeds),
            Documents = await GetDocuments(request.CaseId, valuationDetail.Documents, cancellationToken)
        };
    }

    private async Task<List<DocumentsMetadata>?> GetDocuments(long caseId, RepeatedField<__Contracts.RealEstateValuationDocument> realEstateValuationDocuments, CancellationToken cancellationToken)
    {
        var documentIds = realEstateValuationDocuments.SelectMany(rd => new[] { rd.DocumentInfoPrice, rd.DocumentRecommendationForClient }.Where(str => !string.IsNullOrWhiteSpace(str))).ToArray();

        if (documentIds.Length == 0)
            return default;

        var documentList = await _mediator.Send(new GetDocumentListRequest(caseId, default), cancellationToken);

        return documentList.DocumentsMetadata.Where(d => documentIds.Contains(d.DocumentId)).ToList();
    }

    private static List<RealEstateValuationAttachment>? getAttachments(IEnumerable<__Contracts.RealEstateValuationAttachment> attachments, List<GenericCodebookResponse.Types.GenericCodebookItem> categories)
        => attachments?
            .OrderByDescending(t => (DateTime)t.CreatedOn)
            .Select(t => new RealEstateValuationAttachment
            {
                CreatedOn = t.CreatedOn,
                RealEstateValuationAttachmentId = t.RealEstateValuationAttachmentId,
                Title = t.Title,
                FileName = t.FileName,
                AcvAttachmentCategoryId = t.AcvAttachmentCategoryId,
                AcvAttachmentCategoryName = categories.FirstOrDefault(x => x.Id == t.AcvAttachmentCategoryId)?.Name ?? ""
            })
            .ToList();

    private static List<DeedOfOwnershipDocumentWithId>? getDeedOfOwnerships(List<__Contracts.DeedOfOwnershipDocument> deeds)
        => deeds?.Select(t => new DeedOfOwnershipDocumentWithId
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
        })
        .ToList();

    private static RealEstateValuationListItem getListItem(
        __Contracts.RealEstateValuationDetail valuationDetail, 
        WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem state,
        List<GenericCodebookResponse.Types.GenericCodebookItem> priceTypes)
    {
        var model = new RealEstateValuationListItem
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
            IsRevaluationRequired = valuationDetail.IsRevaluationRequired,
            DeveloperAllowed = valuationDetail.DeveloperAllowed,
            DeveloperApplied = valuationDetail.DeveloperApplied,
            PossibleValuationTypeId = valuationDetail.PossibleValuationTypeId
                ?.Select(t => (RealEstateValuationValuationTypes)t)
                ?.ToList(),
            Prices = valuationDetail.Prices?.Select(x => new RealEstatePriceDetail
            {
                Price = x.Price,
                PriceTypeName = priceTypes.FirstOrDefault(xx => xx.Code == x.PriceSourceType)?.Name ?? x.PriceSourceType
            }).ToList()
        };

        // to be removed
        if (valuationDetail.Prices?.Any(x => x.PriceSourceType == "STANDARD_PRICE_EXIST") ?? false)
        {
            model.ValuationResultCurrentPrice = valuationDetail.Prices.First(x => x.PriceSourceType == "STANDARD_PRICE_EXIST").Price;
        }
        if (valuationDetail.Prices?.Any(x => x.PriceSourceType == "STANDARD_PRICE_FUTURE") ?? false)
        {
            model.ValuationResultFuturePrice = valuationDetail.Prices.First(x => x.PriceSourceType == "STANDARD_PRICE_FUTURE").Price;
        }

        return model;
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
            ParcelNumbers = parcel?.ParcelNumbers?.Select(t => new ParcelNumber
            {
                Number = t.Number,
                Prefix = t.Prefix
            }).ToList()
        };
    }
}