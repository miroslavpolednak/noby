using ProtoBuf.Grpc;
using Grpc.Core;
using CIS.Testing;
using CIS.Testing.xunit;
using CIS.Core.Types;

namespace CIS.InternalServices.Storage.Tests;

[TestCaseOrderer(PriorityOrderer.AttTypeName, PriorityOrderer.AttAssemblyName)]
public partial class BlobServiceTests : BaseBlobTest, IClassFixture<TestFixture<Api.Startup>>
{
    [Fact]
    public async Task InvalidAuthentication_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobSaveRequest { BlobData = createFile() };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, CallContext.Default));
        Assert.True(exception.StatusCode == StatusCode.Unauthenticated);
    }

    #region Save
    [Fact, TestPriority(0)]
    public async Task CreateBlobWithInvalidAppKey_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobSaveRequest { BlobData = createFile() };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task CreateBlobWithEmptyData_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobSaveRequest { ApplicationKey = Constants.ApplicationKey };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task CreateBlobWithEmptyData2_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobSaveRequest { ApplicationKey = Constants.ApplicationKey, BlobData = new Contracts.BlobFileStructure { Name = "test1.txt" } };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(10)]
    public async Task CreateBlob_ShouldCreateBlob()
    {
        // arrange
        var request = createSaveRequest();

        // act
        var result = await svc().Save(request, createCallContext());
            
        // assert
        Assert.NotEmpty(result.BlobKey);
        Assert.True(Guid.TryParse(result.BlobKey, out Guid g));
    }
    #endregion Save

    #region Get
    [Theory, TestPriority(0)]
    [InlineData("")]
    [InlineData("asdfqjbe894325")]
    public async Task GetBlobWithInvalidBlobKey_ShouldFail(string key)
    {
        // arrange
        var request = new Contracts.BlobGetRequest { BlobKey = key };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Get(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task GetNonExistingBlob_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobGetRequest { BlobKey = Guid.NewGuid().ToString() };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Get(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.NotFound);
    }

    [Fact, TestPriority(20)]
    public async Task GetBlob_ShouldReturnBlob()
    {
        var blobList = await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Default);

        // arrange
        var request = new Contracts.BlobGetRequest { BlobKey = blobList.First().BlobKey.ToString() };

        // act
        var result = await svc().Get(request, createCallContext());

        // assert
        Assert.True(result.Data.Length > 0);
    }
    #endregion Get

    #region Delete
    [Theory, TestPriority(0)]
    [InlineData("")]
    [InlineData("asdfqjbe894325")]
    public async Task DeleteBlobWithInvalidBlobKey_ShouldFail(string key)
    {
        // arrange
        var request = new Contracts.BlobDeleteRequest { BlobKey = key };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Delete(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task DeleteNonExistingBlob_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobDeleteRequest { BlobKey = Guid.NewGuid().ToString() };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Delete(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.NotFound);
    }

    [Fact, TestPriority(30)]
    public async Task DeleteBlob_Deleted()
    {
        var blobList = await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Default);

        // arrange
        var request = new Contracts.BlobDeleteRequest { BlobKey = blobList.First().BlobKey.ToString() };

        // act
        await svc().Delete(request, createCallContext());

        // assert
        Assert.True(true);
    }
    #endregion Delete
}

public partial class BlobServiceTests
{
    private Contracts.IBlobService svc()
        => _testFixture.CreateGrpcService<Contracts.IBlobService>();

    public BlobServiceTests(TestFixture<Api.Startup> testFixture) : base(testFixture) { }

    private static Contracts.BlobSaveRequest createSaveRequest(string? blobName = null)
        => new()
        {
            BlobData = createFile(blobName),
            ApplicationKey = Constants.ApplicationKey
        };
}
