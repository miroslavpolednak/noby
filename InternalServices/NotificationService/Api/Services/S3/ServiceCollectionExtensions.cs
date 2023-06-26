using Amazon.Runtime;
using Amazon.S3;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.S3;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddS3Client(this WebApplicationBuilder builder)
    {
        var s3Configuration = builder.GetS3Configuration();
        
        builder.Services
            .AddScoped<IAmazonS3, AmazonS3Client>(_ =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = s3Configuration.ServiceUrl,
                    ForcePathStyle = true
                };

                var credentials = new BasicAWSCredentials(s3Configuration.AccessKey, s3Configuration.SecretKey);
                return new AmazonS3Client(credentials, config);
            })
            .AddScoped<IS3AdapterService, S3AdapterService>();

        return builder;
    }
}