using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace NOBY.Api.Endpoints.Document.Shared;

internal class GetDocumentBaseRequest : IRequest<ReadOnlyMemory<byte>>
{
    public required DocumentType DocumentType { get; init; }

    public string? DocumentTemplateVersion { get; set; }

    public required InputParameters InputParameters { get; set; }
}