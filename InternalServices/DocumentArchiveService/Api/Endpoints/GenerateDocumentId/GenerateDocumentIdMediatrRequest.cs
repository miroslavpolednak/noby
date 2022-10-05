﻿namespace CIS.InternalServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed record GenerateDocumentIdMediatrRequest(Contracts.GenerateDocumentIdRequest Request)
    : IRequest<Contracts.GenerateDocumentIdResponse>
{
}
