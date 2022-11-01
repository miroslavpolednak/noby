using Amazon.Runtime;
using Amazon.S3;
using CIS.InternalServices.NotificationService.Api.Configuration;

namespace CIS.InternalServices.NotificationService.Api.S3;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddS3Client(this IServiceCollection services, S3Configuration s3Configuration)
    {
        return services
            .AddScoped<IAmazonS3, AmazonS3Client>(provider =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = s3Configuration.ServiceURL,
                    ForcePathStyle = true
                };

                var credentials = new BasicAWSCredentials(s3Configuration.AccessKey, s3Configuration.SecretKey);
                return new AmazonS3Client(credentials, config);
            });
    }
}