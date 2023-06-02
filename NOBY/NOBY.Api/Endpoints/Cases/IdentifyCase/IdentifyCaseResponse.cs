﻿using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.Workflow.Dto;

namespace NOBY.Api.Endpoints.Cases.IdentifyCase;

public sealed class IdentifyCaseResponse
{
    /// <summary>
    /// ID obchodního případu
    /// </summary>
    /// <example>24777</example>
    public long? CaseId { get; set; }
    
    public WorkflowTask? Task { get; set; }
    
    public WorkflowTaskDetail? TaskDetail { get; set; }
    
    public List<DocumentsMetadata>? Documents { get; set; }
}