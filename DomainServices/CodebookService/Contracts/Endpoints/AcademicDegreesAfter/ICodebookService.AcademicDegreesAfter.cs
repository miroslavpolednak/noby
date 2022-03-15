using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> AcademicDegreesAfter(Endpoints.AcademicDegreesAfter.AcademicDegreesAfterRequest request, CallContext context = default);
    }
}
