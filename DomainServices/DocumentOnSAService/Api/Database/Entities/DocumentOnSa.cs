using CIS.Core.Data;

namespace DomainServices.DocumentOnSAService.Api.Database.Entities;

public class DocumentOnSa : ICreated
{
    public int DocumentOnSAId { get; set; }

    public int DocumentTypeId { get; set; }

    public int? DocumentTemplateVersionId { get; set; }

    public string FormId { get; set; } = null!;

    public string? EArchivId { get; set; }

    public string? DmsxId { get; set; }

    public int SalesArrangementId { get; set; }

    public int? HouseholdId { get; set; }

    public bool IsValid { get; set; }

    public bool IsSigned { get; set; }

    public bool IsDocumentArchived { get; set; }

    public string? SignatureMethodCode { get; set; } = null!;

    public DateTime? SignatureDateTime { get; set; }

    public int? SignatureConfirmedBy { get; set; }

    public string? CreatedUserName { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string Data { get; set; } = null!;

    public bool IsFinal { get; set; }
}
