using SharedTypes.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace NOBY.Api.Endpoints.Document.SharedDto;

internal class GetDocumentBaseRequest : IRequest<ReadOnlyMemory<byte>>
{
    public required DocumentTypes DocumentType { get; init; }

    public int DocumentTemplateVersionId { get; set; }

    public int? DocumentTemplateVariantId { get; set; }

    public required InputParameters InputParameters { get; set; }

    public bool? ForPreview { get; set; }
}