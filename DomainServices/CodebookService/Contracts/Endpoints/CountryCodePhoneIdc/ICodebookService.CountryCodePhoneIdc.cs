using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<Endpoints.CountryCodePhoneIdc.CountryCodePhoneIdcItem>> CountryCodePhoneIdc(Endpoints.CountryCodePhoneIdc.CountryCodePhoneIdcRequest request, CallContext context = default);
}