namespace CIS.InternalServices.NotificationService.Api.Database.DocumentDataEntities;

internal sealed class SmsData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public string Type { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public string NationalNumber { get; set; } = string.Empty;

    public int? ProcessingPriority { get; set; }
}
