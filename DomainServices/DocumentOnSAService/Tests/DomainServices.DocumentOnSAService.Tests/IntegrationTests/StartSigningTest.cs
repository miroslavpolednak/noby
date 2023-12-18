using CIS.Core.Exceptions;
using SharedTypes.Enums;
using SharedTypes.GrpcTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.Testing;
using DomainServices.CaseService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;
using ExternalServices.SbQueues.V1.Model;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using FastEnumUtility;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests;

public class StartSigningServiceRequestsTests : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";

    public StartSigningServiceRequestsTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        var sa = new SalesArrangement
        {
            SalesArrangementId = 1,
            State = 1,
            CaseId = 2,
            SalesArrangementTypeId = 6, // Service request
            Drawing = new SalesArrangementParametersDrawing()
            {
                Applicant = { new Identity()
                {
                    IdentityId = 1,
                    IdentityScheme = Identity.Types.IdentitySchemes.Kb,
                }}
            }
        };
       
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(sa);

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household> { });

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);

        List<Contact> contacts = new List<Contact>()
        {
            new()
            {
                Mobile = new MobilePhoneItem
                {
                    PhoneNumber = "725968744",
                    PhoneIDC = "+420"
                }
            },
            new()
            {
                Email = new()
                {
                    EmailAddress ="test@seznam.cz"
                }
            }
        };

        CustomerServiceClient.GetCustomerDetail(Arg.Any<Identity>(), Arg.Any<CancellationToken>()).Returns(new CustomerService.Contracts.CustomerDetailResponse
        {
            NaturalPerson = new()
            {
                FirstName = "Pavel",
                LastName = "Novak",
                BirthNumber = "8509242106"
            },

            Contacts = { contacts }
        });
    }


    [Fact]
    public async Task StartSigning_ServiceRequestWithHouseHold_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = HouseholdTypes.Codebtor.ToByte() // 2
            }
        });

        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = DocumentTypes.ZUSTAVSI.ToByte();
        var customerOnSaId = 4522;
        var customerOnSaId2 = 5555;
        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            SignatureTypeId = (int)SignatureTypes.Paper,
            CustomerOnSAId1 = customerOnSaId,
            CustomerOnSAId1SigningIdentity = CreateSigningIdentity(customerOnSaId: customerOnSaId),
            CustomerOnSAId2 = customerOnSaId2,
            CustomerOnSAId2SigningIdentity = CreateSigningIdentity(customerOnSaId: customerOnSaId2)
        });

        // response check
        response.Should().NotBeNull();
        response.DocumentOnSa.HouseholdId.Should().Be(1);
        response.DocumentOnSa.DocumentOnSAId.Should().Be(1);
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().Be("NT0000000000101");
        response.DocumentOnSa.IsValid.Should().BeTrue();
        response.DocumentOnSa.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.Include(doc => doc.SigningIdentities)
            .SingleOrDefault(r => r.SalesArrangementId == salesArrangementId);

        docOnSaEntity.Should().NotBeNull();
        docOnSaEntity!.HouseholdId.Should().Be(1);
        docOnSaEntity!.DocumentOnSAId.Should().Be(1);
        docOnSaEntity!.DocumentTypeId.Should().Be(documentTypeId);
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.FormId.Should().Be("NT0000000000101");
        docOnSaEntity!.IsValid.Should().BeTrue();
        docOnSaEntity.CustomerOnSAId1.Should().Be(customerOnSaId);
        docOnSaEntity!.CustomerOnSAId2.Should().Be(customerOnSaId2);
        docOnSaEntity.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);
        docOnSaEntity.SigningIdentities.Count.Should().Be(2);
    }
}

public class StartSigningServiceRequestsTestsPart2 : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";

    public StartSigningServiceRequestsTestsPart2(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        var sa = new SalesArrangement
        {
            SalesArrangementId = 1,
            State = 1,
            CaseId = 2,
            SalesArrangementTypeId = 6, // Service request
            Drawing = new SalesArrangementParametersDrawing()
            {
                Applicant = { new Identity()
                {
                    IdentityId = 1,
                    IdentityScheme = Identity.Types.IdentitySchemes.Kb,
                }}
            }
        };
       
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(sa);

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household> { });

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);

        List<Contact> contacts = new List<Contact>()
        {
            new()
            {
                Mobile = new MobilePhoneItem
                {
                    PhoneNumber = "725968744",
                    PhoneIDC = "+420"
                }
            },
            new()
            {
                Email = new()
                {
                    EmailAddress ="test@seznam.cz"
                }
            }
        };

        CustomerServiceClient.GetCustomerDetail(Arg.Any<Identity>(), Arg.Any<CancellationToken>()).Returns(new CustomerService.Contracts.CustomerDetailResponse
        {
            NaturalPerson = new()
            {
                FirstName = "Pavel",
                LastName = "Novak",
                BirthNumber = "8509242106"
            },

            Contacts = { contacts }
        });
    }

    [Fact]
    public async Task StartSigning_ServiceRequestWithoutHouseHold_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = DocumentTypes.ZADOCERP.ToByte(); // 6

        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            SignatureTypeId = (int)SignatureTypes.Paper,
        });

        // response check
        response.Should().NotBeNull();
        response.DocumentOnSa.HouseholdId.Should().BeNull();
        response.DocumentOnSa.DocumentOnSAId.Should().Be(1);
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().Be("NT0000000000101");
        response.DocumentOnSa.IsValid.Should().BeTrue();
        response.DocumentOnSa.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.Include(doc => doc.SigningIdentities)
            .SingleOrDefault(r => r.SalesArrangementId == salesArrangementId);

        docOnSaEntity.Should().NotBeNull();
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.IsValid.Should().BeTrue();
        response.DocumentOnSa.FormId.Should().Be("NT0000000000101");
    }
}

public class StartSigningProductsRequestsTests : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";
    private string workflowFormId = "N00000000000666";
    public StartSigningProductsRequestsTests(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, State = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            }
        });

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);


        var getTaskDetailResponse = new GetTaskDetailResponse()
        {
            TaskObject = new() { SignatureTypeId = SignatureTypes.Paper.ToByte() },
            TaskDetail = new()
            {
                Signing = new AmendmentSigning
                {
                    FormId = workflowFormId,
                    DocumentForSigning = eArchiveId,

                }
            }
        };

        CaseServiceClient.GetTaskDetail(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(getTaskDetailResponse);
    }



    [Fact]
    public async Task StartSigning_ProductRequest_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = 4;
        var customerOnSaId = 4522;
        var customerOnSaId2 = 5555;
        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            CustomerOnSAId1 = customerOnSaId,
            SignatureTypeId = (int)SignatureTypes.Paper,
            CustomerOnSAId1SigningIdentity = CreateSigningIdentity(customerOnSaId: customerOnSaId),
            CustomerOnSAId2 = customerOnSaId2,
            CustomerOnSAId2SigningIdentity = CreateSigningIdentity(customerOnSaId: customerOnSaId2)
        });

        // response check
        response.Should().NotBeNull();
        response.DocumentOnSa.HouseholdId.Should().Be(1);
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().NotBeNull();
        response.DocumentOnSa.IsValid.Should().BeTrue();
        response.DocumentOnSa.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.Include(doc => doc.SigningIdentities)
            .FirstOrDefault(r => r.SalesArrangementId == salesArrangementId);

        docOnSaEntity.Should().NotBeNull();
        docOnSaEntity!.HouseholdId.Should().Be(1);
        docOnSaEntity!.DocumentTypeId.Should().Be(documentTypeId);
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.FormId.Should().NotBeNull();
        docOnSaEntity!.IsValid.Should().BeTrue();
        docOnSaEntity.CustomerOnSAId1.Should().Be(customerOnSaId);
        docOnSaEntity!.CustomerOnSAId2.Should().Be(customerOnSaId2);
        docOnSaEntity.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);
        docOnSaEntity.SigningIdentities.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task StartSigning_PassInvalidParameters_ShouldReturnValidationExp_WithCorrectMessage()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var documentTypeId = 4;
        var signatureTypeId = 5;

        Func<Task> act = async () =>
        {
            var response = await client.StartSigningAsync(new()
            {
                DocumentTypeId = documentTypeId,
                SignatureTypeId = signatureTypeId
            });
        };

        await act.Should().ThrowAsync<CisValidationException>().WithMessage($"SalesArrangementId is required");
    }
}

public class StartSigningProductsRequestsTestsPart2 : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";
    private string workflowFormId = "N00000000000666";

    public StartSigningProductsRequestsTestsPart2(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, State = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            }
        });

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);


        var getTaskDetailResponse = new GetTaskDetailResponse()
        {
            TaskObject = new() { SignatureTypeId = SignatureTypes.Paper.ToByte() },
            TaskDetail = new()
            {
                Signing = new AmendmentSigning
                {
                    FormId = workflowFormId,
                    DocumentForSigning = eArchiveId,

                }
            }
        };

        CaseServiceClient.GetTaskDetail(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(getTaskDetailResponse);
    }

    [Fact]
    public async Task StartSigning_ProductRequestCrs_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var documentTypeId = DocumentTypes.DANRESID.ToByte(); // 13 Crs
        var customerOnSaId = 4522;

        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            DocumentTypeId = documentTypeId,
            CustomerOnSAId1 = customerOnSaId,
            SignatureTypeId = (int)SignatureTypes.Paper,
            CustomerOnSAId1SigningIdentity = CreateSigningIdentity(customerOnSaId: customerOnSaId),
        });

        response.Should().NotBeNull();
        response.DocumentOnSa.DocumentTypeId.Should().Be(documentTypeId);
        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().Be("NT0000000000101");
        response.DocumentOnSa.IsValid.Should().BeTrue();
        response.DocumentOnSa.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);

        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.Include(doc => doc.SigningIdentities)
            .FirstOrDefault(r => r.SalesArrangementId == salesArrangementId);

        docOnSaEntity.Should().NotBeNull();

        docOnSaEntity!.DocumentTypeId.Should().Be(documentTypeId);
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.FormId.Should().NotBeNull();
        docOnSaEntity!.IsValid.Should().BeTrue();
        docOnSaEntity.CustomerOnSAId1.Should().Be(customerOnSaId);
        docOnSaEntity.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);
        docOnSaEntity.SigningIdentities.Count.Should().BeGreaterThan(0);
    }
}

public class StartSigningProductsRequestsTestsPart3 : IntegrationTestBase
{
    private string eArchiveId = "KBHXXD00000000000000000000351";
    private string workflowFormId = "N00000000000666";

    public StartSigningProductsRequestsTestsPart3(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        //Mocks default
        ArrangementServiceClient.GetSalesArrangement(0, Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new SalesArrangement { SalesArrangementId = 1, State = 1, CaseId = 2, SalesArrangementTypeId = 1 });

        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);

        HouseholdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new List<Household>
        {
            new Household {
                HouseholdId = 1,
                HouseholdTypeId = 1,
            }
        });

        DocumentArchiveServiceClient.GenerateDocumentId(new()).ReturnsForAnyArgs(eArchiveId);


        var getTaskDetailResponse = new GetTaskDetailResponse()
        {
            TaskObject = new() { SignatureTypeId = SignatureTypes.Paper.ToByte() },
            TaskDetail = new()
            {
                Signing = new AmendmentSigning
                {
                    FormId = workflowFormId,
                    DocumentForSigning = eArchiveId,
                    DocumentForSigningType = "A"
                }
            }
        };

        CaseServiceClient.GetTaskDetail(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(getTaskDetailResponse);

        SbQueuesRepository.GetDocumentByExternalId(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(new Document
        {
            FileName = "test",
            ContentType = "application/json",
            DocumentId = 1,
            Content = [1, 2, 3],
            IsCustomerPreviewSendingAllowed = true
        });

        SbQueuesRepository.GetAttachmentById(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(new Attachment
        {
            FileName = "test",
            ContentType = "application/json",
            AttachmentId = 1,
            Content = [1, 2, 3],
            IsCustomerPreviewSendingAllowed = true
        });
    }

    [Fact]
    public async Task StartSigning_ProductRequestWorkflow_ShouldCreate_DocOnSa_ShouldReturnCorrectData()
    {
        //Grpc call
        var client = CreateGrpcClient();

        var salesArrangementId = 1;
        var caseId = 132456;
        var taskId = 654321;

        var response = await client.StartSigningAsync(new()
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            TaskId = taskId,
            TaskIdSb = taskId,
            SignatureTypeId = (int)SignatureTypes.Paper,
        });

        response.DocumentOnSa.EArchivId.Should().Be(eArchiveId);
        response.DocumentOnSa.FormId.Should().Be(workflowFormId);
        response.DocumentOnSa.IsValid.Should().BeTrue();
        response.DocumentOnSa.SignatureTypeId.Should().Be((int)SignatureTypes.Paper);


        using var scope = Fixture.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();

        var docOnSaEntity = dbContext.DocumentOnSa.Include(doc => doc.SigningIdentities)
            .SingleOrDefault(r => r.SalesArrangementId == salesArrangementId);

        docOnSaEntity.Should().NotBeNull();
        docOnSaEntity!.EArchivId.Should().Be(eArchiveId);
        docOnSaEntity!.FormId.Should().Be(workflowFormId);
        docOnSaEntity!.IsValid.Should().BeTrue();
        docOnSaEntity!.CaseId.Should().Be(caseId);
        docOnSaEntity!.TaskId.Should().Be(taskId);
        docOnSaEntity!.ExternalId.Should().Be(eArchiveId);
        docOnSaEntity.IsCustomerPreviewSendingAllowed.Should().BeTrue();
    }
}