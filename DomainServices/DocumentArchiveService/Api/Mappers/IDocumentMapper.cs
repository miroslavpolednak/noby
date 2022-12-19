using DomainServices.DocumentArchiveService.Contracts;
using ExternalServices.Sdf.V1.Model;
using ExternalServicesTcp.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.Mappers;

public interface IDocumentMapper
{
    DocumentMetadata MapSdfDocumentMetadata(MetadataValue[] values);

    FindSdfDocumentsQuery MapSdfFindDocumentQuery(GetDocumentListRequest request);

    FindTcpDocumentQuery MapTcpDocumentQuery(GetDocumentListRequest request);

    DocumentMetadata MapTcpDocumentMetadata(DocumentServiceQueryResult result);
}
