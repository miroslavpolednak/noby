using CIS.Core.Attributes;
using NOBY.Dto.Documents;
using __Contract = DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Infrastructure.Services.DocumentHelper;

[TransientService, AsImplementedInterfacesService]
internal sealed class DocumentHelperService 
    : IDocumentHelperService
{
    private readonly ICodebookServiceClient _codebookServiceClient;

    public List<EaCodesMainResponse.Types.EaCodesMainItem> EaCodeMainItems { get; set; } = null!;

    public DocumentHelperService(ICodebookServiceClient codebookServiceClient)
    {
        _codebookServiceClient = codebookServiceClient;
    }

    public IEnumerable<DocumentsMetadata> MergeDocuments(IEnumerable<DocumentsMetadata> documentList, IEnumerable<DocumentsMetadata> documentInQueue)
    {
        return documentList.Concat(documentInQueue.Where(d => !documentList.Select(l => l.DocumentId)
                                                                        .Contains(d.DocumentId)));
    }

    public IEnumerable<DocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult)
    {
        return getDocumentsInQueueResult.QueuedDocuments.Select(s => new DocumentsMetadata
        {
            DocumentId = s.EArchivId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            UploadStatus = getUploadStatus(s.StatusInQueue)
        });
    }

    public IEnumerable<DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return getDocumentListResult.Metadata.Select(s => new DocumentsMetadata
        {
            DocumentId = s.DocumentId,
            EaCodeMainId = s.EaCodeMainId,
            FileName = s.Filename,
            Description = s.Description,
            CreatedOn = s.CreatedOn,
            UploadStatus = getUploadStatus(400) // 400 mean saved in EArchive
        });
    }

    public async Task<IEnumerable<DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<DocumentsMetadata> docMetadata, CancellationToken cancellationToken)
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

    private static UploadStatuses getUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => UploadStatuses.InProgress,
        300 => UploadStatuses.Error,
        400 => UploadStatuses.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}
