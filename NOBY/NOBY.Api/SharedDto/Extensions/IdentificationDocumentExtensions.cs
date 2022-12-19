namespace NOBY.Api.SharedDto;

public static class IdentificationDocumentExtensions
{
    public static DomainServices.CustomerService.Contracts.IdentificationDocument ToDomainService(this IdentificationDocumentWithIssuedBy document)
       => new()
       {
           IssuingCountryId = document.IssuingCountryId,
           IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
           IssuedBy = document.IssuedBy,
           Number = document.Number
       };

    public static IdentificationDocumentFull ToResponseDto(this DomainServices.CustomerService.Contracts.IdentificationDocument document)
       => new()
       {
           IssuingCountryId = document.IssuingCountryId,
           IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
           IssuedBy = document.IssuedBy,
           IssuedOn = document.IssuedOn,
           RegisterPlace = document.RegisterPlace,
           ValidTo = document.ValidTo,
           Number = document.Number
       };
}