using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> EmploymentTypes(Endpoints.EmploymentTypes.EmploymentTypesRequest request, CallContext context = default);
    }
}