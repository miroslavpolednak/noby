namespace ExternalServices.ESignatures.Dto;

public sealed class DispatchFormClientDocument
{
    public string ExternalId { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public bool AttachmentsComplete { get; set; }
    public string? NotCompletedReason { get; set; }
}
