using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.RealEstateValuationService.Clients;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

internal sealed class GetRealEstateValuationDetailHandler(
    IMediator _mediator, 
    ICaseServiceClient _caseService, 
    IRealEstateValuationServiceClient _realEstateValuationService, 
    ICodebookServiceClient _codebookService) 
    : IRequestHandler<GetRealEstateValuationDetailRequest, RealEstateValuationGetRealEstateValuationDetailResponse>
{
    public async Task<RealEstateValuationGetRealEstateValuationDetailResponse> Handle(GetRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
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

        return new RealEstateValuationGetRealEstateValuationDetailResponse
        {
            RealEstateValuationListItem = getListItem(valuationDetail, state, priceTypes),
            RealEstateValuationDetail = new()
            {
                CaseInProgress = caseInstance.State == (int)EnumCaseStates.InProgress,
                RealEstateVariant = getRealEstateVariant(valuationDetail.RealEstateTypeId),
                RealEstateSubtypeId = valuationDetail.RealEstateSubtypeId,
                LoanPurposeDetails = valuationDetail.LoanPurposeDetails is null ? null : new RealEstateValuationSharedLoanPurposeDetail 
                { 
                    LoanPurposes = [.. valuationDetail.LoanPurposeDetails.LoanPurposes] 
                },
                SpecificDetails = getSpecificDetailsObject(valuationDetail)
            },
            LocalSurveyDetails = valuationDetail.LocalSurveyDetails is null ? null : new RealEstateValuationSharedLocalSurveyData
            {
                FirstName = valuationDetail.LocalSurveyDetails?.FirstName ?? "",
                LastName = valuationDetail.LocalSurveyDetails?.LastName ?? "",
                FunctionCode  = valuationDetail.LocalSurveyDetails?.RealEstateValuationLocalSurveyFunctionCode ?? "",
                EmailAddress = new()
                {
                    EmailAddress = valuationDetail.LocalSurveyDetails?.Email ?? ""
                },
                MobilePhone = new()
                {
                    PhoneIDC = valuationDetail.LocalSurveyDetails?.PhoneIDC ?? "",
                    PhoneNumber = valuationDetail.LocalSurveyDetails?.PhoneNumber ?? ""
                }
            },
            OnlinePreorderDetails = valuationDetail.OnlinePreorderDetails is null ? null : new RealEstateValuationSharedOnlinePreorderData
            {
                BuildingMaterialStructureCode = valuationDetail.OnlinePreorderDetails.BuildingMaterialStructureCode,
                FlatArea = (decimal?)valuationDetail.OnlinePreorderDetails.FlatArea ?? 0,
                BuildingTechnicalStateCode = valuationDetail.OnlinePreorderDetails.BuildingTechnicalStateCode,
                BuildingAgeCode = valuationDetail.OnlinePreorderDetails.BuildingAgeCode,
                FlatSchemaCode = valuationDetail.OnlinePreorderDetails.FlatSchemaCode
            },
            Attachments = getAttachments(valuationDetail.Attachments, categories),
            DeedOfOwnershipDocuments = getDeedOfOwnerships(deeds),
            Documents = await getDocuments(request.CaseId, valuationDetail.Documents, cancellationToken)
        };
    }

    private async Task<List<SharedTypesDocumentsMetadata>?> getDocuments(long caseId, RepeatedField<__Contracts.RealEstateValuationDocument> realEstateValuationDocuments, CancellationToken cancellationToken)
    {
        var documentIds = realEstateValuationDocuments.SelectMany(rd => new[] { rd.DocumentInfoPrice, rd.DocumentRecommendationForClient }.Where(str => !string.IsNullOrWhiteSpace(str))).ToArray();

        if (documentIds.Length == 0)
            return default;

        var documentList = await _mediator.Send(new GetDocumentListRequest(caseId, default), cancellationToken);

        //TODO odstranit po kompletnim prevedeni na OPenAPI generovani
        //return documentList.DocumentsMetadata.Where(d => documentIds.Contains(d.DocumentId)).ToList();

        var list = documentList.DocumentsMetadata.Where(d => documentIds.Contains(d.DocumentId)).ToList();
        return list.Select(t => new SharedTypesDocumentsMetadata
        {
            UploadStatus = (SharedTypesDocumentsMetadataUploadStatus)t.UploadStatus,
            CreatedOn = t.CreatedOn,
            DocumentId = t.DocumentId,
            Description = t.Description,
            EaCodeMainId = t.EaCodeMainId,
            FileName = t.FileName,
            FormId = t.FormId
        }).ToList();
    }

    private static List<RealEstateValuationGetRealEstateValuationDetailAttachment>? getAttachments(IEnumerable<__Contracts.RealEstateValuationAttachment> attachments, List<GenericCodebookResponse.Types.GenericCodebookItem> categories)
        => attachments?
            .OrderByDescending(t => (DateTime)t.CreatedOn)
            .Select(t => new RealEstateValuationGetRealEstateValuationDetailAttachment
            {
                CreatedOn = t.CreatedOn,
                RealEstateValuationAttachmentId = t.RealEstateValuationAttachmentId,
                Title = t.Title,
                FileName = t.FileName,
                AcvAttachmentCategoryId = t.AcvAttachmentCategoryId,
                AcvAttachmentCategoryName = categories.FirstOrDefault(x => x.Id == t.AcvAttachmentCategoryId)?.Name ?? ""
            })
            .ToList();

    private static List<RealEstateValuationSharedDeedOfOwnershipDocumentWithId>? getDeedOfOwnerships(List<__Contracts.DeedOfOwnershipDocument> deeds)
        => deeds?.Select(t => new RealEstateValuationSharedDeedOfOwnershipDocumentWithId
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
                RealEstateIds = t.RealEstateIds?.Select(t => t).ToList() ?? []
            }
        })
        .ToList();

    private static RealEstateValuationSharedRealEstateValuationListItem getListItem(
        __Contracts.RealEstateValuationDetail valuationDetail, 
        WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem state,
        List<GenericCodebookResponse.Types.GenericCodebookItem> priceTypes)
    {
        var model = new RealEstateValuationSharedRealEstateValuationListItem
        {
            RealEstateValuationId = valuationDetail.RealEstateValuationId,
            OrderId = valuationDetail.OrderId,
            CaseId = valuationDetail.CaseId,
            RealEstateTypeId = valuationDetail.RealEstateTypeId,
            RealEstateTypeIcon = RealEstateValuationHelpers.GetRealEstateTypeIcon(valuationDetail.RealEstateTypeId),
            ValuationStateId = valuationDetail.ValuationStateId,
            ValuationStateIndicator = (EnumStateIndicators)state.Indicator,
            ValuationStateName = state.Name,
            IsLoanRealEstate = valuationDetail.IsLoanRealEstate,
            RealEstateStateId = valuationDetail.RealEstateStateId,
            ValuationTypeId = (EnumRealEstateValuationTypes)valuationDetail.ValuationTypeId,
            Address = valuationDetail.Address,
            ValuationSentDate = valuationDetail.ValuationSentDate,
            IsRevaluationRequired = valuationDetail.IsRevaluationRequired,
            DeveloperAllowed = valuationDetail.DeveloperAllowed,
            DeveloperApplied = valuationDetail.DeveloperApplied,
            PossibleValuationTypeId = valuationDetail.PossibleValuationTypeId
                ?.Select(t => (EnumRealEstateValuationTypes)t)
                ?.ToList(),
            Prices = valuationDetail.Prices?.Select(x => new RealEstateValuationSharedRealEstateValuationListItemPriceDetail
            {
                Price = x.Price,
                PriceTypeName = priceTypes.FirstOrDefault(xx => xx.Code == x.PriceSourceType)?.Name ?? x.PriceSourceType
            }).ToList()
        };

        return model;
    }

    private static string getRealEstateVariant(int realEstateTypeId)
    {
        return RealEstateValuationHelpers.GetRealEstateVariant(realEstateTypeId) switch
        {
            RealEstateVariants.HouseAndFlat => "HF",
            RealEstateVariants.OnlyFlat => "HF+F",
            RealEstateVariants.Parcel => "P",
            RealEstateVariants.Other => "O",
            _ => throw new NotImplementedException()
        };
    }

    private static RealEstateValuationSharedSpecificDetails? getSpecificDetailsObject(__Contracts.RealEstateValuationDetail valuationDetail)
    {
        return valuationDetail.SpecificDetailCase switch
        {
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.HouseAndFlatDetails => RealEstateValuationSharedSpecificDetails.Create(createHouseAndFlatDetails(valuationDetail.HouseAndFlatDetails)),
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.ParcelDetails => RealEstateValuationSharedSpecificDetails.Create(createParcelDetails(valuationDetail.ParcelDetails)),
            __Contracts.RealEstateValuationDetail.SpecificDetailOneofCase.None => default,
            _ => throw new NotImplementedException()
        };
    }

    private static RealEstateValuationSharedSpecificDetailsHouseAndFlat createHouseAndFlatDetails(__Contracts.SpecificDetailHouseAndFlatObject houseAndFlat)
    {
        return new RealEstateValuationSharedSpecificDetailsHouseAndFlat
        {
            PoorCondition = houseAndFlat.PoorCondition,
            OwnershipRestricted = houseAndFlat.OwnershipRestricted,
            FlatOnlyDetails = houseAndFlat.FlatOnlyDetails is null
                ? default
                : new RealEstateValuationSharedSpecificDetailsHouseAndFlatFlatOnly
                {
                    SpecialPlacement = houseAndFlat.FlatOnlyDetails.SpecialPlacement,
                    Basement = houseAndFlat.FlatOnlyDetails.Basement
                },
            FinishedHouseAndFlatDetails = houseAndFlat.FinishedHouseAndFlatDetails is null
                ? default
                : new RealEstateValuationSharedSpecificDetailsHouseAndFlatFinished
                {
                    Leased = houseAndFlat.FinishedHouseAndFlatDetails.Leased,
                    LeaseApplicable = houseAndFlat.FinishedHouseAndFlatDetails.LeaseApplicable
                }
        };
    }

    private static RealEstateValuationSharedSpecificDetailsParcel createParcelDetails(__Contracts.SpecificDetailParcelObject parcel)
    {
        return new RealEstateValuationSharedSpecificDetailsParcel
        {
            ParcelNumbers = parcel?.ParcelNumbers?.Select(t => new RealEstateValuationSharedSpecificDetailsParcelNumber
            {
                Number = t.Number,
                Prefix = t.Prefix
            }).ToList()
        };
    }
}