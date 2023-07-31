using CIS.Foms.Enums;
using CIS.Foms.Types.Enums;
using CIS.Testing;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using static DomainServices.HouseholdService.Contracts.GetCustomerChangeMetadataResponse.Types;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class GetDocumentsToSignListTestsPart1 : IntegrationTestBase
{
    public GetDocumentsToSignListTestsPart1(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentToSignList_ProductRequest_WithCrsChange_WithoutExistCrs_ShouldReturnCorrectResult()
    {
        // Mocks
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
           .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        // Simulate virtual entity (household without DoOnSa) 
        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            },
            new Household {
                HouseholdId = 2,
                HouseholdTypeId = 2,
            }
        });

        CustomerOnSAServiceClient.GetCustomerChangeMetadata(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<GetCustomerChangeMetadataResponseItem>
        {
            new()
            {
                CustomerOnSAId = 5544,
                CustomerChangeMetadata = new CustomerChangeMetadata {WasCRSChanged = true}
            }
        });

        ESignaturesClient.GetDocumentStatus(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(EDocumentStatuses.NEW);

        var docOnSaEntity = CreateDocOnSaEntity(householdId: 1);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        // signatureTypeId: 3 => Electronic 
        dbContext.DocumentOnSa.AddRange(docOnSaEntity, CreateDocOnSaEntity(householdId: 2, signatureTypeId: 3));
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = docOnSaEntity.SalesArrangementId });
        response.DocumentsOnSAToSign.Should().HaveCount(3);

        // One Virtual Crs (without DocOnSa)
        response.DocumentsOnSAToSign.Any(r => r.DocumentTypeId == DocumentTypes.DANRESID.ToByte() && string.IsNullOrWhiteSpace(r.FormId)).Should().BeTrue();
        // Two real product request
        response.DocumentsOnSAToSign.Where(r => !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(2);
    }
}

public class GetDocumentsToSignListTestsPart2 : IntegrationTestBase
{
    public const int _customerOnSaId = 5544;
    public const int _salesArrangementId = 2;

    public GetDocumentsToSignListTestsPart2(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {

        CustomerOnSAServiceClient.GetCustomerChangeMetadata(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<GetCustomerChangeMetadataResponseItem>
        {
            new()
            {
                CustomerOnSAId = _customerOnSaId,
                CustomerChangeMetadata = new CustomerChangeMetadata {WasCRSChanged = true}
            }
        });

        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
             //SalesArrangementTypeId = 10 -> DocumentTypeId = 12
             .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = _salesArrangementId, CaseId = 2, SalesArrangementTypeId = 10 });
    }

    [Fact]
    public async Task GetDocumentToSignList_ServiceRequest_WithExistDocOnSaServiceRequest_WithCrsChange_WithExistDocOnSaCrs_ShouldReturnCorrectResult()
    {
        // Real productRequest
        var docOnSaEntityServiceRequest = CreateDocOnSaEntity(salesArrangementId: _salesArrangementId, householdId: 1, documentTypeId: DocumentTypes.PRISTOUP.ToByte());

        // Real Crs
        var docOnSaEntityCrs = CreateDocOnSaEntity(salesArrangementId: _salesArrangementId, householdId: null, documentTypeId: DocumentTypes.DANRESID.ToByte(), customerOnSAId1: _customerOnSaId);

        // Save to db
        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        dbContext.DocumentOnSa.AddRange(docOnSaEntityServiceRequest, docOnSaEntityCrs);
        await dbContext.SaveChangesAsync();

        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = 2 });
        // One real service request 12
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.PRISTOUP.ToByte() && !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
        // One real Crs request 13
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.DANRESID.ToByte() && !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);

    }


}

public class GetDocumentsToSignListTestsPart3 : IntegrationTestBase
{
    public GetDocumentsToSignListTestsPart3(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetDocumentToSignList_ProductRequest_OneVirtual_OneReal_WithCrsChange_WithExistCrs_ShouldReturnCorrectResult()
    {
        // Mocks
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
           .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        // Simulate virtual entity (household without DoOnSa) 
        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            },
            new Household {
                HouseholdId = 2,
                HouseholdTypeId = 2
            }
        });
        int customerOnSaId = 5544;
        CustomerOnSAServiceClient.GetCustomerChangeMetadata(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<GetCustomerChangeMetadataResponseItem>
        {
            new()
            {
                CustomerOnSAId = customerOnSaId,
                CustomerChangeMetadata = new CustomerChangeMetadata {WasCRSChanged = true}
            }
        });

        ESignaturesClient.GetDocumentStatus(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(EDocumentStatuses.NEW);

        // Real productRequest
        var docOnSaEntityProductRequest = CreateDocOnSaEntity(householdId: 1, documentTypeId: DocumentTypes.ZADOSTHU.ToByte());

        // Real Crs
        var docOnSaEntityCrs = CreateDocOnSaEntity(householdId: null, documentTypeId: DocumentTypes.DANRESID.ToByte(), customerOnSAId1: customerOnSaId);

        // Save to db
        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        dbContext.DocumentOnSa.AddRange(docOnSaEntityProductRequest, docOnSaEntityCrs);
        await dbContext.SaveChangesAsync();

        // Call service
        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = 1 });
        // One real and one virtual product request and one real crs request
        response.DocumentsOnSAToSign.Should().HaveCount(3);

        // one real product request 
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte() && !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
        // one virtual product request
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.ZADOSTHD.ToByte() && string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
        // one real crs request
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.DANRESID.ToByte() && !string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
    }
}

public class GetDocumentsToSignListTestsPart4 : IntegrationTestBase
{
    public const int _customerOnSaId = 5544;
    public const int _salesArrangementId = 2;

    public GetDocumentsToSignListTestsPart4(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        CustomerOnSAServiceClient.GetCustomerChangeMetadata(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<GetCustomerChangeMetadataResponseItem>
        {
            new()
            {
                CustomerOnSAId = _customerOnSaId,
                CustomerChangeMetadata = new CustomerChangeMetadata {WasCRSChanged = true}
            }
        });

        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>())
             //SalesArrangementTypeId = 10 -> DocumentTypeId = 12
             .ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = _salesArrangementId, CaseId = 2, SalesArrangementTypeId = 10 });
    }

    [Fact]
    public async Task GetDocumentToSignList_ServiceRequest_WithoutExistDocOnSaServiceRequest_WithCrsChange_WithoutExistDocOnSaCrs_ShouldReturnCorrectResult()
    {
        var client = CreateGrpcClient();
        var response = await client.GetDocumentsToSignListAsync(new() { SalesArrangementId = 2 });
        // One virtual service request 12
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.PRISTOUP.ToByte() && string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
        // One virtual Crs request 13
        response.DocumentsOnSAToSign.Where(r => r.DocumentTypeId == DocumentTypes.DANRESID.ToByte() && string.IsNullOrWhiteSpace(r.FormId)).Should().HaveCount(1);
    }
}