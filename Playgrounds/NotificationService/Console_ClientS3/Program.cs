// See https://aka.ms/new-console-template for more information

using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Console_ClientS3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

Console.WriteLine("run");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddCommandLine(args)
    .Build();

var services = new ServiceCollection();
    
services
    .AddSingleton<IConfiguration>(configuration)
    .AddOptions<S3Configuration>()
    .Bind(configuration.GetSection(nameof(S3Configuration)));
    
var serviceProvider = services
    .AddScoped<IAmazonS3, AmazonS3Client>(provider =>
    {
        var options = provider.GetRequiredService<IOptions<S3Configuration>>();
        var config = new AmazonS3Config
        {
            ServiceURL = options.Value.ServiceURL,
            ForcePathStyle = true
        };

        var credentials = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
        return new AmazonS3Client(credentials, config);
    })
    .BuildServiceProvider();

var s3Client = serviceProvider.GetRequiredService<IAmazonS3>();
var response = await s3Client.ListBucketsAsync();

var bucketName = "b-s3-mpss-noby-nofication_service";
if (response.Buckets.All(b => b.BucketName != bucketName))
{
    Console.WriteLine($"Bucket '{bucketName}' does not exist");
    return;
}

var key = Guid.NewGuid().ToString();
var putRequest = new PutObjectRequest
{
    BucketName = bucketName,
    Key = key,
    ContentBody = "Content"
};

var putResponse = await s3Client.PutObjectAsync(putRequest);

var getRequest = new GetObjectRequest()
{
    BucketName = bucketName,
    Key = key
};

var getResponse = await s3Client.GetObjectAsync(getRequest);

using var memoryStream = new MemoryStream();

await getResponse.ResponseStream.CopyToAsync(memoryStream);

var content = Encoding.Default.GetString(memoryStream.ToArray());

Console.WriteLine("end");