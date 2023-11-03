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

    public string FullNameWitDegree
    {
        get
        {
            if (!NaturalPerson.DegreeBeforeId.HasValue)
                return FullName;

            var degreeName = CodebookManager.DegreesBefore.FirstOrDefault(d => d.Id != 0 && d.Id == NaturalPerson.DegreeBeforeId.Value)?.Name;

            return string.IsNullOrWhiteSpace(degreeName) ? FullName : $"{FullName}, {degreeName}";
        }
    }

    public CountriesResponse.Types.CountryItem? BirthCountry => CodebookManager.Countries.FirstOrDefault(c => c.Id == NaturalPerson.BirthCountryId);

    public IEnumerable<NaturalPersonResidenceCountry> TaxResidenceCountriesLimited => NaturalPerson.TaxResidence.ResidenceCountries.Take(8);

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configure)
    {
        configure.DegreesBefore().Countries();
    }
}