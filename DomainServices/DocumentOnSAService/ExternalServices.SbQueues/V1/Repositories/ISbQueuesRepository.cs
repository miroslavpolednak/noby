using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbQueues.V1.Model;

namespace DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;

public interface ISbQueuesRepository : IExternalServiceClient
{
    const string Version = "V1";

    Task<Attachment?> GetAttachmentById(string attachmentId, bool getMetadataOnly, CancellationToken cancellationToken);

    Task<Document?> GetDocumentByExternalId(string documentId, bool getMetadataOnly, CancellationToken cancellationToken);

    Task UpdateAttachmentProcessingDate(long attachmentId, DateTime? processingDate, CancellationToken cancellationToken);

    Task UpdateDocumentProcessingDate(long documentId, DateTime? processingDate, CancellationToken cancellationToken);

    Task UpdateClientProcessingDate(long documentId, DateTime? processingDate, CancellationToken cancellationToken);

    Task<long> GetDocumentIdAccordingAtchId(string attachmentId, CancellationToken cancellationToken);
}