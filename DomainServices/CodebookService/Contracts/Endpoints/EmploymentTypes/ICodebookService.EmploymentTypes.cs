using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithCode>> EmploymentTypes(Endpoints.EmploymentTypes.EmploymentTypesRequest request, CallContext context = default);
    }
}