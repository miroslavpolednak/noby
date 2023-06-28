using NOBY.Dto.Documents;
using __Contract = DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Infrastructure.Services.DocumentHelper;

public interface IDocumentHelperService
{
    IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<DocumentsMetadata> documentList, IEnumerable<DocumentsMetadata> documentInQueue);

    IEnumerable<DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult);

    IEnumerable<DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult);

    Task<IEnumerable<DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<DocumentsMetadata> docMetadata, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<DocumentsMetadata> documentsMetadata, CancellationToken cancellationToken);
}