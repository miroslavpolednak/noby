using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration;
using DomainServices.CodebookService.Clients;
using NOBY.Api.Endpoints.Document.Shared;
using System.Globalization;
using CIS.Core.Attributes;
using System.Runtime.InteropServices;

namespace NOBY.Api.Endpoints.Document;

[TransientService, SelfService]
public class DocumentManager
{
    private readonly ICodebookServiceClients _codebookService;
    private readonly ICurrentUserAccessor _userAccessor;

    public DocumentManager(ICodebookServiceClients codebookService, ICurrentUserAccessor userAccessor)
    {
        _codebookService = codebookService;
        _userAccessor = userAccessor;
    }

    public int UserId => _userAccessor.User!.Id;

    internal async Task<TRequest> CreateRequest<TRequest>(DocumentTemplateType templateType, InputParameters inputParameters, CancellationToken cancellationToken)
        where TRequest : GetDocumentBaseRequest, new()
    {
        var templateVersion = await GetTemplateVersion((int)templateType, cancellationToken);

        return new TRequest
        {
            TemplateType = templateType,
            TemplateVersion = templateVersion,
            InputParameters = inputParameters
        };
    }

    internal InputParameters GetOfferInput(int offerId) =>
        new()
        {
            OfferId = offerId,
            UserId = UserId
        };

    internal InputParameters GetSalesArrangementInput(int salesArrangementId) =>
        new()
        {
            SalesArrangementId = salesArrangementId,
            UserId = UserId
        };

    internal byte[] GetByteArray(ReadOnlyMemory<byte> memory)
    {
        if (!MemoryMarshal.TryGetArray(memory, out var arraySegment))
            throw new InvalidOperationException("Failed to get memory of document buffer");

        return arraySegment.Array!;
    }

    internal async Task<string> GetFileName(GetDocumentBaseRequest baseRequest, CancellationToken cancellationToken)
    {
        var nameParts = new[]
        {
            await GetTemplateShortName(),
            baseRequest.InputParameters.CaseId?.ToString(CultureInfo.InvariantCulture),
            DateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)
        };

        return string.Join("_", nameParts.Where(str => !string.IsNullOrWhiteSpace(str))) + ".pdf";

        async Task<string> GetTemplateShortName()
        {
            var templates = await _codebookService.DocumentTemplateTypes(cancellationToken);

            return templates.First(t => t.Id == (int)baseRequest.TemplateType).ShortName;
        }
    }

    private async Task<string> GetTemplateVersion(int templateTypeId, CancellationToken cancellationToken)
    {
        var versions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        return versions.Where(x => x.DocumentTemplateTypeId == templateTypeId)
                       .Select(x => x.DocumentVersion)
                       .FirstOrDefault() ?? throw new CisValidationException($"Document Version was not found for template type {templateTypeId}");
    }
}