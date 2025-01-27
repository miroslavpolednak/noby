﻿using DomainServices.CodebookService.Clients;
using ExternalServices.MpHome.V1.Contracts;

namespace DomainServices.CustomerService.Api.Services;

[ScopedService, SelfService]
internal sealed class MpHomeDetailMapper(
    IMediator _mediator,
    ICodebookServiceClient _codebookService)
{
    public async Task<Customer> MapDetailResponse(PartnerResponse partner, CancellationToken cancellationToken)
    {
        var titles1 = await _codebookService.AcademicDegreesBefore(cancellationToken);
        var titles2 = await _codebookService.AcademicDegreesAfter(cancellationToken);

        Customer customer = new()
        {
            Identities = { getIdentities(partner.Id, partner.KbId) },
            NaturalPerson = new()
            {
                BirthNumber = partner.BirthNumber ?? "",
                FirstName = partner.Name ?? "",
                LastName = partner.Lastname ?? "",
                GenderId = (int)(partner.Gender == GenderEnum.Male ? Genders.Male : Genders.Female),
                DateOfBirth = partner.DateOfBirth,
                PlaceOfBirth = partner.PlaceOfBirth ?? "",
                DegreeBeforeId = titles1.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeBefore, StringComparison.OrdinalIgnoreCase))?.Id,
                DegreeAfterId = titles2.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeAfter, StringComparison.OrdinalIgnoreCase))?.Id,
                IsPoliticallyExposed = partner.Pep,
                IsUSPerson = partner.UsPerson
            },
            IdentificationDocument = mapIdentificationDocument(partner)
        };
        
        if (partner.NationalityId.HasValue)
        {
            customer.NaturalPerson.CitizenshipCountriesId.Add(partner.NationalityId.Value);
        }

        if (partner.Addresses is not null)
        {
            foreach (var address in partner.Addresses)
            {
                var parsedAddress = await mapAddress(address, cancellationToken);

                if (parsedAddress is null)
                    continue;
                    
                customer.Addresses.Add(parsedAddress);
            }
        }

        if (partner.Contacts is not null)
        {
            customer.Contacts.AddRange(partner.Contacts.Select(t => t.ToContract()).Where(contact => contact is not null));
        }

        return customer;
    }

    private static Contracts.IdentificationDocument? mapIdentificationDocument(PartnerResponse partner)
    {
        var idDoc = partner.IdentificationDocuments?.FirstOrDefault();
        if (idDoc is null || idDoc.Type == IdentificationCardType.Undefined)
        {
            return null;
        }

        return new()
        {
            Number = idDoc.Number ?? string.Empty,
            IdentificationDocumentTypeId = (int)idDoc.Type,
            IssuedOn = idDoc.IssuedOn,
            IssuedBy = idDoc.IssuedBy ?? string.Empty,
            IssuingCountryId = idDoc.IssuingCountryId,
            ValidTo = idDoc.ValidTo
        };
    }

    private static IEnumerable<SharedTypes.GrpcTypes.Identity> getIdentities(long partnerId, long? kbId)
    {
        yield return new SharedTypes.GrpcTypes.Identity(partnerId, IdentitySchemes.Mp);

        if (kbId.HasValue)
            yield return new SharedTypes.GrpcTypes.Identity(kbId.Value, IdentitySchemes.Kb);
    }

    private async Task<SharedTypes.GrpcTypes.GrpcAddress?> mapAddress(AddressData? address, CancellationToken cancellationToken)
    {
        if (address is null || string.IsNullOrWhiteSpace(address.LandRegistryNumber) || string.IsNullOrWhiteSpace(address.PostCode))
        {
            return null;
        }

        var result = new SharedTypes.GrpcTypes.GrpcAddress
        {
            AddressTypeId = (int)address.Type,
            Street = address.Street ?? string.Empty,
            StreetNumber = address.BuildingIdentificationNumber ?? string.Empty,
            HouseNumber = address.LandRegistryNumber ?? string.Empty,
            Postcode = address.PostCode ?? string.Empty,
            City = address.City ?? string.Empty,
            CountryId = address.CountryId
        };

        if (!string.IsNullOrWhiteSpace(address.City))
        {
            var formatAddressResponse = await _mediator.Send(new FormatAddressRequest { Address = result }, cancellationToken);

            result.SingleLineAddressPoint = formatAddressResponse.SingleLineAddress;
        }

        return result;
    }
}
