// See https://aka.ms/new-console-template for more information

using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("run");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddDefaultAWSOptions(configuration.GetAWSOptions())
    .AddAWSService<IAmazonS3>()
    .BuildServiceProvider();
    
var s3Client = serviceProvider.GetRequiredService<IAmazonS3>();
var buckets = await s3Client.ListBucketsAsync();

Console.WriteLine("end");