using NOBY.Dto.Documents;
using __Contract = DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.UserService.Contracts;
using CIS.Core.Security;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.DocumentHelper;

[TransientService, AsImplementedInterfacesService]
internal sealed class DocumentHelperService
    : IDocumentHelperService
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public List<EaCodesMainResponse.Types.EaCodesMainItem> EaCodeMainItems { get; set; } = null!;

    public DocumentHelperService(
        ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _currentUserAccessor = currentUserAccessor;
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
            FormId = s.FormId,
            FileName = s.Filename,
            UploadStatus = getUploadStatus(s.StatusInQueue),
            CreatedOn = s.CreatedOn,
            Description = s.Description
        });
    }

    public IEnumerable<DocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return getDocumentListResult.Metadata.Select(s => new DocumentsMetadata
        {
            DocumentId = s.DocumentId,
            EaCodeMainId = s.EaCodeMainId,
            FormId = s.FormId,
            FileName = s.Filename,
            Description = s.Description,
            CreatedOn = s.CreatedOn,
            UploadStatus = getUploadStatus(400) // 400 mean saved in EArchive
        });
    }

    public async Task<IEnumerable<DocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<DocumentsMetadata> docMetadata, CancellationToken cancellationToken)
    {
        EaCodeMainItems = await _codebookService.EaCodesMain(cancellationToken);

        var query = docMetadata.Select(data =>
         new
         {
             docData = data,
             eACodeMainObj = EaCodeMainItems.Find(r => r.Id == data.EaCodeMainId)
         })
         .Where(f => f.eACodeMainObj?.IsVisibleForKb == true);

        return query.Select(s => s.docData);
    }

    public async Task<IReadOnlyCollection<CategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<DocumentsMetadata> documentsMetadata, CancellationToken cancellationToken)
    {
        EaCodeMainItems ??= await _codebookService.EaCodesMain(cancellationToken);

        var dataWithEaCodeMain = documentsMetadata.Select(data =>
        new
        {
            docData = data,
            eACodeMainObj = EaCodeMainItems.Find(r => r.Id == data.EaCodeMainId)
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

    public string GetAuthorUserLoginForDocumentUpload(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.UserInfo.Icp))
            return user.UserInfo.Icp;
        else if (!string.IsNullOrWhiteSpace(user.UserInfo.Cpm))
            return user.UserInfo.Cpm;
        else if (_currentUserAccessor?.User?.Id is not null)
            return _currentUserAccessor.User!.Id.ToString(CultureInfo.InvariantCulture);
        else
            throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "Cannot get NOBY user identifier");
    }

    private static UploadStatuses getUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => UploadStatuses.InProgress,
        300 => UploadStatuses.Error,
        400 => UploadStatuses.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}
