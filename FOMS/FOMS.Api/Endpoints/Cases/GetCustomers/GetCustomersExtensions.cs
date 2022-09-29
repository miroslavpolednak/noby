using CIS.Core;
using CIS.Infrastructure.gRPC.CisTypes;
using _CB = DomainServices.CodebookService.Contracts.Endpoints.Countries;
using System.ComponentModel.DataAnnotations;
using _Cust = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Cases.GetCustomers;

internal static class GetCustomersExtensions
{
    public static GetCustomersResponseCustomer ToApiResponse(
        this _Cust.CustomerDetailResponse customer, 
        List<(Identity Identity, int Role, bool Agent)> customerIdentities, 
        List<_CB.CountriesItem> countries)
    {
        var customerDetail = customerIdentities.First(x => x.Identity.IdentityId == customer.Identity.IdentityId);
        var permanentAddress = customer.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent);
        var mailngAddress = customer.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Mailing);
        var country = countries.FirstOrDefault(t => t.Id == customer.NaturalPerson.CitizenshipCountriesId.FirstOrDefault());

        return new GetCustomersResponseCustomer
        {
            Agent = customerDetail.Agent,
            Email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Value,
            Mobile = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.MobilPrivate)?.Value,
            KBID = customer.Identity.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            RoleName = ((CIS.Foms.Enums.CustomerRoles)customerDetail.Role).GetAttribute<DisplayAttribute>()!.Name,
            DateOfBirth = customer.NaturalPerson?.DateOfBirth,
            LastName = customer.NaturalPerson?.LastName,
            FirstName = customer.NaturalPerson?.FirstName,
            PermanentAddress = permanentAddress,
            ContactAddress = mailngAddress,
            CitizenshipCountry = country is null ? null : new()
            {
                Id = country.Id,
                Name = country?.Name
            }
        };
    }
}
