using CIS.Core.ErrorCodes;

namespace DomainServices.DocumentOnSAService.Api;

public sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int SalesArrangementNotExist = 19000;
    public const int SignatureTypeNotExist = 19002;
    public const int DocumentOnSANotExist = 19003;
    public const int UnableToSignDocumentOnSA = 19005;
    public const int EasAddFirstSignatureDateReturnedErrorState = 19006; // Without Desc
    public const int CombinationOfParametersNotSupported = 19010;
    public const int VersionHaveToBeLowerThanMaxVersion = 19011;
    public const int SetVersionDirectlyError = 19012;
    public const int SalesArrangementCategoryNotSupported = 19013;
    public const int HouseholdTypeIdNotExist = 19014;
    public const int ForSpecifiedDocumentTypeIdCannotFindHousehold = 19015;
    public const int DocumentTypeIdNotSupportedForProductRequest = 19016;
    public const int UnsupportedStatusReturnedFromESignature = 19017;
    public const int UnableToStartSigningOrSignInvalidSalesArrangementState = 19018;
    public const int UnableToStartSigningOrSignInvalidDocument = 19019;
    public const int UnableToSendDocumentPreviewForPaperSignedDocuments = 19020;

    // Non BL validation
    public const int SalesArrangementIdIsRequired = 19030;
    public const int DocumentTypeIdIsRequired = 19031;
    public const int FormIdIsRequired = 19032;
    public const int EArchivIdIsRequired = 19033;
    public const int SignatureMethodCodeIsRequired = 19034;
    public const int UnsupportedSbSignatureType = 19035;
    public const int AmendmentHasToBeOfTypeSigning = 19036;
    public const int OnlyElectronicOrPaperSignatureSupported = 19037;
    public const int CustomerOnSAIdRequired = 19038;
    public const int UnsupportedDocumentTypeIdForServiceRequest = 19039;
    public const int WorkflowRequestCaseIdRequired = 19040;
    public const int UnsupportedKindOfSigningRequest = 19041;


    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { SalesArrangementNotExist, "SalesArrangement {PropertyValue} does not exist" },
            { SignatureTypeNotExist, "SignatureType {PropertyValue} does not exist." },
            { DocumentOnSANotExist, "DocumentOnSA {PropertyValue} does not exist."},
            { UnableToSignDocumentOnSA, "Unable to sign DocumentOnSA {PropertyValue}. Document is for electronic signature only or is already signed."},
            { CombinationOfParametersNotSupported, "Combination of parameters not supported"},
            { VersionHaveToBeLowerThanMaxVersion, "Version have to be lower than {PropertyValue}" },
            { SetVersionDirectlyError, "If you want set version directly, you have to set increaseVersion to false and version have to be {PropertyValue}" },
            { SalesArrangementCategoryNotSupported, "This kind of SalesArrangementCategories {PropertyValue} is not supported" },
            { HouseholdTypeIdNotExist, "HouseholdTypeId {PropertyValue} does not exist"},
            { ForSpecifiedDocumentTypeIdCannotFindHousehold, "For specified documentTypeId {PropertyValue} cannot find Household"},
            { DocumentTypeIdNotSupportedForProductRequest, "DocumentTypeId {PropertyValue} not supported for ProductRequest"},
            { SalesArrangementIdIsRequired, "SalesArrangementId is required"},
            { DocumentTypeIdIsRequired, " DocumentTypeId is required"},
            { FormIdIsRequired, "FormId is required"},
            { EArchivIdIsRequired, "EArchivId is required"},
            { SignatureMethodCodeIsRequired, "SignatureMethodCode is required"},
            { UnableToStartSigningOrSignInvalidSalesArrangementState, "Unable to start signing or sign (SalesArrangement is not in correct state)."},
            { UnableToStartSigningOrSignInvalidDocument, "Unable to start signing or sign (DocumentOnSA is invalid or already signed)."},
            { UnsupportedStatusReturnedFromESignature, "Unsupported status returned from ESignature: {PropertyValue}"},
            { UnsupportedSbSignatureType, "Unsupported sb SignatureType TaskId: {PropertyValue}"},
            { AmendmentHasToBeOfTypeSigning, "Amendment has to be of type signing" },
            { OnlyElectronicOrPaperSignatureSupported, "Only electronic(3) or paper(1) signatures are supported" },
            { CustomerOnSAIdRequired,"CustomerOnSAId is required" },
            { UnsupportedDocumentTypeIdForServiceRequest, "Unsupported DocumentTypeId for ServiceRequest, supported DocumentTypeId (9,10,11,12)" },
            { WorkflowRequestCaseIdRequired, "For processing workflow request CaseId is required" },
            { UnsupportedKindOfSigningRequest, "Unsupported kind of signing request" }
        });

        return Messages;
    }
}
