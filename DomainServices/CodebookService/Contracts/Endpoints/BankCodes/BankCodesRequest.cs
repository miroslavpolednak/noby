
namespace DomainServices.CodebookService.Contracts.Endpoints.BankCodes;

[DataContract]
public sealed class BankCodesRequest : IRequest<List<BankCodeItem>> { }
