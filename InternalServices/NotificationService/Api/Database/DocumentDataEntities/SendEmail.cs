namespace CIS.InternalServices.NotificationService.Api.Database.DocumentDataEntities;

public class SendEmail
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public LegacyContracts.Email.SendEmailRequest? Data { get; set; }
}
