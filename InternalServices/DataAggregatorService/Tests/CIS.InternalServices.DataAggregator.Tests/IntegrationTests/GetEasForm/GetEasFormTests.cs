using CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common;
using CIS.InternalServices.DataAggregatorService.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetEasForm;

public class GetEasFormTests : IntegrationTestBase
{
    private readonly EasFormConfigurationBuilder _configurationBuilder;

    public GetEasFormTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        _configurationBuilder = new EasFormConfigurationBuilder(Fixture.Services.CreateScope().ServiceProvider);

        SalesArrangementServiceClient.MockGetSalesArrangement();
        HouseholdServiceClient.MockHouseholdList(CustomerOnSAServiceClient);
        CustomerServiceClient.MockCustomerList();
        OfferServiceClient.MockGetOfferDetail();
        DocumentOnSAServiceClient.MockDocumentOnSa();
    }

    [Fact]
    public async Task GetEasForm_ProductRequest_ShouldReturnTwoForms()
    {
        var client = CreateGrpcClient();
        await _configurationBuilder.MapFields().Build();

        var response = await client.GetEasFormAsync(new GetEasFormRequest
        {
            EasFormRequestType = EasFormRequestType.Product, 
            DynamicFormValues =
            {
                new DynamicFormValues { DocumentTypeId = 4, HouseholdId = 1 },
                new DynamicFormValues { DocumentTypeId = 5, HouseholdId = 2 }
            }
        });

        response.Forms.Should().HaveCount(2);
    }
}