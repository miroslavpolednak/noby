using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1.Contracts;

namespace DomainServices.CustomerService.Api.Services;

[ScopedService, SelfService]
internal sealed class MpHomeDetailMapper(
    IMediator _mediator,
    ICodebookServiceClient _codebookService)
{
    public async Task<List<SearchCustomersItem>> MapSearchResponse(List<PartnerResponse>? partners, CancellationToken cancellationToken)
    {
        if (partners is null)
        {
            return [];
        }

        var response = new List<SearchCustomersItem>(partners.Count);

        foreach (PartnerResponse partner in partners)
        {
            var p = new SearchCustomersItem
            {
                Identity = new SharedTypes.GrpcTypes.Identity(partner.PartnerId, IdentitySchemes.Mp),
                NaturalPerson = new NaturalPersonBasicInfo
                {
                    BirthNumber = partner.BirthNumber,
                    FirstName = partner.Name,
                    LastName = partner.Lastname,
                    GenderId = (int)(partner.Gender == GenderEnum.Male ? Genders.Male : Genders.Female),
                    DateOfBirth = partner.DateOfBirth
                },
                IdentificationDocument = await mapIdentificationDocument(partner, cancellationToken),
                Address = partner.Addresses.FirstOrDefault()
            };
        }

        return response;
    }

    public async Task<CustomerDetailResponse> MapDetailResponse(PartnerResponse partner, CancellationToken cancellationToken)
    {
        var countries = await _codebookService.Countries(cancellationToken);
        var titles1 = await _codebookService.AcademicDegreesBefore(cancellationToken);
        var titles2 = await _codebookService.AcademicDegreesAfter(cancellationToken);

        CustomerDetailResponse customer = new()
        {
            Identities = { getIdentities(partner.PartnerId, partner.KbId) },
            NaturalPerson = new()
            {
                BirthNumber = partner.BirthNumber,
                FirstName = partner.Name,
                LastName = partner.Lastname,
                GenderId = (int)(partner.Gender == GenderEnum.Male ? Genders.Male : Genders.Female),
                DateOfBirth = partner.DateOfBirth,
                PlaceOfBirth = partner.PlaceOfBirth,
                DegreeBeforeId = titles1.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeBefore, StringComparison.OrdinalIgnoreCase))?.Id,
                DegreeAfterId = titles2.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeAfter, StringComparison.OrdinalIgnoreCase))?.Id,
                IsPoliticallyExposed = partner.Pep,
                IsUSPerson = partner.UsPerson
            },
            IdentificationDocument = await mapIdentificationDocument(partner, cancellationToken)
        };

        var nationalityId = countries.FirstOrDefault(t => t.ShortName == partner.Nationality)?.Id;
        if (nationalityId.HasValue)
        {
            customer.NaturalPerson.CitizenshipCountriesId.Add(nationalityId.Value);
        }

        if (partner.Addresses is not null)
        {
            foreach (var address in partner.Addresses)
            {
                customer.Addresses.Add(await mapAddress(address, cancellationToken));
            }
        }

        if (partner.Contacts is not null)
        {
            customer.Contacts.AddRange(partner.Contacts.Select(t => t.ToContract()));
        }

        return customer;
    }

    private async Task<Contracts.IdentificationDocument?> mapIdentificationDocument(PartnerResponse partner, CancellationToken cancellationToken)
    {
        var idDoc = partner.IdentificationDocuments?.FirstOrDefault();
        if (idDoc is null)
        {
            return null;
        }

        var countries = await _codebookService.Countries(cancellationToken);
        
        return new()
        {
            Number = idDoc.Number ?? string.Empty,
            IdentificationDocumentTypeId = (int)idDoc.Type,
            IssuedOn = idDoc.IssuedOn,
            IssuedBy = idDoc.IssuedBy ?? string.Empty,
            IssuingCountryId = countries.FirstOrDefault(t => t.ShortName == idDoc.IssuingCountry)?.Id,
            ValidTo = idDoc.ValidTo
        };
    }

    private static IEnumerable<SharedTypes.GrpcTypes.Identity> getIdentities(long partnerId, long? kbId)
    {
        yield return new SharedTypes.GrpcTypes.Identity(partnerId, IdentitySchemes.Mp);

        if (kbId.HasValue)
            yield return new SharedTypes.GrpcTypes.Identity(kbId.Value, IdentitySchemes.Kb);
    }

    private async Task<SharedTypes.GrpcTypes.GrpcAddress> mapAddress(AddressData address, CancellationToken cancellationToken)
    {
        var countries = await _codebookService.Countries(cancellationToken);

        var result = new SharedTypes.GrpcTypes.GrpcAddress
        {
            AddressTypeId = (int)address.Type,
            Street = address.Street ?? string.Empty,
            StreetNumber = address.BuildingIdentificationNumber ?? string.Empty,
            HouseNumber = address.LandRegistryNumber ?? string.Empty,
            Postcode = address.PostCode ?? string.Empty,
            City = address.City ?? string.Empty,
            CountryId = string.IsNullOrEmpty(address.Country) ? 16 : countries.FirstOrDefault(c => c.ShortName == address.Country)?.Id
        };

        if (!string.IsNullOrWhiteSpace(address.City))
        {
            var formatAddressResponse = await _mediator.Send(new FormatAddressRequest { Address = result }, cancellationToken);

            result.SingleLineAddressPoint = formatAddressResponse.SingleLineAddress;
        }

        return result;
    }
}
