namespace NOBY.Api.SharedDto;

public static class IdentificationDocumentExtensions
{
    public static DomainServices.CustomerService.Contracts.IdentificationDocument ToDomainService(this IdentificationDocumentFull document)
       => new()
       {
           IssuingCountryId = document.IssuingCountryId,
           IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
           IssuedBy = document.IssuedBy,
           Number = document.Number,
           IssuedOn = document.IssuedOn, // ?? DateTime.Today.AddYears(-1), //Mock HFICH-4410
           ValidTo = document.ValidTo // ?? DateTime.Today.AddYears(2) //Mock
       };
    
    public static IdentificationDocumentFull ToResponseDto(this DomainServices.CustomerService.Contracts.IdentificationDocument document)
        => new()
        {
            IssuingCountryId = document.IssuingCountryId ?? 0,
            IdentificationDocumentTypeId = document.IdentificationDocumentTypeId,
            IssuedBy = document.IssuedBy,
            IssuedOn = document.IssuedOn,
            RegisterPlace = document.RegisterPlace,
            ValidTo = document.ValidTo,
            Number = document.Number
        };
}