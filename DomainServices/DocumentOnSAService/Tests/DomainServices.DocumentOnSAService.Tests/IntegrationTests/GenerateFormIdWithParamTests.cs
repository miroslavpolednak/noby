using CIS.Testing;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

[CollectionDefinition("DbSequenceTests", DisableParallelization = true)]
public class GenerateFormIdWithParamTests : IntegrationTestBase
{
    public GenerateFormIdWithParamTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateFormIds_WithHousehold_ToMaxVersion_ShouldReturnCorrectFormId()
    {
        var client = CreateGrpcClient();

        for (int i = 1; i <= 100; i++)
        {
            var formIdResponse = await client.GenerateFormIdAsync(new() { HouseholdId = 1 });
            formIdResponse.Should().NotBeNull();
            if (i < 100)
            {
                formIdResponse.FormId.Should().NotBeEmpty()
                     .And.HaveLength(15)
                     .And.EndWith($"1{i:D2}");
            }
            else if (i == 100)
            {
                formIdResponse.FormId.Should().NotBeEmpty()
                     .And.HaveLength(15)
                     .And.EndWith($"201");
            }
        }

        var finalFormId = await client.GenerateFormIdAsync(new() { HouseholdId = 1, IsFormIdFinal = true });
        finalFormId.FormId.Should().NotBeEmpty()
                    .And.HaveLength(15)
                    .And.EndWith($"299");

        var afterfinalFormId = await client.GenerateFormIdAsync(new() { HouseholdId = 1});
        afterfinalFormId.FormId.Should().NotBeEmpty()
                    .And.HaveLength(15)
                    .And.EndWith($"301");
    }
}


