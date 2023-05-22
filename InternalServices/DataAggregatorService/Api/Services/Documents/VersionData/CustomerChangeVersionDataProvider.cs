using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, SelfService]
internal class CustomerChangeVersionDataProvider : IDocumentVersionDataProvider
{
    private readonly IDocumentVersionDataProvider _documentVersionDataProvider;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;

    public CustomerChangeVersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider, ISalesArrangementServiceClient salesArrangementService, ICodebookServiceClient codebookService)
    {
        _documentVersionDataProvider = documentVersionDataProvider;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
    }

    public async Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

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
            var variantName = await LoadVariantName(request.InputParameters.SalesArrangementId!.Value, cancellationToken);
            variant = VersionVariantHelper.GetDocumentVariant(variants, versionData.VersionId, variantName);
        }

        return versionData with { VariantId = variant.Id, VariantName = variant.DocumentVariant };
    }

    public async Task<string> LoadVariantName(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        return salesArrangement.CustomerChange.Applicants.Count switch
        {
            1 => "A",
            2 => "B",
            3 => "C",
            4 => "D",
            _ => throw new NotImplementedException()
        };
    }
}