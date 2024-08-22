using __Contract = DomainServices.DocumentArchiveService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.UserService.Contracts;
using CIS.Core.Security;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.ApiContracts;
using DomainServices.UserService.Contracts.v1;

namespace NOBY.Services.DocumentHelper;

public interface IDocumentHelperServiceOld
{
    IEnumerable<SharedTypesDocumentsMetadata> MergeDocuments(IEnumerable<SharedTypesDocumentsMetadata> documentList, IEnumerable<SharedTypesDocumentsMetadata> documentInQueue);

    IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult);

    IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult);

    Task<IEnumerable<SharedTypesDocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<SharedTypesDocumentsMetadata> docMetadata, CancellationToken cancellationToken);

    Task<List<SharedTypesDocumentsCategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<SharedTypesDocumentsMetadata> documentsMetadata, CancellationToken cancellationToken);

    string GetAuthorUserLoginForDocumentUpload(DomainServices.UserService.Clients.Dto.UserDto user);
}

[ScopedService, AsImplementedInterfacesService]
internal sealed class DocumentHelperServiceOld
    : IDocumentHelperServiceOld
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public List<EaCodesMainResponse.Types.EaCodesMainItem> EaCodeMainItems { get; set; } = null!;

    public DocumentHelperServiceOld(
        ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _currentUserAccessor = currentUserAccessor;
    }

    public IEnumerable<SharedTypesDocumentsMetadata> MergeDocuments(IEnumerable<SharedTypesDocumentsMetadata> documentList, IEnumerable<SharedTypesDocumentsMetadata> documentInQueue)
    {
        return documentList.Concat(documentInQueue.Where(d => !documentList.Select(l => l.DocumentId)
                                                                        .Contains(d.DocumentId)));
    }

    public IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentsInQueueMetadata(__Contract.GetDocumentsInQueueResponse getDocumentsInQueueResult)
    {
        return getDocumentsInQueueResult.QueuedDocuments.Select(s => new SharedTypesDocumentsMetadata
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

    public IEnumerable<SharedTypesDocumentsMetadata> MapGetDocumentListMetadata(__Contract.GetDocumentListResponse getDocumentListResult)
    {
        return getDocumentListResult.Metadata.Select(s => new SharedTypesDocumentsMetadata
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

    public async Task<IEnumerable<SharedTypesDocumentsMetadata>> FilterDocumentsVisibleForKb(IEnumerable<SharedTypesDocumentsMetadata> docMetadata, CancellationToken cancellationToken)
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

    public async Task<List<SharedTypesDocumentsCategoryEaCodeMain>> CalculateCategoryEaCodeMain(List<SharedTypesDocumentsMetadata> documentsMetadata, CancellationToken cancellationToken)
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

        var categoryEaCodeMains = new List<SharedTypesDocumentsCategoryEaCodeMain>();

        foreach (var eaCodeMainCategory in eaCodeMainCategories)
        {
            var categoryEaCodeMain = new SharedTypesDocumentsCategoryEaCodeMain
            {
                Name = eaCodeMainCategory,
                DocumentCountInCategory = dataWithEaCodeMain.Count(c => c.eACodeMainObj!.Category == eaCodeMainCategory),
                EaCodeMainIdList = dataWithEaCodeMain.Where(c => c.eACodeMainObj!.Category == eaCodeMainCategory)
                                                     .Select(s => s.docData.EaCodeMainId ?? 0).Distinct().ToList(),
            };

            categoryEaCodeMains.Add(categoryEaCodeMain);
        }

        return categoryEaCodeMains;
    }

    public string GetAuthorUserLoginForDocumentUpload(DomainServices.UserService.Clients.Dto.UserDto user)
    {
        if (!string.IsNullOrWhiteSpace(user.UserInfo.Icp))
            return user.UserInfo.Icp;
        else if (!string.IsNullOrWhiteSpace(user.UserInfo.Cpm))
            return user.UserInfo.Cpm;
        else if (_currentUserAccessor?.User?.Id is not null)
            return _currentUserAccessor.User!.Id.ToString(CultureInfo.InvariantCulture);
        else
            throw new CisNotFoundException(ErrorCodeMapper.DefaultExceptionCode, "Cannot get NOBY user identifier");
    }

    private static SharedTypesDocumentsMetadataUploadStatus getUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => SharedTypesDocumentsMetadataUploadStatus.InProgress,
        300 => SharedTypesDocumentsMetadataUploadStatus.Error,
        400 => SharedTypesDocumentsMetadataUploadStatus.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}
