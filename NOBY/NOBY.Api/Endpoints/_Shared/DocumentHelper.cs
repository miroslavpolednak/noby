using CIS.Core.Attributes;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using __Contract = DomainServices.DocumentArchiveService.Contracts;
using __Api = NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using DomainServices.CodebookService.Clients;
using System.Threading;
using DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;

namespace NOBY.Api.Endpoints.Shared;
public interface IDocumentHelper
{
    IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<__Api.DocumentsMetadata> documentList, IEnumerable<__Api.DocumentsMetadata> documentInQueue);

    IEnumerable<__Api.DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult);

    IEnumerable<__Api.DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult);

    Task<IEnumerable<__Api.DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<__Api.DocumentsMetadata> docMetadata, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<DocumentsMetadata> documentsMetadata, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class DocumentHelper : IDocumentHelper
{
    private readonly ICodebookServiceClients _codebookServiceClient;

    public List<EaCodeMainItem> EaCodeMainItems { get; set; } = null!;

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
        EaCodeMainItems = await _codebookServiceClient.EaCodesMain(cancellationToken);

        var query = docMetadata.Select(data =>
         new
         {
             docData = data,
             eACodeMainObj = EaCodeMainItems.FirstOrDefault(r => r.Id == data.EaCodeMainId)
         })
         .Where(f => f.eACodeMainObj is not null && f.eACodeMainObj.IsVisibleForKb);

        return query.Select(s => s.docData);
    }

    public async Task<IReadOnlyCollection<CategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<DocumentsMetadata> documentsMetadata, CancellationToken cancellationToken)
    {
        EaCodeMainItems = EaCodeMainItems ?? await _codebookServiceClient.EaCodesMain(cancellationToken);

        var dataWithEaCodeMain = documentsMetadata.Select(data =>
        new
        {
            docData = data,
            eACodeMainObj = EaCodeMainItems.FirstOrDefault(r => r.Id == data.EaCodeMainId)
        })
        .Where(f => f.eACodeMainObj is not null).ToList();

        var eaCodeMainCategories = dataWithEaCodeMain.Select(s => s.eACodeMainObj!.Category.Trim()).Distinct().ToList();

        var categoryEaCodeMains = new List<CategoryEaCodeMain>();

        foreach (var eaCodeMainCategory in eaCodeMainCategories)
        {
            var categoryEaCodeMain = new CategoryEaCodeMain
            {
                Name = eaCodeMainCategory,
                DocumentCountInCategory = dataWithEaCodeMain.Count(c => c.eACodeMainObj!.Category == eaCodeMainCategory),
                EaCodeMainIdList = dataWithEaCodeMain.Where(c => c.eACodeMainObj!.Category == eaCodeMainCategory)
                                                     .Select(s => s.docData.EaCodeMainId).Distinct().ToList(),
            };

            categoryEaCodeMains.Add(categoryEaCodeMain);
        }

        return categoryEaCodeMains;
    }
}
