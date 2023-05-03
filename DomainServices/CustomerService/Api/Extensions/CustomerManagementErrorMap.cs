using System.ComponentModel;
using CIS.Core.Exceptions;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;
using __Contracts = DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;

namespace DomainServices.CustomerService.Api.Extensions;

[SingletonService, SelfService]
internal sealed class CustomerManagementErrorMap
{
    private readonly Dictionary<string, Error> _errors = new();

    public CustomerManagementErrorMap()
    {
        MapErrors();
    }

    public static long ResolveAndThrowIfError(__Contracts.CreateIdentifiedSubjectResponse response)
    {
        switch (response.ResponseCode)
        {
            case __Contracts.CreateIdentifiedSubjectResponseResponseCode.CREATED:
                return response.CreatedSubject.CustomerId;

            case __Contracts.CreateIdentifiedSubjectResponseResponseCode.IDENTIFIED when response.IdentifiedSubjects.Count == 1:
                {
                    // nemame jak vratit ID (nevracime Result object), takze do zpravy...
                    throw new CisValidationException(11023, response.IdentifiedSubjects.First().CustomerId.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }

            case __Contracts.CreateIdentifiedSubjectResponseResponseCode.IDENTIFIED:
                {
                    var message = $"KB CM: Duplicity already exist. List of customerIds = {string.Join(", ", response.IdentifiedSubjects.Select(x => x.CustomerId))}";
                    throw new CisValidationException(11024, message);
                }

            case __Contracts.CreateIdentifiedSubjectResponseResponseCode.NOT_FOUND_IN_BR:
                throw new CisValidationException(11025, "KB CM: Unable to identify customer in state registry ");

            case __Contracts.CreateIdentifiedSubjectResponseResponseCode.UNAVAILABLE_BR:
                throw new CisValidationException(11026, "KB CM: State registry is unavailable");

            default:
                throw new InvalidEnumArgumentException(nameof(response.ResponseCode), (int)(response.ResponseCode ?? 0), typeof(__Contracts.CreateIdentifiedSubjectResponseResponseCode));
        }
    }

    public void ResolveValidationError(IdentifiedSubjectError errorData)
    {
        foreach (var errorMessage in errorData.Detail)
        {
            if (_errors.ContainsKey(errorMessage))
                continue;

            var error = _errors[errorMessage];

            throw new CisValidationException(error.ErrorCode, $"{errorData.Message}, {errorMessage} - {error.Description}");
        }
    }

    private void MapErrors()
    {
        AddError(11018, "KB CM: Unable to create customer - Subject not identifiable", "CM_SUBJECT_NOT_IDENTIFIABLE");

        AddError(11019,
                 "KB CM: Unable to create customer - Identification document is incorrect",
                 "CM_IDDOC_DOC_NUMBER_FORMAT",
                 "CM_IDDOC_ISSUED_ON_EMPTY",
                 "CM_IDDOC_VALID_TO_EMPTY",
                 "CM_IDDOC_LOST_DOCUMENT",
                 "CM_IDDOC_ISSUED_ON_FUTURE",
                 "CM_IDDOC_ISSUED_ON_OLD");

        AddError(11020, "KB CM: Unable to create customer - Incorrect set of addresses", "CM_SUBJECT_CONTACT_ADDRESS_ONLY", "CM_SUBJECT_TEMPORARY_STAY_INVALID");

        AddError(11021,
                 "KB CM: Unable to create customer - Address parameters are incorrect",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_POST_CODE_INVALID",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_CITY_DISTRICT_EMPTY," +
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_ADDRESS_POINT_INVALID",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_BUILDING_NUMBER_EMPTY",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_PRAGUE_DISTRICT_EMPTY",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_REQUIRED_CZECH_ADDRESS",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_PO_BOX_ADDRESS_TYPE_INVALID",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_PO_BOX_FORBIDDEN_FIELDS_USED",
                 "CM_PRIMARY_ADDRESS_COMPONENT_FORMAT_PO_BOX_NONNUMERICAL",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_POST_CODE_INVALID",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_CITY_DISTRICT_EMPTY," +
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_ADDRESS_POINT_INVALID",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_BUILDING_NUMBER_EMPTY",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_PRAGUE_DISTRICT_EMPTY",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_REQUIRED_CZECH_ADDRESS",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_PO_BOX_ADDRESS_TYPE_INVALID",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_PO_BOX_FORBIDDEN_FIELDS_USED",
                 "CM_CONTACT_ADDRESS_COMPONENT_FORMAT_PO_BOX_NONNUMERICAL");

        AddError(11022,
                 "KB CM: Unable to create customer - Contact parameters are incorrect",
                 "CM_PHONE_IDC_MISSING",
                 "CM_PHONE_NO_DIGITS",
                 "CM_PHONE_MORE_THAN_MAX_LENGTH",
                 "CM_PHONE_INVALID_NATIONAL_PREFIX",
                 "CM_PHONE_LESS_THAN_MIN_LENGTH",
                 "CM_PHONE_ALL_ZEROS",
                 "CM_EMAIL_INVALID_FORMAT",
                 "CM_EMAIL_MISSING_TOP_DOMAIN",
                 "CM_EMAIL_DUMMY_EMAIL",
                 "CM_EMAIL_CONTAINS_DIACRITICS",
                 "CM_IDDOC_CUST_IDENT_MISSING_IDDOC");
    }

    private void AddError(int errorNumberCode, string description, params string[] errorCodes)
    {
        var error = new Error
        {
            ErrorCode = errorNumberCode,
            Description = description
        };

        foreach (var errorCode in errorCodes)
        {
            _errors.Add(errorCode, error);
        }
    }

    private sealed record Error
    {
        public int ErrorCode { get; init; }

        public string Description { get; init; } = null!;
    }
}