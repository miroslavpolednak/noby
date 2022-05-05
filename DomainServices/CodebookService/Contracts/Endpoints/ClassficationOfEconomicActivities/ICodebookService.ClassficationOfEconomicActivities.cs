using DomainServices.CodebookService.Contracts.Endpoints.ClassificationOfEconomicActivities;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItem>> ClassificationOfEconomicActivities(ClassificationOfEconomicActivitiesRequest request, CallContext context = default);
}