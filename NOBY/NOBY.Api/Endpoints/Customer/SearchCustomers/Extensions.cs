using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Contracts;
using Google.Protobuf.Collections;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal static class Extensions
{
    public static SearchCustomersRequest ToDomainServiceRequest(this CustomerSearchData request)
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
    
    public static List<CustomerInList> ToApiResponse(this RepeatedField<CustomerDetailResponse> request)
    {
        return request.Select(t => (new CustomerInList()).FillBaseData(t)).ToList();
    }

    public static CustomerInList FillBaseData(this CustomerInList customer, CustomerDetailResponse t)
    {
        var address = t.Addresses.FirstOrDefault();

        customer.FirstName = t.NaturalPerson?.FirstName;
        customer.LastName = t.NaturalPerson?.LastName;
        customer.Street = address?.Street;
        customer.City = address?.City;
        customer.Postcode = address?.Postcode;
        customer.BirthNumber = t.NaturalPerson?.BirthNumber;
        customer.DateOfBirth = t.NaturalPerson?.DateOfBirth;
        customer.Identity = t.Identities.FirstOrDefault();

        return customer;
    }
}