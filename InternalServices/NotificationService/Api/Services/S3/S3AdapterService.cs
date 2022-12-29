﻿using Amazon.S3;
using Amazon.S3.Model;
using CIS.Core.Attributes;

namespace CIS.InternalServices.NotificationService.Api.Services.S3;

[ScopedService, SelfService]
public class S3AdapterService
{
    private readonly IAmazonS3 _s3Client;

    public S3AdapterService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<string> UploadFile(string content, string bucketName)
    {
        var key = Guid.NewGuid().ToString();
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentBody = content
        };

        var putResponse = await _s3Client.PutObjectAsync(putRequest);
        
        return key;
    }

    public async Task<byte[]> GetFile(string key, string bucketName)
    {
        var getRequest = new GetObjectRequest
        {
            Key = key,
            BucketName = bucketName
        };
        
        var response = await _s3Client.GetObjectAsync(getRequest);
        
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}