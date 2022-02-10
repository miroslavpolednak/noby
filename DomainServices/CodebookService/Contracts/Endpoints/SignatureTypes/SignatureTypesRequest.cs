namespace DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;

[DataContract]
public class SignatureTypesRequest : IRequest<List<SignatureTypeItem>>
{
}