﻿using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentsToArchiveRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    public List<DocumentsInformation> DocumentsInformation { get; set; } = null!;

    internal SaveDocumentsToArchiveRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}

public class DocumentsInformation
{
    public Dto.Documents.DocumentInformation DocumentInformation { get; set; } = null!;

    public string? FormId { get; set; }
}