using CIS.Foms.Types;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.Customer.Search.Dto;

namespace NOBY.Api.Endpoints.Customer.Search;

internal static class Extensions
{
    public static SearchCustomersRequest ToDomainServiceRequest(this Dto.SearchData request)
    {
        var model = new SearchCustomersRequest
        {
            NaturalPerson = new NaturalPersonSearch
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            },
            Mandant = Mandants.Kb
        };

        if (!string.IsNullOrEmpty(request.Contacts?.MobilePhone?.PhoneNumber))
        {
            model.MobilePhone = new MobilePhoneItem
            {
                PhoneIDC = request.Contacts.MobilePhone.PhoneIDC,
                PhoneNumber = request.Contacts.MobilePhone.PhoneNumber
            };
        }

        if (!string.IsNullOrEmpty(request.Contacts?.EmailAddress?.EmailAddress))
        {
            model.Email = new EmailAddressItem
            {
                EmailAddress = request.Contacts.EmailAddress.EmailAddress
            };
        }

        // ID klienta
        if (request.IdentityId.HasValue)
        {
            model.Identity = new Identity(request.IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb);
        }

        // document
        if (!string.IsNullOrEmpty(request.IdentificationDocumentNumber)
            && request.IdentificationDocumentTypeId.HasValue
            && request.IssuingCountryId.HasValue)
        {
            model.IdentificationDocument = new IdentificationDocumentSearch
            {
                IdentificationDocumentTypeId = request.IdentificationDocumentTypeId!.Value,
                IssuingCountryId = request.IssuingCountryId!.Value,
                Number = request.IdentificationDocumentNumber ?? ""
            };
        }

        return model;
    }
    
    public static List<CustomerInList> ToApiResponse(this RepeatedField<SearchCustomersItem> request)
    {
        return request.Select(t => (new CustomerInList())
            .FillBaseData(t)
            .FillIdentification(t.Identity)
            ).ToList();
    }

    internal static CustomerInList FillBaseData(this CustomerInList customer, SearchCustomersItem result)
    {
        customer.FirstName = result.NaturalPerson?.FirstName;
        customer.LastName = result.NaturalPerson?.LastName;
        customer.Street = result.Street;
        customer.City = result.City;
        customer.Postcode = result.Postcode;
        customer.BirthNumber = result.NaturalPerson?.BirthNumber;
        customer.DateOfBirth = result.NaturalPerson?.DateOfBirth;
        
        return customer;
    }

    internal static CustomerInList FillIdentification(this CustomerInList customer, Identity? identity)
    {
        if (identity is not null)
        {
            customer.Identity = new CustomerIdentity(identity.IdentityId, identity.IdentityScheme.ToString());
        }
        return customer;
    }
}