using _SA = DomainServices.SalesArrangementService.Contracts;
using _Cust = DomainServices.CustomerService.Contracts;
using Azure.Core;
using FOMS.Api.Endpoints.Customer.GetDetail;

namespace FOMS.Api.Endpoints.Customer.Create;

internal static class CreateExtensions
{
    public static _Cust.CreateCustomerRequest ToDomainService(this CreateRequest request)
    {
        var model = new _Cust.CreateCustomerRequest
        {
            NaturalPerson = new()
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.CzechBirthNumber ?? "",
                DateOfBirth = request.BirthDate,
                PlaceOfBirth = request.BirthPlace ?? "",
                GenderId = request.GenderCode
            },
            IdentificationDocument = request.IdentificationDocument is null ? null : new()
            {
                IdentificationDocumentTypeId = request.IdentificationDocument.typeCode,
                IssuedBy = request.IdentificationDocument.issuedBy ?? "",
                Number = request.IdentificationDocument.documentNumber ?? "",
                IssuingCountryId = request.IdentificationDocument.issuingCountryCode
            },
            Identity = new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityScheme = CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb
            }
        };

        // adresa
        if (request.PrimaryAddress is not null)
        {
            request.PrimaryAddress!.AddressTypeId = (int)CIS.Foms.Enums.AddressTypes.PERMANENT;
            model.Addresses.Add(request.PrimaryAddress);
        }
        // narodnost
        if (request.CitizenshipCodes > 0)
            model.NaturalPerson.CitizenshipCountriesId.Add(request.CitizenshipCodes);

        return model;
    }

    public static _SA.UpdateCustomerRequest ToUpdateRequest(this _SA.CustomerOnSA customerOnSA, _Cust.CustomerDetailResponse customerKb)
    {
        var model = new _SA.UpdateCustomerRequest
        {
            CustomerOnSAId = customerOnSA.CustomerOnSAId,
            Customer = new _SA.CustomerOnSABase
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

        return model;
    }

    public static CreateResponse ToResponseDto(this _Cust.CustomerDetailResponse customer)
        => new CreateResponse
        {
            NaturalPerson = customer.NaturalPerson?.ToResponseDto(),
            JuridicalPerson = null,
            IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
            Contacts = customer.Contacts?.ToResponseDto(),
            Addresses = customer.Addresses?.ToResponseDto()
        };
}
