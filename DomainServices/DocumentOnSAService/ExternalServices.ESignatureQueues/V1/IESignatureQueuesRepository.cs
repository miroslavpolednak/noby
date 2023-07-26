using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.ESignatureQueues.V1;

public interface IESignatureQueuesRepository : IExternalServiceClient
{
    const string Version = "V1";

    Task<string> GetAttachmentExternalId(string attachmentId, CancellationToken cancellationToken);
}