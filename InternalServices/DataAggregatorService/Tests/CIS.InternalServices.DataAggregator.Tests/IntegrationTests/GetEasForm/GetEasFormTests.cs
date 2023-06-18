using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetEasForm;

public class GetEasFormTests : IntegrationTestBase
{
    private readonly EasFormConfigurationBuilder _configurationBuilder;

    public GetEasFormTests()
    {
        _configurationBuilder = new EasFormConfigurationBuilder(Fixture.Services.CreateScope().ServiceProvider);

        HouseholdServiceClient.MockHouseholdList(CustomerOnSAServiceClient);
        CustomerServiceClient.MockCustomerList();
        DocumentOnSAServiceClient.MockDocumentOnSa();
        CaseServiceClient.MockGetCaseDetail();
    }

    [Fact]
    public async Task GetEasForm_ProductRequest_ShouldReturnTwoForms()
    {
        SalesArrangementServiceClient.MockGetSalesArrangement<SalesArrangementParametersMortgage>((sa, parameter) => sa.Mortgage = parameter);
        OfferServiceClient.MockGetOfferDetail();

        _configurationBuilder.DataFields().ProductRequest().Commit();

        var client = CreateGrpcClient();

        var response = await client.GetEasFormAsync(new GetEasFormRequest
        {
            EasFormRequestType = EasFormRequestType.Product, 
            DynamicFormValues =
            {
                new DynamicFormValues { DocumentTypeId = DocumentTypes.ZADOSTHU.ToByte(), HouseholdId = DefaultMockValues.HouseholdMainId },
                new DynamicFormValues { DocumentTypeId = DocumentTypes.ZADOSTHD.ToByte(), HouseholdId = DefaultMockValues.HouseholdCodebtorId }
            }
        });

        response.Forms.Should().HaveCount(2);
        response.Forms.Should().NotContainNulls(f => f.Json);
    }

    [Fact]
    public async Task GetEasForm_ServiceRequest_ShouldReturnOneForm()
    {
        SalesArrangementServiceClient.MockGetSalesArrangement<SalesArrangementParametersDrawing>((sa, parameter) => sa.Drawing = parameter);
        _configurationBuilder.DataFields().ServiceRequest().Commit();

        var client = CreateGrpcClient();

        var response = await client.GetEasFormAsync(new GetEasFormRequest
        {
            EasFormRequestType = EasFormRequestType.Service,
            DynamicFormValues = { new DynamicFormValues { DocumentTypeId = DocumentTypes.ZADOCERP.ToByte() } }
        });

        response.Forms.Should().ContainSingle();
        response.Forms.First().Json.Should().NotBeNullOrWhiteSpace();
    }
}