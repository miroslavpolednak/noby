using CIS.Core;
using CIS.Infrastructure.gRPC.CisTypes;
using System.ComponentModel.DataAnnotations;
using _Cust = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Cases.GetCustomers;

internal static class GetCustomersExtensions
{
    public static GetCustomersResponseCustomer ToApiResponse(this _Cust.CustomerDetailResponse customer, List<(Identity Identity, int Role)> customerIdentities, List<DomainServices.CodebookService.Contracts.Endpoints.Countries.CountriesItem> countries)
    {
        var permanentAddress = customer.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Permanent);
        var mailngAddress = customer.Addresses?.FirstOrDefault(t => t.AddressTypeId == (int)CIS.Foms.Enums.AddressTypes.Mailing);
        var country = countries.FirstOrDefault(t => t.Id == customer.NaturalPerson.CitizenshipCountriesId.FirstOrDefault());

        return new GetCustomersResponseCustomer
        {
            //Agent = 
            Email = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Value,
            Mobile = customer.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.MobilPrivate)?.Value,
            KBID = customer.Identity.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
            RoleName = ((CIS.Foms.Enums.CustomerRoles)customerIdentities.First(x => x.Identity.IdentityId == customer.Identity.IdentityId).Role).GetAttribute<DisplayAttribute>()!.Name,
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
