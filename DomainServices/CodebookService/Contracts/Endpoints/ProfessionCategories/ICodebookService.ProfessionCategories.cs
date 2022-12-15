using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.ProfessionCategories.ProfessionCategoryItem>> ProfessionCategories(Endpoints.ProfessionCategories.ProfessionCategoriesRequest request, CallContext context = default);
    }
}
