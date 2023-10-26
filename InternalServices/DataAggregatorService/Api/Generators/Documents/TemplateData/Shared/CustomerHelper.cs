using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;

public static class CustomerHelper
{
    public static string FullName(CustomerDetailResponse customerDetail) => $"{customerDetail.NaturalPerson.FirstName} {customerDetail.NaturalPerson.LastName}";

    public static string FullName(CustomerDetailResponse customerDetail, ICollection<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore)
    {
        if (!customerDetail.NaturalPerson.DegreeBeforeId.HasValue)
            return FullName(customerDetail);

        var degree = degreesBefore.FirstOrDefault(d => d.Id != 0 && d.Id == customerDetail.NaturalPerson.DegreeBeforeId.Value)?.Name;

        return string.IsNullOrWhiteSpace(degree) ? FullName(customerDetail) : $"{FullName(customerDetail)}, {degree}";
    }

    public static string NameWithDateOfBirth(string fullName, DateTime dateOfBirth)
    {
        return $"{fullName}, datum narození: {dateOfBirth.ToString("d", CultureProvider.GetProvider())}";
    }

    public static string FullAddress(CustomerDetailResponse customerDetail, AddressTypes addressType)
    {
        var address = customerDetail.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)addressType);

        return address is null ? string.Empty : address.SingleLineAddressPoint;
    }
}