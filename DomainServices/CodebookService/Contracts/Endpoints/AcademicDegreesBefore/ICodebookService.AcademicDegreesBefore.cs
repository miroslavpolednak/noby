using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> AcademicDegreesBefore(Endpoints.AcademicDegreesBefore.AcademicDegreesBeforeRequest request, CallContext context = default);
    }
}
