using DomainServices.UserService.Contracts;
using NOBY.ApiContracts;
using __Contract = DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Services.DocumentHelper;

public interface IDocumentHelperService
{
    IEnumerable<SharedTypesDocumentsMetadata> MergeDocuments(IEnumerable<SharedTypesDocumentsMetadata> documentList, IEnumerable<SharedTypesDocumentsMetadata> documentInQueue);

    IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult);

    IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult);

    Task<IEnumerable<SharedTypesDocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<SharedTypesDocumentsMetadata> docMetadata, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<SharedTypesDocumentsCategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<SharedTypesDocumentsMetadata> documentsMetadata, CancellationToken cancellationToken);

    string GetAuthorUserLoginForDocumentUpload(User user);
}