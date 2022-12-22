namespace DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;

[DataContract]
public class SmsNotificationTypeItem
{
    [DataMember(Order = 1)]
    public string Code { get; set; }

    [DataMember(Order = 2)]
    public string Description { get; set; }

    [DataMember(Order = 3)]
    public string SmsText { get; set; }

    [DataMember(Order = 4)]
    public string McsCode { get; set; }

    [DataMember(Order = 5)]
    public bool IsAuditLogEnabled { get; set; }
}
