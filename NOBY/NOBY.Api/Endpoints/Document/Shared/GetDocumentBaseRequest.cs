using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration;

namespace NOBY.Api.Endpoints.Document.Shared;

internal class GetDocumentBaseRequest : IRequest<ReadOnlyMemory<byte>>
{
    public DocumentTemplateType TemplateType { get; init; }

    public string TemplateVersion { get; init; } = null!;

    public InputParameters InputParameters { get; init; } = null!;
}