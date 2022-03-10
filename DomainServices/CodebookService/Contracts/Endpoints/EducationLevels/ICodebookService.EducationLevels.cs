using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> EducationLevels(Endpoints.EducationLevels.EducationLevelsRequest request, CallContext context = default);
    }
}