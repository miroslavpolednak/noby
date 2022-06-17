using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<EducationLevelItem>> EducationLevels(EducationLevelsRequest request, CallContext context = default);
}