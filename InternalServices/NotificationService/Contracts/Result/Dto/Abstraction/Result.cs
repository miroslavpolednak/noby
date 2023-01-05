using Swashbuckle.AspNetCore.Annotations;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto.Abstraction;

[SwaggerSubType(typeof(EmailResult))]
[SwaggerSubType(typeof(SmsResult))]
public abstract class Result
{
}