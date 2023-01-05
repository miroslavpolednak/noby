using DomainServices.DocumentArchiveService.Contracts;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;

namespace DomainServices.DocumentArchiveService.Api.Mappers;

public interface IDocumentMapper
{
    DocumentMetadata MapSdfDocumentMetadata(MetadataValue[] values);

    FindSdfDocumentsQuery MapSdfFindDocumentQuery(GetDocumentListRequest request);

    FindTcpDocumentQuery MapTcpDocumentQuery(GetDocumentListRequest request);

    DocumentMetadata MapTcpDocumentMetadata(DocumentServiceQueryResult result);
}
