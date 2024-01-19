namespace CIS.InternalServices.NotificationService.Contracts;

public partial class SendSmsRequest
    : MediatR.IRequest<NotificationIdResponse>, CIS.Core.Validation.IValidatableRequest
{ }
