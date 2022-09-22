using Microsoft.EntityFrameworkCore;

namespace FOMS.Api.Endpoints.Customer.Create;

internal static class CreateExtensions
{
    public DomainServices.CustomerService.Contracts.CreateCustomerRequest ToDomainService(this CreateRequest request)
    {
        var model = new DomainServices.CustomerService.Contracts.CreateCustomerRequest
        {
            NaturalPerson = new DomainServices.CustomerService.Contracts.NaturalPerson
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthNumber = request.CzechBirthNumber,
                DateOfBirth = request.BirthDate,
                PlaceOfBirth = request.BirthPlace,
                GenderId = request.GenderCode
            },
            IdentificationDocument = new DomainServices.CustomerService.Contracts.IdentificationDocument
            {
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
}
