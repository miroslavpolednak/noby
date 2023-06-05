using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public static class CustomerHelper
{
    public static string FullName(CustomerDetailResponse customerDetail) => $"{customerDetail.NaturalPerson.FirstName} {customerDetail.NaturalPerson.LastName}";

    public static string FullName(CustomerDetailResponse customerDetail, ICollection<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore)
    {
        if (!customerDetail.NaturalPerson.DegreeBeforeId.HasValue)
            return FullName(customerDetail);

        var degree = degreesBefore.First(d => d.Id == customerDetail.NaturalPerson.DegreeBeforeId.Value).Name;

        return $"{FullName(customerDetail)}, {degree}";
    }

    public static string NameWithDateOfBirth(string fullName, DateTime dateOfBirth)
    {
        return $"{fullName}, datum narození: {dateOfBirth.ToString("d", CultureProvider.GetProvider())}";
    }

    public static string FullAddress(CustomerDetailResponse customerDetail, AddressTypes addressType, ICollection<CountriesResponse.Types.CountryItem> countries)
    {
        var address = customerDetail.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)addressType);

        return address is null ? string.Empty : FullAddress(address, countries);
    }

    public static string FullAddress(GrpcAddress address, ICollection<CountriesResponse.Types.CountryItem> countries)
    {
        return $"{address.Street} {CombineHouseAndStreetNumber(address.HouseNumber, address.StreetNumber)}, " +
               $"{address.Postcode} {address.City}, " +
               $"{countries.Where(c => c.Id == address.CountryId).Select(c => c.LongName).FirstOrDefault("No country")}";
    }

    private static string CombineHouseAndStreetNumber(string houseNumber, string streetNumber) => 
        string.Join("/", new[] { houseNumber, streetNumber }.Where(str => !string.IsNullOrWhiteSpace(str)));
}