using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1.Model;

namespace DomainServices.DocumentArchiveService.Api.Mappers;

public interface IDocumentMapper
{
    DocumentMetadata MapSdfDocumentMetadata(MetadataValue[] values);

    FindSdfDocumentsQuery MapSdfFindDocumentQuery(GetDocumentListRequest request);

    FindTcpDocumentQuery MapTcpDocumentQuery(GetDocumentListRequest request);

    DocumentMetadata MapTcpDocumentMetadata(DocumentServiceQueryResult result);
}
