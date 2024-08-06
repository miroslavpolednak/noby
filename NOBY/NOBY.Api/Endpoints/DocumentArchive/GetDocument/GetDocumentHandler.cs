using CIS.Core.Security;
using CIS.Infrastructure.gRPC;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using System.Net;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentHandler(
    ICurrentUserAccessor _currentUserAccessor,
    IDocumentArchiveServiceClient _documentArchiveService,
    IDocumentOnSAServiceClient _documentOnSAService,
    ILogger<GetDocumentHandler> _logger) 
    : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        return request.Source switch
        {
            EnumDocumentSource.EArchive => await HandleByEArchive(request.DocumentId ?? string.Empty, cancellationToken),
            EnumDocumentSource.SbDocument or EnumDocumentSource.SbAttachment => await HandleBySb(request.ExternalId ?? string.Empty, request.Source, cancellationToken),
            _ => throw new NobyValidationException($"Unsupported kind of source {request.Source}")
        };
    }

    private async Task<GetDocumentResponse> HandleByEArchive(string documentId, CancellationToken cancellationToken)
    {
        try
        {
            return await GetDocumentFromEArchive(documentId, cancellationToken);
        }
        catch (Exception exp)
        {
            _logger.LogError(exp, "Error when getting document");
            throw new NobyValidationException(90054, (int)HttpStatusCode.BadRequest);
        }
    }

    private async Task<GetDocumentResponse> GetDocumentFromEArchive(string documentId, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var documentResponse = await _documentArchiveService.GetDocument(new()
        {
            DocumentId = documentId,
            UserLogin = user is null ? "Unknown NOBY user" : user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            GetLocalCopyAsBackup = true,
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

    private async Task<GetDocumentResponse> HandleBySb(string externalId, EnumDocumentSource source, CancellationToken cancellationToken)
    {
        if (!_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_Manage) && !_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_RefinancingManage))
        {
            throw new CisAuthorizationException("DOCUMENT_SIGNING_Manage or DOCUMENT_SIGNING_RefinancingManage permission missing");
        }

        if (!_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_Manage))
        {
            throw new CisAuthorizationException("DOCUMENT_SIGNING_Manage permission missing");
        }

        if (!_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_DownloadWorkflowDocument))
        {
            throw new CisAuthorizationException("DOCUMENT_SIGNING_Manage permission missing");
        }

        var request = source switch
        {
            EnumDocumentSource.SbAttachment => new GetElectronicDocumentFromQueueRequest
            {
                DocumentAttachment = new()
                {
                    AttachmentId = externalId
                }
            },
            EnumDocumentSource.SbDocument => new GetElectronicDocumentFromQueueRequest
            {
                MainDocument = new()
                {
                    DocumentId = externalId
                }
            },
            _ => throw new NotSupportedException($"Unsupported kind of source {source}"),
        };

        var response = await _documentOnSAService.GetElectronicDocumentFromQueue(request, cancellationToken);

        return new GetDocumentResponse
        {
            Content = new()
            {
                BinaryData = response.BinaryData.ToArrayUnsafe(),
                MimeType = response.MimeType
            },
            Metadata = new()
            {
                Filename = response.Filename
            }
        };
    }
}
