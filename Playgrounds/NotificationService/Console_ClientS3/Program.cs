// See https://aka.ms/new-console-template for more information

using System.Net;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("run");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .Build();

var awsOptions = configuration.GetAWSOptions("AWS");

var config = new AmazonS3Config()
{
    ServiceURL = "https://ecs-s3.sos.kb.cz:9021",
    ForcePathStyle = true,
    ProxyHost = "wproxy.kb.cz",
    ProxyPort = 8080,
    ProxyCredentials = CredentialCache.DefaultCredentials
};

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddDefaultAWSOptions(awsOptions)
    .AddAWSService<IAmazonS3>()
    .BuildServiceProvider();

var s3Client = new AmazonS3Client(new BasicAWSCredentials("AKIAC2FA54725D65F6FF", "J0EvmiuUN0iVQAtod/lEc9dGIsJNECBJvA5AeWwJ"), config);
// var s3Client = serviceProvider.GetRequiredService<IAmazonS3>();
var response = await s3Client.ListBucketsAsync();
var bucket = response.Buckets.FirstOrDefault(b => b.BucketName == "b-s3-mcs");

// var listRequest = new ListObjectsRequest
// {
//     BucketName = "b-s3-mcs"
// };
//
// var bucketObjects = await s3Client.ListObjectsAsync(listRequest);

var key = Guid.NewGuid().ToString();
var putRequest = new PutObjectRequest
{
    BucketName = "b-s3-mcs",
    Key = key,
    ContentBody = "Content"
};

var putResponse = await s3Client.PutObjectAsync(putRequest);

var getRequest = new GetObjectRequest()
{
    BucketName = "b-s3-mcs",
    Key = key
};

var getResponse = await s3Client.GetObjectAsync(getRequest);

Console.WriteLine("end");