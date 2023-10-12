﻿using CIS.Core.ErrorCodes;
using System.Security.Policy;

namespace DomainServices.DocumentOnSAService.Api;

public sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int SalesArrangementNotExist = 19000;
    public const int DocumentTypeIdDoesNotExist = 19001;
    public const int SignatureTypeNotExist = 19002;
    public const int DocumentOnSANotExist = 19003;
    public const int AlreadySignedDocumentOnSA = 19005;
    public const int EasAddFirstSignatureDateReturnedErrorState = 19006; // Without Desc
    public const int CombinationOfParametersNotSupported = 19010;
    public const int VersionHaveToBeLowerThanMaxVersion = 19011;
    public const int SetVersionDirectlyError = 19012;
    public const int SalesArrangementCategoryNotSupported = 19013;
    public const int HouseholdTypeIdNotExist = 19014;
    public const int ForSpecifiedDocumentTypeIdCannotFindHousehold = 19015;
    public const int DocumentTypeIdNotSupportedForProductRequest = 19016;
    public const int UnsupportedStatusReturnedFromESignature = 19017;
    public const int SigningInvalidSalesArrangementState = 19018;
    public const int UnableToStartSigningOrSignInvalidDocument = 19019;
    public const int UnableToSendDocumentPreviewForPaperSignedDocuments = 19020;
    public const int SignatureTypeIdHasToBeSame = 19021;
    public const int DocumentFileNotExist = 19022;
    public const int AttachmentFileNotExist = 19023;
    public const int NoDocumentsToSignForSa = 19024;
    public const int UnableGetExternalIdForDocumentOnSaId = 19025;
    public const int DocumentOnSaDoesntExistForFormId = 19026;

    // Gap 
    public const int SalesArrangementIdIsRequired = 19030;
    public const int DocumentTypeIdIsRequired = 19031;
    public const int FormIdIsRequired = 19032;
    public const int EArchivIdIsRequired = 19033;
    public const int UnsupportedSbSignatureType = 19035;
    public const int AmendmentHasToBeOfTypeSigning = 19036;
    public const int OnlyElectronicOrPaperSignatureSupported = 19037;
    public const int CustomerOnSAIdRequired = 19038;
    public const int UnsupportedDocumentTypeIdForServiceRequest = 19039;
    public const int WorkflowRequestCaseIdRequired = 19040;
    public const int UnsupportedKindOfSigningRequest = 19041;
    public const int CannotGetNobyUserIdentifier = 19042;
    public const int UnsuccessfulCustomerDataUpdateToCM = 19043;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { SalesArrangementNotExist, "SalesArrangement {PropertyValue} does not exist" },
            { DocumentTypeIdDoesNotExist, "DocumentTypeId doesn't exist." },
            { SignatureTypeNotExist, "SignatureType {PropertyValue} does not exist." },
            { DocumentOnSANotExist, "DocumentOnSA {PropertyValue} does not exist."},
            { AlreadySignedDocumentOnSA, "Unable to sign DocumentOnSA {PropertyValue}. Document has been already signed."},
            { CombinationOfParametersNotSupported, "Combination of parameters not supported"},
            { VersionHaveToBeLowerThanMaxVersion, "Version have to be lower than {PropertyValue}" },
            { SetVersionDirectlyError, "If you want set version directly, you have to set increaseVersion to false and version have to be {PropertyValue}" },
            { SalesArrangementCategoryNotSupported, "This kind of SalesArrangementCategories {PropertyValue} is not supported" },
            { HouseholdTypeIdNotExist, "HouseholdTypeId {PropertyValue} does not exist"},
            { ForSpecifiedDocumentTypeIdCannotFindHousehold, "For specified documentTypeId {PropertyValue} cannot find Household"},
            { DocumentTypeIdNotSupportedForProductRequest, "DocumentTypeId {PropertyValue} not supported for ProductRequest"},
            { SalesArrangementIdIsRequired, "SalesArrangementId is required"},
            { UnableToSendDocumentPreviewForPaperSignedDocuments, "Unable to send document preview for paper signed documents"},
            { SignatureTypeIdHasToBeSame, "SignatureTypeId has to be same (cannot sign electronic document manually and contrary)"},
            { DocumentFileNotExist,"Document file {PropertyValue} doest not exist."},
            { AttachmentFileNotExist,"Attachment file {PropertyValue} doest not exist."},
            { NoDocumentsToSignForSa,"For SalesArrangementId {PropertyValue} there isn't document to sign"},
            { UnableGetExternalIdForDocumentOnSaId, "Unable get external id for provided DocumentOnSAId {PropertyValue}"},
            { DocumentOnSaDoesntExistForFormId, "DocumentOnSa doesn't exist for specified FormId {PropertyValue}" },

            { DocumentTypeIdIsRequired, " DocumentTypeId is required"},
            { FormIdIsRequired, "FormId is required"},
            { EArchivIdIsRequired, "EArchivId is required"},
            { SigningInvalidSalesArrangementState, "Unable to set SalesArrangementState (SalesArrangement is not in correct state)."},
            { UnableToStartSigningOrSignInvalidDocument, "Unable to start signing or sign (DocumentOnSA is invalid or already signed)."},
            { UnsupportedStatusReturnedFromESignature, "Unsupported status returned from ESignature: {PropertyValue}"},
            { UnsupportedSbSignatureType, "Unsupported sb SignatureType TaskId: {PropertyValue}"},
            { AmendmentHasToBeOfTypeSigning, "Amendment has to be of type signing" },
            { OnlyElectronicOrPaperSignatureSupported, "Only electronic(3) or paper(1) signatures are supported" },
            { CustomerOnSAIdRequired,"CustomerOnSAId is required" },
            { UnsupportedDocumentTypeIdForServiceRequest, "Unsupported DocumentTypeId for ServiceRequest, supported DocumentTypeId (9,10,11,12)" },
            { WorkflowRequestCaseIdRequired, "For processing workflow request CaseId is required" },
            { UnsupportedKindOfSigningRequest, "Unsupported kind of signing request" },
            { CannotGetNobyUserIdentifier, "Cannot get NOBY user identifier"},
            { UnsuccessfulCustomerDataUpdateToCM, "Unsuccessful customer data update to CM"}
        });

        return Messages;
    }
}
