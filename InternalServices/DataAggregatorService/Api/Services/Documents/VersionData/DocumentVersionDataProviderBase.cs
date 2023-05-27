using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

internal abstract class DocumentVersionDataProviderBase : IDocumentVersionDataProvider
{
    private readonly IDocumentVersionDataProvider _documentVersionDataProvider;
    private readonly ICodebookServiceClient _codebookService;

    protected DocumentVersionDataProviderBase(IDocumentVersionDataProvider documentVersionDataProvider, ICodebookServiceClient codebookService)
    {
        _documentVersionDataProvider = documentVersionDataProvider;
        _codebookService = codebookService;
    }

    public async Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        var versionData = await _documentVersionDataProvider.GetDocumentVersionData(request, cancellationToken);

        var variants = await _codebookService.DocumentTemplateVariants(cancellationToken);

        DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem variant;

        if (request.DocumentTemplateVariantId.HasValue)
        {
            VersionVariantHelper.CheckIfDocumentVariantExists(variants, versionData.VersionId, request.DocumentTemplateVariantId.Value);

            variant = variants.First(v => v.Id == request.DocumentTemplateVariantId.Value && v.DocumentTemplateVersionId == versionData.VersionId);
        }
        else
        {
            var variantName = await LoadVariantName(request, cancellationToken);
            variant = VersionVariantHelper.GetDocumentVariant(variants, versionData.VersionId, variantName);
        }

        return versionData with { VariantId = variant.Id, VariantName = variant.DocumentVariant };
    }

    protected abstract Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken);
}