using CIS.Core.Types;
using CIS.Testing;
using Grpc.Core;
using ProtoBuf.Grpc;

namespace CIS.InternalServices.Storage.Tests;

public abstract class BaseBlobTest
{
    protected readonly TestFixture<Api.Startup> _testFixture;
    internal readonly Blob.BlobRepository _repository;

    public BaseBlobTest(TestFixture<Api.Startup> testFixture)
    {
        _testFixture = testFixture
            .Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/BlobDatabaseSeed.sql";
            });
        _repository = _testFixture.GetService<Blob.BlobRepository>() ?? throw new ArgumentException("_repository");
    }

    protected async Task<(int CountTemp, int CountDefault)> getCurrentBlobCounts()
    {
        var blobList = await _repository.GetList(new ApplicationKey(Constants.ApplicationKey));
        int countTemp = blobList.Count(t => t.Kind == Blob.BlobKinds.Temp);
        int countDefault = blobList.Count(t => t.Kind == Blob.BlobKinds.Default);

        return (countTemp, countDefault);
    }

    protected static Contracts.BlobFileStructure createFile(string? blobName = null)
        => new()
        {
            ContentType = "text/plain",
            Name = blobName ?? System.Guid.NewGuid().ToString() + ".txt",
            Data = System.Text.Encoding.UTF8.GetBytes("test")
        };

    protected static CallContext createCallContext()
    {
        var headers = new Metadata();

        // authentication
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{Constants.ServiceLogin}:{Constants.ServicePassword}");
        headers.Add("Authorization", "Basic " + Convert.ToBase64String(plainTextBytes));

        var options = new CallOptions(headers: headers);

        return new CallContext(options);
    }
}
