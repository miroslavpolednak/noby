using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace NOBY.Api.Endpoints.Document.Shared;

internal class GetDocumentBaseRequest : IRequest<ReadOnlyMemory<byte>>
{
    public required DocumentType DocumentType { get; init; }

    public string? DocumentTemplateVersion { get; set; }

    public int? DocumentTemplateVariantId { get; set; }

    public string? DocumentTemplateVariant { get; set; }

    public required InputParameters InputParameters { get; set; }

    public bool? ForPreview { get; set; }
}