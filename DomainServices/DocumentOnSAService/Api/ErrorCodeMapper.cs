﻿using CIS.Core.ErrorCodes;

namespace DomainServices.DocumentOnSAService.Api;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int SalesArrangementNotExist = 19000;
    public const int SignatureMethodNotExist = 19002;
    public const int DocumentOnSANotExist = 19003;
    public const int UnableToSignDocumentOnSA = 19005;
    public const int EasAddFirstSignatureDateReturnedErrorState = 19006; // Without Desc
    public const int CombinationOfParametersNotSupported = 19010;
    public const int VersionHaveToBeLowerThanMaxVersion = 19011;
    public const int SetVersionDirectlyError = 19012;
    public const int SalesArrangementCategoryNotSupported = 19013;
    public const int HouseholdTypeIdNotExist = 19014;
    public const int ForSpecifiedDocumentTypeIdCannotFindHousehold = 19015;
    public const int DocumentTypeIdNotExist = 19016;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { SalesArrangementNotExist, "SalesArrangement {PropertyValue} does not exist" },
            { SignatureMethodNotExist, "SignatureMethod {PropertyValue} does not exist." },
            { DocumentOnSANotExist, "DocumentOnSA {PropertyValue} does not exist."},
            { UnableToSignDocumentOnSA, "Unable to sign DocumentOnSA {PropertyValue}. Document is for electronic signature only or is already signed."},
            { CombinationOfParametersNotSupported, "Combination of parameters not supported"},
            { VersionHaveToBeLowerThanMaxVersion, "Version have to be lower than {PropertyValue}" },
            { SetVersionDirectlyError, "If you want set version directly, you have to set increaseVersion to false and version have to be {PropertyValue}" },
            { SalesArrangementCategoryNotSupported, "This kind of SalesArrangementCategories {PropertyValue} is not supported" },
            { HouseholdTypeIdNotExist, "HouseholdTypeId {PropertyValue} does not exist"},
            { ForSpecifiedDocumentTypeIdCannotFindHousehold, "For specified documentTypeId {PropertyValue} cannot find Household"},
            { DocumentTypeIdNotExist, "DocumentTypeId {PropertyValue} does not exist"}
        });

        return Messages;
    }
}