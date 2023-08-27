using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ExtendedObjects;

internal class CustomerDetailExtended : ExtendedObject<CustomerDetailResponse>
{
    public NaturalPerson NaturalPerson => Source.NaturalPerson;

    public ICollection<GrpcAddress> Addresses => Source.Addresses;

    public ICollection<Contact> Contacts => Source.Contacts;

    public GrpcAddress? PermanentAddress => Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent);

    public GrpcAddress? MailingAddress => Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Mailing);

    public string FullName => $"{NaturalPerson.FirstName} {NaturalPerson.LastName}";

    public string FullNameWitDegree => !NaturalPerson.DegreeBeforeId.HasValue ? FullName : $"{FullName}, {CodebookManager.DegreesBefore.First(d => d.Id == NaturalPerson.DegreeBeforeId.Value).Name}";

    public CountriesResponse.Types.CountryItem? BirthCountry => CodebookManager.Countries.FirstOrDefault(c => c.Id == NaturalPerson.BirthCountryId); 

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configure)
    {
        configure.DegreesBefore().Countries();
    }
}