using DomainServices.CodebookService.Contracts.Endpoints.BankCodes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<BankCodeItem>> BankCodes(BankCodesRequest request, CallContext context = default);
}