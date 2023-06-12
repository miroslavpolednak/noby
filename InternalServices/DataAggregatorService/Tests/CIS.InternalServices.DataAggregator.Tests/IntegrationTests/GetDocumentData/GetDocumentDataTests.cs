using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common;
using CIS.InternalServices.DataAggregatorService.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetDocumentData;

public class GetDocumentDataTests : IntegrationTestBase
{
    private readonly DocumentConfigurationBuilder _configurationBuilder;

    public GetDocumentDataTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        _configurationBuilder = new DocumentConfigurationBuilder(Fixture.Services.CreateScope().ServiceProvider);

        SalesArrangementServiceClient.MockGetSalesArrangement();
        HouseholdServiceClient.MockHouseholdList(CustomerOnSAServiceClient);
        CustomerServiceClient.MockCustomerList();
        OfferServiceClient.MockGetOfferDetail();
    }

    [Fact]
    public async Task GetDocumentData_CorrectInput_ShouldReturnData()
    {
        var client = CreateGrpcClient();
        await _configurationBuilder.DataFields().DocumentFieldsMapping().Table().Build();

        var response = await client.GetDocumentDataAsync(new GetDocumentDataRequest
        {
            DocumentTypeId = DocumentConstants.DocumentTypeId, 
            InputParameters = new InputParameters { SalesArrangementId = DocumentConstants.DefaultValidId }
        });

        response.DocumentTemplateVersionId.Should().Be(DocumentConstants.DocumentTemplateVersionId);
        response.DocumentTemplateVariantId.Should().Be(DocumentConstants.DocumentTemplateVariantId);
        response.InputParameters.OfferId.Should().NotBeNull();
        response.DocumentData.Should().NotBeEmpty();
        response.DocumentData.Should().ContainSingle(x => x.ValueCase == DocumentFieldData.ValueOneofCase.Table);
    }

    [Fact]
    public async Task GetDocumentData_InvalidVariant_ShouldThrowValidation()
    {
        var client = CreateGrpcClient();

        var act = async () => await client.GetDocumentDataAsync(new GetDocumentDataRequest
        {
            DocumentTypeId = DocumentConstants.DocumentTypeId, 
            DocumentTemplateVersionId = DocumentConstants.DocumentTemplateVersionId,
            DocumentTemplateVariantId = DocumentConstants.InvalidId
        });

        await act.Should().ThrowAsync<CisValidationException>().WithMessage("The template variant ID * does not exist for the template version *");
    }

    [Fact]
    public async Task GetDocumentData_InvalidVersion_ShouldThrowValidation()
    {
        var client = CreateGrpcClient();

        var act = async () => await client.GetDocumentDataAsync(new GetDocumentDataRequest
        {
            DocumentTypeId = DocumentConstants.DocumentTypeId, 
            DocumentTemplateVersionId = DocumentConstants.InvalidId
        });

        await act.Should().ThrowAsync<CisValidationException>().WithMessage("Could not find a version for the document type with id * and requested version *");
    }
}