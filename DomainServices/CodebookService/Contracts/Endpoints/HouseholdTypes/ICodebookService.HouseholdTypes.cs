using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<HouseholdTypeItem>> HouseholdTypes(HouseholdTypesRequest request, CallContext context = default);
    }
}