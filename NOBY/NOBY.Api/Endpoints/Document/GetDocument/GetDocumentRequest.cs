using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration;

namespace NOBY.Api.Endpoints.Document.GetDocument;

internal class GetDocumentRequest : IRequest<GetDocumentResponse>
{
    public required int TemplateTypeId { get; init; }

    public required string TemplateVersion { get; init; }

    public required InputParameters InputParameters { get; init; }

    public DocumentTemplateType TemplateType => (DocumentTemplateType)TemplateTypeId;
}