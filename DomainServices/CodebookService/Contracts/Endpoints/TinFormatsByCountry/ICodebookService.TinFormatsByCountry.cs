using DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<TinFormatItem>> TinFormatsByCountry(TinFormatsByCountryRequest request, CallContext context = default);
}
