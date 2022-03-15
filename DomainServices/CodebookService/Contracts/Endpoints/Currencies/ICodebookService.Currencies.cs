using DomainServices.CodebookService.Contracts.Endpoints.Currencies;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<CurrenciesItem>> Currencies(CurrenciesRequest request, CallContext context = default);
    }
}
