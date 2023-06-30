using CIS.Core.Security;
using CIS.Infrastructure.gRPC;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentOnSAServiceClient _documentOnSaService;
    

    public GetDocumentHandler(
        ICurrentUserAccessor currentUserAccessor,
        IDocumentArchiveServiceClient documentArchiveService,
        IDocumentOnSAServiceClient documentOnSaService)
    {
        _currentUserAccessor = currentUserAccessor;
        _documentArchiveService = documentArchiveService;
        _documentOnSaService = documentOnSaService;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        return request.Source switch
        {
            DocumentSource.EArchive => await HandleFromEArchive(request, cancellationToken),
            DocumentSource.QueueEPodpisy => await HandleFromEPodpisy(request, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private async Task<GetDocumentResponse> HandleFromEArchive(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;
        
        var documentResponse = await _documentArchiveService.GetDocument(new()
        {
            DocumentId = request.DocumentId,
            UserLogin = user is null ? "Unknown NOBY user" : user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            WithContent = true
        }, cancellationToken);

        return new GetDocumentResponse
        {
            Content = new FileInfo
            {
                BinaryData = documentResponse.Content.BinaryData.ToArrayUnsafe(),
                MimeType = documentResponse.Content.MineType
            },
            Metadata = new DocumentMetadata
            {
                CaseId = documentResponse.Metadata.CaseId,
                DocumentId = documentResponse.Metadata.DocumentId,
                EaCodeMainId = documentResponse.Metadata.EaCodeMainId,
                Filename = documentResponse.Metadata.Filename,
                Description = documentResponse.Metadata.Description,
                OrderId = documentResponse.Metadata.OrderId,
                CreatedOn = documentResponse.Metadata.CreatedOn,
                AuthorUserLogin = documentResponse.Metadata.AuthorUserLogin,
                Priority = documentResponse.Metadata.Priority,
                Status = documentResponse.Metadata.Status,
                FolderDocument = documentResponse.Metadata.FolderDocument,
                FolderDocumentId = documentResponse.Metadata.FolderDocumentId,
                DocumentDirection = documentResponse.Metadata.DocumentDirection,
                SourceSystem = documentResponse.Metadata.SourceSystem,
                FormId = documentResponse.Metadata.FormId,
                ContractNumber = documentResponse.Metadata.ContractNumber,
                PledgeAgreementNumber = documentResponse.Metadata.PledgeAgreementNumber,
                Completeness = documentResponse.Metadata.Completeness,
                MinorCodes = documentResponse.Metadata.MinorCodes.ToArray(),
            }
        };
    }

    private async Task<GetDocumentResponse> HandleFromEPodpisy(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _documentOnSaService.GetElectronicDocumentFromQueue(
            new GetElectronicDocumentFromQueueRequest
            {

            },
            cancellationToken);

        return new GetDocumentResponse
        {
            Content = new FileInfo
            {
                BinaryData = result.BinaryData.ToArrayUnsafe(),
            },
            Metadata = new DocumentMetadata()
        };
    }
}
