using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

public static class PartnerExtensions
{
    public static NaturalPersonBasicInfo ToNaturalPersonBasicInfo(this Partner partner)
    {
        return new NaturalPersonBasicInfo
        {
            FirstName = partner.FirstName ?? string.Empty,
            LastName = partner.LastName ?? string.Empty,
            GenderId = partner.GenderId,
            BirthNumber = partner.BirthNumber ?? string.Empty,
            DateOfBirth = partner.BirthDate
        };
    }

    public static IdentificationDocument ToIdentificationDocument(this Partner partner)
    {
        return new IdentificationDocument
        {
            Number = partner.IdentificationDocumentNumber ?? string.Empty,
            IdentificationDocumentTypeId = partner.IdentificationDocumentTypeId,
            IssuedOn = partner.IdentificationDocumentIssuedOn,
            IssuedBy = partner.IdentificationDocumentIssuedBy ?? string.Empty,
            IssuingCountryId = partner.IdentificationDocumentIssuingCountryId,
            ValidTo = partner.IdentificationDocumentValidTo
        };
    }
}