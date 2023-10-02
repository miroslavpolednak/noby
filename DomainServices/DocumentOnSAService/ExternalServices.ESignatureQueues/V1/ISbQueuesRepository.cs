﻿using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.SbQueues.V1.Model;

namespace ExternalServices.SbQueues.V1;

public interface ISbQueuesRepository : IExternalServiceClient
{
    const string Version = "V1";

    Task<Attachment?> GetAttachmentById(string attachmentId, CancellationToken cancellationToken);

    Task<Document?> GetDocumentByExternalId(string documentId, CancellationToken cancellationToken);
}