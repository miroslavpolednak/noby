using DomainServices.CodebookService.Contracts.Endpoints.SigningMethodsForNaturalPerson;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<SigningMethodsForNaturalPersonItem>> SigningMethodsForNaturalPerson(SigningMethodsForNaturalPersonRequest request, CallContext context = default);
    }
}
