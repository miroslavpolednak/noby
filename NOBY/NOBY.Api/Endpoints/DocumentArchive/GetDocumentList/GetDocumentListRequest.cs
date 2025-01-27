﻿namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListRequest:IRequest<DocumentArchiveGetDocumentListResponse>
{
	public GetDocumentListRequest(long caseId, string? formId)
	{
        CaseId = caseId;
        FormId = formId;
	}

    public long CaseId { get; }

    public string? FormId { get; }
}
