using ProtoBuf.Grpc;
using Grpc.Core;
using CIS.Testing;
using CIS.Testing.xunit;
using CIS.Core.Types;

namespace CIS.InternalServices.Storage.Tests;

[TestCaseOrderer(PriorityOrderer.AttTypeName, PriorityOrderer.AttAssemblyName)]
public partial class BlobTempServiceTests : BaseBlobTest, IClassFixture<TestFixture<Api.Startup>>
{
    [Fact]
    public async Task InvalidAuthentication_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobTempSaveRequest { ApplicationKey = Constants.ApplicationKey };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, CallContext.Default));
        Assert.True(exception.StatusCode == StatusCode.Unauthenticated);
    }

    #region Save
    [Fact, TestPriority(0)]
    public async Task CreateBlobWithEmptyData_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobTempSaveRequest { ApplicationKey = Constants.ApplicationKey };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task CreateBlobWithEmptyData2_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobTempSaveRequest { ApplicationKey = Constants.ApplicationKey, BlobData = new Contracts.BlobFileStructure { Name = "test1.txt" } };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Save(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(10)]
    public async Task CreateBlob_ShouldCreateTempBlob()
    {
        // arrange
        var request = createSaveRequest("", Constants.ApplicationKey);

        // act
        var result = await svc().Save(request, createCallContext());
            
        // assert
        Assert.NotEmpty(result.BlobKey);
        Assert.Equal(1, (await getCurrentBlobCounts()).CountTemp);
    }

    [Fact, TestPriority(15)]
    public async Task CreateBlob_ShouldCreateTempSession()
    {
        // arrange
        var request = createSaveRequest(_sessionName, Constants.ApplicationKey);

        // act
        var result = await svc().Save(request, createCallContext());

        // assert
        Assert.NotEmpty(result.BlobKey);
        Assert.Equal(2, (await getCurrentBlobCounts()).CountTemp);
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
        // arrange
        var blobList = (await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Temp));
        var request = new Contracts.BlobGetRequest { BlobKey = blobList.First().BlobKey.ToString() };

        // act
        var result = await svc().Get(request, createCallContext());

        // assert
        Assert.True(result.Data.Length > 0);
    }
    #endregion Get

    #region move
    [Fact, TestPriority(0)]
    public async Task MoveSessionWithWrongId_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobMoveSessionFromTempRequest() { SessionId = "wrongsessionid" };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().MoveSession(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
        Assert.Equal(209, exception.Trailers.GetInt32(Core.Exceptions.ExceptionHandlingConstants.GrpcTrailerCisCodeKey).GetValueOrDefault());
    }

    [Fact, TestPriority(0)]
    public async Task MoveSessionIdIsNull_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobMoveSessionFromTempRequest() { SessionId = null };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().MoveSession(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(0)]
    public async Task MoveBlobWithWrongKey_ShouldFail()
    {
        // arrange
        var request = new Contracts.BlobMoveFromTempRequest() { BlobKey = new List<string>() { "wrongblobkey" } };

        // assert, act
        var exception = await Assert.ThrowsAsync<RpcException>(() => svc().Move(request, createCallContext()));
        Assert.True(exception.StatusCode == StatusCode.InvalidArgument);
    }

    [Fact, TestPriority(40)]
    public async Task MoveSession_ShouldMove()
    {
        // arrange
        var countBefore = await getCurrentBlobCounts();
        var request = new Contracts.BlobMoveSessionFromTempRequest() { SessionId = _sessionName };

        // act
        await svc().MoveSession(request, createCallContext());

        // assert
        var countAfter = await getCurrentBlobCounts();
        Assert.Equal(countBefore.CountTemp + countBefore.CountDefault, countAfter.CountTemp + countAfter.CountDefault);
        Assert.True(countBefore.CountTemp != countAfter.CountTemp);
        Assert.True(countBefore.CountDefault != countAfter.CountDefault);
    }

    [Fact, TestPriority(45)]
    public async Task MoveSingleBlob_ShouldMove()
    {
        // arrange
        var countBefore = await getCurrentBlobCounts();
        var key = (await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Temp)).First().BlobKey;
        var request = new Contracts.BlobMoveFromTempRequest() { BlobKey = new List<string>() { key.GetValueOrDefault().ToString() } };

        // act
        await svc().Move(request, createCallContext());

        // assert
        var countAfter = await getCurrentBlobCounts();
        Assert.Equal(countBefore.CountTemp + countBefore.CountDefault, countAfter.CountTemp + countAfter.CountDefault);
        Assert.True(countBefore.CountTemp != countAfter.CountTemp);
        Assert.True(countBefore.CountDefault != countAfter.CountDefault);
    }
    #endregion move
}

public partial class BlobTempServiceTests
{
    private Contracts.IBlobTempService svc()
        => _testFixture.CreateGrpcService<Contracts.IBlobTempService>();

    private const string _sessionName = "testsession";

    public BlobTempServiceTests(TestFixture<Api.Startup> testFixture) : base(testFixture) { }

    private static Contracts.BlobTempSaveRequest createSaveRequest(string sessionId, string? blobName = null)
        => new()
        {
            BlobData = createFile(blobName),
            SessionId = sessionId,
            ApplicationKey = Constants.ApplicationKey
        };
}
