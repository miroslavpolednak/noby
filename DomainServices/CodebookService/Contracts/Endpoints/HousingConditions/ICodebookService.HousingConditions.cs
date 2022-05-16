using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.HousingConditions.HousingConditionItem>> HousingConditions(Endpoints.HousingConditions.HousingConditionsRequest request, CallContext context = default);
    }
}