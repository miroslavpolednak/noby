using Amazon.Runtime.Internal.Transform;
using CIS.Core.ErrorCodes;

namespace DomainServices.CustomerService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int CustomerNotFound = 11000;
    public const int PartnerNotInKonsDb = 11001;
    public const int InvalidIdentityScheme = 11004;
    public const int IdentityIdIsEmpty = 11005;
    public const int UnknownIdentityScheme = 11006;
    public const int ProfileCodeIsEmpty = 11007;
    public const int MandantIsEmpty = 11008;
    public const int SearchFieldsEmpty = 11009;
    public const int LastNameIsEmpty = 11010;
    public const int InvalidEmail = 11011;
    public const int IdentDocNumberIsEmpty = 11012;
    public const int IdentDocTypeIsEmpty = 11013;
    public const int IdentDocCountryIsEmpty = 11014;
    public const int IdentityNotExist = 11015;
    public const int MissingPartnerId = 11016;
    public const int CMError1 = 11023;
    public const int CMError2 = 11024;
    public const int CMError3 = 11025;
    public const int CMError4 = 11026;
    public const int CantSetPEP = 11027;
    public const int CantSetUS = 11028;
    public const int DateOfBirthSearchInvalid = 11029;
    public const int UnsupportedMandant = 11030;
    public const int ContactIsEmpty = 11031;
    public const int ContactTypeIsEmpty = 11032;
    public const int ContactTypeUnsupported = 11033;
    public const int AddressDataMissing = 11034;
    public const int KbIdentityMissing = 11035;
    public const int RequestMustContainBothIdentities = 11036;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CustomerNotFound, "Customers with ID: {PropertyValue} do not exist." },
            { PartnerNotInKonsDb, "Partner {PropertyValue} does not exist in KonsDB." },
            { InvalidIdentityScheme, "Invalid identity scheme." },
            { IdentityIdIsEmpty, "IdentityId must be > 0" },
            { UnknownIdentityScheme, "IdentityScheme must be specified" },
            { ProfileCodeIsEmpty, "CustomerProfileCode must be specified" },
            { MandantIsEmpty, "Mandant must be not empty" },
            { SearchFieldsEmpty, "At least one of search field is required" },
            { LastNameIsEmpty, "LastName must be not empty" },
            { InvalidEmail, "Email is not valid" },
            { IdentDocNumberIsEmpty, "IdentificationDocument.Number must be not empty" },
            { IdentDocTypeIsEmpty, "IdentificationDocumentTypeId is not valid" },
            { IdentDocCountryIsEmpty, "IssuingCountryId is not valid" },
            { IdentityNotExist, "Identity {PropertyValue} for identityScheme MP does not exist" },
            { MissingPartnerId, "Unable to create customer in KonsDb without PartnerId." },
            { CMError1, "{PropertyValue}" },
            { CMError2, "KB CM: Duplicity already exist. List of customerIds = {PropertyValue}" },
            { CMError3, "KB CM: Unable to identify customer in state registry" },
            { CMError4, "KB CM: State registry is unavailable" },
            { RequestMustContainBothIdentities, "Request must contain both KB ID and MP ID" },
            { CantSetPEP, "Cannot set isPoliticallyExposed = true in KB CM" },
            { CantSetUS, "Cannot set isUSPerson = true in KB CM" },
            { DateOfBirthSearchInvalid, "One more parameter, which can be sent to CM, is needed to search by date of birth." },
            { UnsupportedMandant, "Unsupported mandant" },
            { ContactIsEmpty, "Contact must not be empty." },
            { ContactTypeIsEmpty, "ContactType must not be empty." },
            { ContactTypeUnsupported, "ContactType has unexpected value." },
            { AddressDataMissing, "City and CountryId must not be empty" },
            { KbIdentityMissing, "Customer does not have KB Identity" }
        });

        return Messages;
    }
}
