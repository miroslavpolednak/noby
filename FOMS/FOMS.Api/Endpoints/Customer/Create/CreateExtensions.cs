using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.GetDetail;

namespace FOMS.Api.Endpoints.Customer.Create;

internal static class CreateExtensions
{
    public static _Cust.CreateCustomerRequest ToDomainService(this CreateRequest request, CIS.Infrastructure.gRPC.CisTypes.Identity identity)
    {
        var model = new _Cust.CreateCustomerRequest
        {
            NaturalPerson = new()
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.BirthDate,
                PlaceOfBirth = request.BirthPlace ?? "",
                GenderId = request.GenderId
            },
            IdentificationDocument = request.IdentificationDocument is null ? null : new()
            {
                IdentificationDocumentTypeId = request.IdentificationDocument.IdentificationDocumentTypeId,
                IssuedBy = request.IdentificationDocument.IssuedBy ?? "",
                Number = request.IdentificationDocument.Number ?? "",
                IssuingCountryId = request.IdentificationDocument.IssuingCountryId
            },
            Identity = identity,
            HardCreate = request.HardCreate
        };

        // adresa
        if (request.PrimaryAddress is not null)
        {
            request.PrimaryAddress!.AddressTypeId = (int)CIS.Foms.Enums.AddressTypes.Permanent;
            model.Addresses.Add(request.PrimaryAddress);
        }
        // narodnost
        if (request.CitizenshipCountryId > 0)
            model.NaturalPerson.CitizenshipCountriesId.Add(request.CitizenshipCountryId);

        return model;
    }

    public static _HO.UpdateCustomerRequest ToUpdateRequest(this _HO.CustomerOnSA customerOnSA, _Cust.CustomerDetailResponse customerKb)
    {
        var model = new _HO.UpdateCustomerRequest
        {
            CustomerOnSAId = customerOnSA.CustomerOnSAId,
            Customer = new _HO.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = customerKb.NaturalPerson.DateOfBirth,
                MaritalStatusId = customerKb.NaturalPerson?.MaritalStatusStateId,
                FirstNameNaturalPerson = customerKb.NaturalPerson?.FirstName ?? "",
                Name = customerKb.NaturalPerson?.LastName ?? "",
                LockedIncomeDateTime = customerOnSA.LockedIncomeDateTime
            }
        };
        if (customerOnSA.CustomerIdentifiers is not null)
            model.Customer.CustomerIdentifiers.AddRange(customerOnSA.CustomerIdentifiers);
        model.Customer.CustomerIdentifiers.Add(customerKb.Identity);

        return model;
    }

    public static _Cust.CreateCustomerRequest ToDomainService(this _Cust.CustomerDetailResponse customer)
    {
        var model = new _Cust.CreateCustomerRequest
        {
            NaturalPerson = new()
            {
                FirstName = customer.NaturalPerson?.FirstName ?? "",
                LastName = customer.NaturalPerson?.LastName ?? "",
                BirthNumber = customer.NaturalPerson?.BirthNumber ?? "",
                DateOfBirth = customer.NaturalPerson?.DateOfBirth,
                PlaceOfBirth = customer.NaturalPerson?.PlaceOfBirth ?? "",
                GenderId = customer.NaturalPerson?.GenderId ?? 0,
                IsBrSubscribed = customer.NaturalPerson?.IsBrSubscribed ?? false,
                MaritalStatusStateId = customer.NaturalPerson?.MaritalStatusStateId ?? 0,
                BirthCountryId = customer.NaturalPerson?.BirthCountryId,
                BirthName = customer.NaturalPerson?.BirthName ?? "",
                DegreeBeforeId = customer.NaturalPerson?.DegreeBeforeId ?? 0,
                DegreeAfterId = customer.NaturalPerson?.DegreeAfterId ?? 0,
                EducationLevelId = customer.NaturalPerson?.EducationLevelId ?? 0,
                KbRelationshipCode = customer.NaturalPerson?.KbRelationshipCode,
                TaxResidencyCountryId = customer.NaturalPerson?.TaxResidencyCountryId,
                IsLegallyIncapable = customer.NaturalPerson?.IsLegallyIncapable,
                IsPoliticallyExposed = customer.NaturalPerson?.IsPoliticallyExposed,
                LegallyIncapableUntil = customer.NaturalPerson?.LegallyIncapableUntil
            },
            IdentificationDocument = customer.IdentificationDocument,
            Identity = new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb
            }
        };
        if (customer.NaturalPerson?.CitizenshipCountriesId is not null)
            model.NaturalPerson!.CitizenshipCountriesId.AddRange(customer.NaturalPerson!.CitizenshipCountriesId);
        if (customer.Contacts is not null)
            model.Contacts.AddRange(customer.Contacts);
        if (customer.Addresses is not null)
            model.Addresses.AddRange(customer.Addresses);

        return model;
    }

    public static CreateResponse ToResponseDto(this _Cust.CustomerDetailResponse customer)
        => new CreateResponse
        {
            NaturalPerson = customer.NaturalPerson?.ToResponseDto(),
            JuridicalPerson = null,
            IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
            Contacts = customer.Contacts?.ToResponseDto(),
            Addresses = customer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList(),
            InputDataDifferent = true
        };

    public static CreateResponse SetResponseCode(this CreateResponse response, bool createOk)
    {
        response.ResponseCode = createOk ? "KBCM_CREATED" : "KBCM_IDENTIFIED";
        return response;
    }
    
    public static CreateResponse InputDataComparison(this CreateResponse response, CreateRequest originalRequest)
    {
        if (
            !stringCompare(originalRequest.Mobile, response.Contacts?.FirstOrDefault(t => t.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.MobilPrivate)?.Value)
            || !stringCompare(originalRequest.Email, response.Contacts?.FirstOrDefault(t => t.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Value)
            || originalRequest.BirthDate != originalRequest.BirthDate
            || !stringCompare(originalRequest.BirthNumber, response.NaturalPerson?.BirthNumber)
            || !stringCompare(originalRequest.BirthPlace, response.NaturalPerson?.PlaceOfBirth)
            || originalRequest.CitizenshipCountryId != (response.NaturalPerson?.CitizenshipCountriesId?.FirstOrDefault() ?? 0)
            || originalRequest.GenderId != ((int?)response.NaturalPerson?.Gender ?? 0)
            || !stringCompare(originalRequest.FirstName, response.NaturalPerson?.FirstName)
            || !stringCompare(originalRequest.LastName, response.NaturalPerson?.LastName)
            || !(originalRequest.PrimaryAddress?.Equals(response.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent)) ?? true)
        )
            response.InputDataDifferent = true;

        return response;
    }

    private static bool stringCompare(string? s1, string? s2)
        => (s1 ?? "").Equals(s2, StringComparison.OrdinalIgnoreCase);
}
