using DomainServices.CodebookService.Contracts.Endpoints.StatementFrequencies;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<StatementFrequencyItem>> StatementFrequencies(StatementFrequenciesRequest request, CallContext context = default);
}
