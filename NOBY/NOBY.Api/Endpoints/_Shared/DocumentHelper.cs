using CIS.Core.Attributes;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using __Contract = DomainServices.DocumentArchiveService.Contracts;
using __Api = NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using DomainServices.CodebookService.Clients;
using System.Threading;

namespace NOBY.Api.Endpoints.Shared;
public interface IDocumentHelper
{
    IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<__Api.DocumentsMetadata> documentList, IEnumerable<__Api.DocumentsMetadata> documentInQueue);

    IEnumerable<__Api.DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult);

    IEnumerable<__Api.DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult);

    Task<IEnumerable<__Api.DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<__Api.DocumentsMetadata> docMetadata, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class DocumentHelper : IDocumentHelper
{
    private readonly ICodebookServiceClients _codebookServiceClient;

    public DocumentHelper(ICodebookServiceClients codebookServiceClient)
    {
        _codebookServiceClient = codebookServiceClient;
    }

    public IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<__Api.DocumentsMetadata> documentList, IEnumerable<__Api.DocumentsMetadata> documentInQueue)
    {
        return documentList.Concat(documentInQueue.Where(d => !documentList.Select(l => l.DocumentId)
                                                                        .Contains(d.DocumentId)));
    }

    public IEnumerable<__Api.DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult)
    {
        return getDocumentsInQueueResult.QueuedDocuments.Select(s => new DocumentsMetadata
        {
            DocumentId = s.EArchivId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            UploadStatus = UploadStatusHelper.GetUploadStatus(s.StatusInQueue)
        });
    }

    public IEnumerable<__Api.DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return getDocumentListResult.Metadata.Select(s => new DocumentsMetadata
        {
            DocumentId = s.DocumentId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            Description = s.Description,
            CreatedOn = s.CreatedOn,
            UploadStatus = UploadStatusHelper.GetUploadStatus(400) // 400 mean saved in EArchive
        });
    }

    public async Task<IEnumerable<__Api.DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<DocumentsMetadata> docMetadata, CancellationToken cancellationToken)
    {
        var eaCodeMain = await _codebookServiceClient.EaCodesMain(cancellationToken);

        return docMetadata.Select(data =>
         new
         {
             docData = data,
             eACodeMainObj = eaCodeMain.FirstOrDefault(r => r.Id == data.EaCodeMainId)
         })
         .Where(f => f.eACodeMainObj is not null && f.eACodeMainObj.IsVisibleForKb)
         .Select(s => s.docData);
    }
}
