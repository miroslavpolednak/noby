using DomainServices.CodebookService.Contracts.Endpoints.ClassficationOfEconomicActivities;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<ClassficationOfEconomicActivitiesItem>> ClassficationOfEconomicActivities(ClassficationOfEconomicActivitiesRequest request, CallContext context = default);
}