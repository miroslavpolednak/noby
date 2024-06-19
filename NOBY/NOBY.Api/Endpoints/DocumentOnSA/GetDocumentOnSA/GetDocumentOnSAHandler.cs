using CIS.Core.Security;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using System.Globalization;
using DomainServices.SalesArrangementService.Clients;
using _DocOnSaSource = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSA;

public class GetDocumentOnSAHandler : IRequestHandler<GetDocumentOnSARequest, GetDocumentOnSAResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorServiceClient;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookServiceClients;
    private readonly TimeProvider _dateTime;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetDocumentOnSAHandler(
        ICurrentUserAccessor currentUserAccessor,
        IDocumentOnSAServiceClient documentOnSaClient,
        IDocumentGeneratorServiceClient documentGeneratorServiceClient,
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookServiceClients,
        TimeProvider dateTime)
    {
        _currentUserAccessor = currentUserAccessor;
        _documentOnSaClient = documentOnSaClient;
        _documentGeneratorServiceClient = documentGeneratorServiceClient;
        _salesArrangementService = salesArrangementService;
        _codebookServiceClients = codebookServiceClients;
        _dateTime = dateTime;
    }

    public async Task<GetDocumentOnSAResponse> Handle(GetDocumentOnSARequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
            throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        if (documentOnSa.DocumentTypeId is null)
            throw new NobyValidationException("Unknown DocumentTypeId, cannot generate document");

        var documentOnSaData = await _documentOnSaClient.GetDocumentOnSAData(documentOnSa.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSaData.SignatureTypeId is not null && documentOnSaData.SignatureTypeId != (int)SignatureTypes.Paper)
            throw new NobyValidationException("Only paper signed documents can be generated");

        if (!documentOnSaData.IsValid)
            throw new NobyValidationException("Unable to generate document for invalid document");

        if (!_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_Manage) && !_currentUserAccessor.HasPermission(UserPermissions.DOCUMENT_SIGNING_RefinancingManage))
        {
            throw new CisAuthorizationException("DOCUMENT_SIGNING_Manage or DOCUMENT_SIGNING_RefinancingManage permission missing");
        }

        return await GetDocumentFromDocumentGenerator(documentOnSa, documentOnSaData, cancellationToken);
    }

    private async Task<GetDocumentOnSAResponse> GetDocumentFromDocumentGenerator(_DocOnSaSource.DocumentOnSAToSign documentOnSa, _DocOnSaSource.GetDocumentOnSADataResponse documentOnSaData, CancellationToken cancellationToken)
    {
        //Should be in the cache, so load it because we need CaseId
        var saValidationResult = await _salesArrangementService.ValidateSalesArrangementId(documentOnSa.SalesArrangementId, false, cancellationToken);

        var generateDocumentRequest = DocumentOnSAExtensions.CreateGenerateDocumentRequest(documentOnSa, documentOnSaData, saValidationResult.CaseId, forPreview: false);

        var result = await _documentGeneratorServiceClient.GenerateDocument(generateDocumentRequest, cancellationToken);

        return new GetDocumentOnSAResponse
        {
            ContentType = MediaTypeNames.Application.Pdf,
            Filename = await GetFileName(documentOnSa, cancellationToken),
            FileData = result.Data.ToArrayUnsafe()
        };
    }

    private async Task<string> GetFileName(_DocOnSaSource.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.GetLocalNow().ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf";
    }
}
