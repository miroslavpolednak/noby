using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.ESignatureQueues.V1.Model;

namespace ExternalServices.ESignatureQueues.V1;

public interface IESignatureQueuesRepository : IExternalServiceClient
{
    const string Version = "V1";

    Task<Attachment?> GetAttachmentById(long attachmentId, CancellationToken cancellationToken);

    Task<Document?> GetDocumentByExternalId(string externalId, CancellationToken cancellationToken);
}