using CIS.Testing;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

[CollectionDefinition("DbSequenceTests", DisableParallelization = true)]
public class GenerateFormIdWithoutParamTests : IntegrationTestBase
{
    public GenerateFormIdWithoutParamTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateFormIds_WithoutHousehold_ShouldReturnCorrectFormId()
    {
        var client = CreateGrpcClient();

        var formIdResponseFirst = await client.GenerateFormIdAsync(new());
        formIdResponseFirst.Should().NotBeNull();
        formIdResponseFirst.FormId.Should().NotBeEmpty()
            .And.HaveLength(15)
            .And.EndWith("101");

        // With final request
        var formIdResponseSecond = await client.GenerateFormIdAsync(new());
        formIdResponseSecond.Should().NotBeNull();
        formIdResponseSecond.FormId.Should().NotBeEmpty()
            .And.HaveLength(15)
            .And.EndWith("201");

        var formIdResponseFinal = await client.GenerateFormIdAsync(new() { IsFormIdFinal = true });
        formIdResponseFinal.Should().NotBeNull();
        formIdResponseFinal.FormId.Should().NotBeEmpty()
            .And.HaveLength(15)
            .And.EndWith("399"); ;

    }
}
