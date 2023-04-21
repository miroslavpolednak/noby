using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public static class CustomerHelper
{
    public static string FullName(CustomerDetailResponse customerDetail, ICollection<GenericCodebookItem> degreesBefore)
    {
        if (!customerDetail.NaturalPerson.DegreeBeforeId.HasValue)
            return $"{customerDetail.NaturalPerson.FirstName} {customerDetail.NaturalPerson.LastName}";

        var degree = degreesBefore.First(d => d.Id == customerDetail.NaturalPerson.DegreeBeforeId.Value).Name;

        return $"{customerDetail.NaturalPerson.FirstName} {customerDetail.NaturalPerson.LastName}, {degree}";
    }

    public static string NameWithDateOfBirth(string fullName, DateTime dateOfBirth)
    {
        return $"{fullName}, datum narození: {dateOfBirth.ToString("d", CultureProvider.GetProvider())}";
    }

    public static string FullAddress(CustomerDetailResponse customerDetail, ICollection<CountriesItem> countries)
    {
        var address = customerDetail.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent);

        if (address is null)
            return string.Empty;

        return $"{address.Street} {CombineHouseAndStreetNumber(address.HouseNumber, address.StreetNumber)}, " +
               $"{address.Postcode} {address.City}, " +
               $"{countries.Where(c => c.Id == address.CountryId).Select(c => c.LongName).FirstOrDefault("No country")}";
    }

    private static string CombineHouseAndStreetNumber(string houseNumber, string streetNumber) => 
        string.Join("/", new[] { houseNumber, streetNumber }.Where(str => !string.IsNullOrWhiteSpace(str)));
}