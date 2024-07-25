using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Contracts;
using Google.Protobuf.Collections;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal static class Extensions
{
    public static DomainServices.CustomerService.Contracts.SearchCustomersRequest ToDomainServiceRequest(this CustomerSearchData request)
    {
        var model = new DomainServices.CustomerService.Contracts.SearchCustomersRequest
        {
            NaturalPerson = new NaturalPersonSearch
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            },
            Mandant = SharedTypes.GrpcTypes.Mandants.Kb
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
            model.Identity = new Identity(request.IdentityId, SharedTypes.Enums.IdentitySchemes.Kb);
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
        customer.Street = result.Address?.Street;
        customer.City = result.Address?.City;
        customer.Postcode = result.Address?.Postcode;
        customer.BirthNumber = result.NaturalPerson?.BirthNumber;
        customer.DateOfBirth = result.NaturalPerson?.DateOfBirth;
        
        return customer;
    }

    internal static CustomerInList FillIdentification(this CustomerInList customer, Identity? identity)
    {
        if (identity is not null)
        {
            customer.Identity = identity;
        }
        return customer;
    }
}