namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class S3Configuration
{
    public string ServiceUrl { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}