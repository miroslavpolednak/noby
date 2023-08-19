using CIS.Core.Data;
using DomainServices.DocumentOnSAService.Api.Database.Enums;

namespace DomainServices.DocumentOnSAService.Api.Database.Entities;

public class DocumentOnSa : ICreated
{
    public int DocumentOnSAId { get; set; }

    public int? DocumentTypeId { get; set; }

    public int? DocumentTemplateVersionId { get; set; }

    public int? DocumentTemplateVariantId { get; set; }

    public string FormId { get; set; } = null!;

    public string? EArchivId { get; set; }

    public string? DmsxId { get; set; }

    public int SalesArrangementId { get; set; }

    public int? HouseholdId { get; set; }

    public bool IsValid { get; set; }

    public bool IsSigned { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? SignatureDateTime { get; set; }

    public int? SignatureConfirmedBy { get; set; }

    public string? CreatedUserName { get; set; }

    public int? CreatedUserId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string? Data { get; set; } = null!;

    public bool IsFinal { get; set; }

    public string? ExternalId { get; set; }

    public Source Source { get; set; }

    public int? SignatureTypeId { get; set; }

    public int? CustomerOnSAId1 { get; set; }

    public int? CustomerOnSAId2 { get; set; }

    public long? CaseId { get; set; }

    public int? TaskId { get; set; }

    public bool IsPreviewSentToCustomer { get; set; }

    public ICollection<EArchivIdsLinked> EArchivIdsLinkeds { get; } = new List<EArchivIdsLinked>();
    
    public ICollection<SigningIdentity> SigningIdentities { get; } = new List<SigningIdentity>();
}
