
namespace DomainServices.CodebookService.Contracts.Endpoints.PostCodes;

[DataContract]
public sealed class PostCodesRequest : IRequest<List<PostCodeItem>> { }
