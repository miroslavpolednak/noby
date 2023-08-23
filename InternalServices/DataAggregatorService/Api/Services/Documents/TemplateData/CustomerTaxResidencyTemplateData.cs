﻿using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class CustomerTaxResidencyTemplateData : AggregatedData
{
    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string SignerName => CustomerHelper.FullName(Customer);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer, AddressTypes.Permanent);

    public string? CorrespondenceAddress
    {
        get
        {
            var address = CustomerHelper.FullAddress(Customer, AddressTypes.Mailing);

            return string.IsNullOrWhiteSpace(address) ? null : address;
        }
    }

    public string BirthPlace
    {
        get
        {
            var birthCountry = _codebookManager.Countries.FirstOrDefault(c => c.Id == Customer.NaturalPerson.BirthCountryId);

            return birthCountry is null ? Customer.NaturalPerson.PlaceOfBirth : $"{Customer.NaturalPerson.PlaceOfBirth}, {birthCountry.LongName}";
        }
    }

    public string[] TaxResidencyCountries { get; private set; } = Array.Empty<string>();

    public string[] TaxResidencyCountriesTinMandatory { get; private set; } = Array.Empty<string>();

    public string[] TaxResidencyCountriesMissingTinReason { get; private set; } = Array.Empty<string>();

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        TaxResidencyCountries = GetTaxResidencyCountries().Select(country => country?.LongName ?? "N/A").ToArray();

        var taxResidencyCountriesTinMandatory = GetTaxResidencyCountries().Select(country => _codebookManager.TinNoFillReasonsByCountry.FirstOrDefault(t => t.Id == country?.ShortName)?.IsTinMandatory ?? false).ToList();

        TaxResidencyCountriesTinMandatory = taxResidencyCountriesTinMandatory.Select(required => required ? "Ano" : "Ne").ToArray();

        TaxResidencyCountriesMissingTinReason = GetTaxResidencyCountries().Select((country, index) =>
        {
            var residencyCountry = Customer.NaturalPerson.TaxResidence.ResidenceCountries[index];

            if (taxResidencyCountriesTinMandatory[index] && !string.IsNullOrWhiteSpace(residencyCountry.Tin))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(residencyCountry.TinMissingReasonDescription))
                return _codebookManager.TinNoFillReasonsByCountry.FirstOrDefault(t => t.Id == country?.ShortName)?.ReasonForBlankTin ?? string.Empty;

            return residencyCountry.TinMissingReasonDescription;
        }).ToArray();

        return Task.CompletedTask;
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().Countries().TinNoFillReasonsByCountry();
    }

    private IEnumerable<CountriesResponse.Types.CountryItem?> GetTaxResidencyCountries() => 
        Customer.NaturalPerson.TaxResidence.ResidenceCountries.Select(r => _codebookManager.Countries.FirstOrDefault(c => c.Id == r.CountryId));
}