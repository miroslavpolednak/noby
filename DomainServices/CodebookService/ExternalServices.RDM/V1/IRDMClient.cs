﻿using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CodebookService.ExternalServices.RDM.V1;

public interface IRDMClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<List<Contracts.GetCodebookResponse_CodebookEntry>> GetCodebookItems(string codebookCode, CancellationToken cancellationToken = default);

    Task<List<Contracts.GetCodebookMappingResponse_CodebookEntryMapping>> GetMappingItems(string codebookCode, CancellationToken cancellationToken = default);
}
