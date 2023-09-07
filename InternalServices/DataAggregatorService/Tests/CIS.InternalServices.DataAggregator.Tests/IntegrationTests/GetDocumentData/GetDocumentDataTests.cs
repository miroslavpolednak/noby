using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetDocumentData;

public class GetDocumentDataTests : IntegrationTestBase
{
    public GetDocumentDataTests()
    {
        SalesArrangementServiceClient.MockGetSalesArrangement<SalesArrangementParametersMortgage>((sa, parameter) => sa.Mortgage = parameter);
        HouseholdServiceClient.MockHouseholdList(CustomerOnSAServiceClient);
        CustomerServiceClient.MockCustomerList();
        OfferServiceClient.MockGetOfferDetail();
    }

    [Fact]
    public async Task GetDocumentData_ValidDocumentType_ShouldReturnDocumentData()
    {
        ConfigurationManager.MockDocumentFields();

        var client = CreateGrpcClient();

        var response = await client.GetDocumentDataAsync(new GetDocumentDataRequest
        {
            DocumentTypeId = DocumentConstants.DocumentTypeId, 
            InputParameters = new InputParameters { SalesArrangementId = DocumentConstants.DefaultValidId }
        });

        response.DocumentTemplateVersionId.Should().Be(DocumentConstants.DocumentTemplateVersionId);
        response.DocumentTemplateVariantId.Should().Be(DocumentConstants.DocumentTemplateVariantId);
        response.InputParameters.OfferId.Should().NotBeNull();
        response.DocumentData.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetDocumentData_DocumentWithTable_ShouldReturnTable()
    {
        ConfigurationManager.MockTable();

        var client = CreateGrpcClient();

        var response = await client.GetDocumentDataAsync(new GetDocumentDataRequest
        {
            DocumentTypeId = DocumentConstants.DocumentTypeTableId,
            InputParameters = new InputParameters { SalesArrangementId = DocumentConstants.DefaultValidId }
        });

        response.DocumentData.Should().ContainSingle().And.Contain(x => x.ValueCase == DocumentFieldData.ValueOneofCase.Table);
        response.DocumentData.First().Table.Columns.Should().NotBeEmpty();
        response.DocumentData.First().Table.Rows.Should().NotBeEmpty();
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