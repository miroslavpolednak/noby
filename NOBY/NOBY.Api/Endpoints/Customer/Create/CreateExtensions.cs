using CIS.Infrastructure.gRPC.CisTypes;
using _HO = DomainServices.HouseholdService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.Create;

internal static class CreateExtensions
{
    public static _Cust.CreateCustomerRequest ToDomainService(this CreateRequest request, Mandants createIn, params Identity[] identities)
    {
        var model = new _Cust.CreateCustomerRequest
        {
            Mandant = Mandants.Kb,
            NaturalPerson = new()
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.BirthDate,
                PlaceOfBirth = request.BirthPlace ?? "",
                GenderId = request.GenderId
            },
            IdentificationDocument = request.IdentificationDocument?.ToDomainService(),
            HardCreate = request.HardCreate
        };
        model.Identities.Add(identities);

        // adresa
        if (request.PrimaryAddress is not null)
        {
            request.PrimaryAddress!.AddressTypeId = (int)CIS.Foms.Enums.AddressTypes.Permanent;
            model.Addresses.Add(request.PrimaryAddress);
        }
        // narodnost
        if (request.CitizenshipCountryId.GetValueOrDefault() > 0)
            model.NaturalPerson.CitizenshipCountriesId.Add(request.CitizenshipCountryId!.Value);

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
        model.Customer.CustomerIdentifiers.Add(customerKb.Identities.First(x => x.IdentityScheme == Identity.Types.IdentitySchemes.Kb));

        return model;
    }

    public static CreateResponse ToResponseDto(this _Cust.CustomerDetailResponse customer)
    {
        GetDetail.Dto.NaturalPersonModel person = new();
        customer.NaturalPerson?.FillResponseDto(person);
        person.IsBrSubscribed = customer.NaturalPerson?.IsBrSubscribed;

        var model = new CreateResponse
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
            Addresses = customer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList(),
            IsInputDataDifferent = true,
            Contacts = new()
        };

        var email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Email?.EmailAddress;
        if (!string.IsNullOrEmpty(email))
            model.Contacts.EmailAddress = new() { EmailAddress = email };

        var phone = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil)?.Mobile?.PhoneNumber;
        if (!string.IsNullOrEmpty(phone))
            model.Contacts.MobilePhone = new()
            {
                PhoneNumber = phone,
                PhoneIDC = customer.Contacts!.First(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil).Mobile.PhoneIDC
            };

        return model;
    }

    public static CreateResponse InputDataComparison(this CreateResponse response, CreateRequest originalRequest)
    {
        if (
            !stringCompare(originalRequest.Contacts?.MobilePhone?.PhoneNumber, response.Contacts?.MobilePhone?.PhoneNumber)
            || !stringCompare(originalRequest.Contacts?.EmailAddress?.EmailAddress, response.Contacts?.EmailAddress?.EmailAddress)
            || originalRequest.BirthDate != response.NaturalPerson?.DateOfBirth
            || !stringCompare(originalRequest.BirthNumber, response.NaturalPerson?.BirthNumber)
            || !stringCompare(originalRequest.BirthPlace, response.NaturalPerson?.PlaceOfBirth)
            || originalRequest.CitizenshipCountryId != (response.NaturalPerson?.CitizenshipCountriesId?.FirstOrDefault() ?? 0)
            || originalRequest.GenderId != ((int?)response.NaturalPerson?.Gender ?? 0)
            || !stringCompare(originalRequest.FirstName, response.NaturalPerson?.FirstName)
            || !stringCompare(originalRequest.LastName, response.NaturalPerson?.LastName)
            || !(originalRequest.PrimaryAddress?.Equals(response.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent)) ?? true)
        )
            response.IsInputDataDifferent = true;

        return response;
    }

    private static bool stringCompare(string? s1, string? s2)
        => (s1 ?? "").Equals(s2, StringComparison.OrdinalIgnoreCase);
}
