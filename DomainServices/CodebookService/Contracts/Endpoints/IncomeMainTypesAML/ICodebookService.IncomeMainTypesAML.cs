using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithRdmCode>> IncomeMainTypesAML(Endpoints.IncomeMainTypesAML.IncomeMainTypesAMLRequest request, CallContext context = default);
    }
}