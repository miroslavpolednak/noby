
namespace DomainServices.CodebookService.Contracts.Endpoints.StatementTypes;

[DataContract]
public sealed class StatementTypesRequest : IRequest<List<StatementTypeItem>> { }
