using CIS.InternalServices.Storage.Abstraction;
using CIS.Testing;
using CIS.Testing.xunit;
using CIS.Infrastructure.StartupExtensions;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.Storage.Tests;

[TestCaseOrderer(PriorityOrderer.AttTypeName, PriorityOrderer.AttAssemblyName)]
public partial class BlobServiceAbstractionTests : IClassFixture<TestFixture<Api.Startup>>
{
    #region Save
    [Fact, TestPriority(0)]
    public async Task CreateBlobWithInvalidAppKey_ShouldFail()
    {
        // assert, act
        var exception = await Assert.ThrowsAsync<CisArgumentException>(() => _service.Save(mockData(), applicationKey: "my_invalid_key"));
    }

    [Fact, TestPriority(0)]
    public async Task CreateBlobWithEmptyData_ShouldFail()
    {
        // assert, act
        var exception = await Assert.ThrowsAsync<Abstraction.Exceptions.BlobDataNullException>(() => _service.Save(new byte[0]));
    }

    [Fact, TestPriority(10)]
    public async Task CreateBlob_ShouldCreateBlob()
    {
        // act
        var result = await _service.Save(mockData(), "test.txt");

        // assert
        Assert.NotEmpty(result);
        Assert.True(Guid.TryParse(result, out Guid g));
    }
    #endregion Save

    #region Get
    [Theory, TestPriority(0)]
    [InlineData("")]
    [InlineData("asdfqjbe894325")]
    public async Task GetBlobWithInvalidBlobKey_ShouldFail(string key)
    {
        // assert, act
        var exception = await Assert.ThrowsAsync<Abstraction.Exceptions.InvalidBlobKeyException>(() => _service.Get(key));
    }

    [Fact, TestPriority(0)]
    public async Task GetNonExistingBlob_ShouldFail()
    {
        // assert, act
        var exception = await Assert.ThrowsAsync<Abstraction.Exceptions.BlobNotFoundException>(() => _service.Get(Guid.NewGuid().ToString()));
    }

    [Fact, TestPriority(20)]
    public async Task GetBlob_ShouldReturnBlob()
    {
        var blobList = await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Default);

        // act
        var result = await _service.Get(blobList?.First().BlobKey.ToString() ?? "");

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
        // assert, act
        var exception = await Assert.ThrowsAsync<Abstraction.Exceptions.InvalidBlobKeyException>(() => _service.Delete(key));
    }

    [Fact, TestPriority(0)]
    public async Task DeleteNonExistingBlob_ShouldFail()
    {
        // assert, act
        var exception = await Assert.ThrowsAsync<Abstraction.Exceptions.BlobNotFoundException>(() => _service.Delete(Guid.NewGuid().ToString()));
    }

    [Fact, TestPriority(30)]
    public async Task DeleteBlob_Deleted()
    {
        var blobList = await _repository.GetList(new ApplicationKey(Constants.ApplicationKey), Blob.BlobKinds.Default);

        // act
        await _service.Delete(blobList?.First().BlobKey.ToString() ?? "");

        // assert
        Assert.True(true);
    }
    #endregion Delete
}

public partial class BlobServiceAbstractionTests
{
    private readonly Blob.BlobRepository _repository;
    private readonly IBlobServiceAbstraction _service;

    private byte[] mockData()
        => System.Text.Encoding.UTF8.GetBytes("test");

    public BlobServiceAbstractionTests(TestFixture<Api.Startup> testFixture)
    {
        testFixture.Init(this)
            .ConfigureTestDatabase(options =>
            {
                options.SeedPaths = "~/BlobDatabaseSeed.sql";
            })
            .ConfigureTestServices(services =>
            {
                services.AddHttpContextAccessor();

                services
                    .AddCisEnvironmentConfiguration(opt =>
                    {
                        opt.DefaultApplicationKey = new(Constants.ApplicationKey);
                        opt.EnvironmentName = new(Constants.Environment);
                        opt.InternalServicesLogin = Constants.ServiceLogin;
                        opt.InternalServicePassword = Constants.ServicePassword;
                    })
                    .AddStorageTest((provider, options) =>
                    {
                        options.ChannelOptionsActions.Add(t => t.HttpHandler = null);
                        options.ChannelOptionsActions.Add(t => t.HttpClient = testFixture.GrpcClient);
                        options.Address = testFixture.GrpcClient.BaseAddress;
                    });
            });

        _service = testFixture.GetService<IBlobServiceAbstraction>() ?? throw new ArgumentNullException("_service");
        _repository = testFixture.GetService<Blob.BlobRepository>() ?? throw new ArgumentNullException("_repository");
    }
}
