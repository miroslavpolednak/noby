using DomainServices.CodebookService.Contracts.Endpoints.TinNoFillReasonsByCountry;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<TinNoFillReasonItem>> TinNoFillReasonsByCountry(TinNoFillReasonsByCountryRequest request, CallContext context = default);
}
