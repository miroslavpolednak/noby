using CIS.Core.Exceptions;
using CIS.Core.Security;
using DomainServices.CaseService.Api;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using MediatR;
using Moq;
using SharedAudit;
using SharedTypes.Enums;
using System.Globalization;

namespace DomainServices.CaseService.Tests.EndpointsTests;

public class CancelCaseHandlerTests
{
    private readonly Mock<IAuditLogger> _auditLogger;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ICurrentUserAccessor> _currentUser;
    private readonly Mock<RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient> _riskBusinessCaseService;
    private readonly Mock<HouseholdService.Clients.v1.ICustomerOnSAServiceClient> _customerOnSAService;
    private readonly Mock<HouseholdService.Clients.v1.IHouseholdServiceClient> _householdService;
    private readonly Mock<DocumentOnSAService.Clients.IDocumentOnSAServiceClient> _documentOnSAService;
    private readonly Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient> _salesArrangementService;
    private readonly Mock<ProductService.Clients.IProductServiceClient> _productService;

    public CancelCaseHandlerTests()
    {
        ErrorCodeMapper.Init();

        _auditLogger = new Mock<IAuditLogger>();
        _mediator = new Mock<IMediator>();
        _currentUser = new Mock<ICurrentUserAccessor>();
        _riskBusinessCaseService = new Mock<RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient>();
        _customerOnSAService = new Mock<HouseholdService.Clients.v1.ICustomerOnSAServiceClient>();
        _householdService = new Mock<HouseholdService.Clients.v1.IHouseholdServiceClient>();
        _documentOnSAService = new Mock<DocumentOnSAService.Clients.IDocumentOnSAServiceClient>();
        _salesArrangementService = new Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient>();
        _productService = new Mock<ProductService.Clients.IProductServiceClient>();
    }

    [Fact]
    public async Task Handle_Should_ResultToBeCancelledWithFullFlow()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 1
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.CaseState.Should().Be((int)EnumCaseStates.ToBeCancelled);

        // should call SendToCmp
        _salesArrangementService
            .Verify(v =>
                v.SendToCmp(It.Is<int>(x => x == MockDataSetupHelper.SalesArrangementIdCase1), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
                Times.Once);

        // call mediatr
        _mediator
            .Verify(v =>
                v.Send(It.Is<UpdateCaseStateRequest>(x => x.CaseId == request.CaseId && x.State == (int)EnumCaseStates.ToBeCancelled), It.IsAny<CancellationToken>()),
                Times.Once);

        // delete SA
        _salesArrangementService
            .Verify(v =>
                v.DeleteSalesArrangement(It.Is<int>(x => x == MockDataSetupHelper.SalesArrangementIdCase1), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
                Times.Once);

        // audit log
        _auditLogger
            .Verify(v =>
                v.Log(It.Is<AuditEventTypes>(x => x == AuditEventTypes.Noby004), It.IsAny<string>(), It.IsAny<ICollection<AuditLoggerHeaderItem>?>(), It.IsAny<ICollection<AuditLoggerHeaderItem>?>(), It.IsAny<AuditLoggerHeaderItem?>(), It.IsAny<string?>(), It.IsAny<IDictionary<string, string>?>(), It.IsAny<IDictionary<string, string>?>()),
                Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ResultCancelled_DebtorNotIdentified()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 3
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.CaseState.Should().Be((int)EnumCaseStates.Cancelled);

        // should NOT call CancelMortgage
        _productService
            .Verify(v => v.CancelMortgage(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ResultCancelled_DebtorIdentifiedWithoutRip()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 4
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.CaseState.Should().Be((int)EnumCaseStates.Cancelled);

        // should call CancelMortgage
        _productService
            .Verify(v => v.CancelMortgage(It.Is<long>(x => x == request.CaseId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ResultCancelled_DebtorIdentifiedWithRip()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 5
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.CaseState.Should().Be((int)EnumCaseStates.Cancelled);

        // should call RIP
        _riskBusinessCaseService
            .Verify(v => v.CommitCase(It.Is<RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseCommitCaseRequest>(x => x.SalesArrangementId == MockDataSetupHelper.SalesArrangementIdCase5), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallStopSigning()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 1
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        _documentOnSAService
            .Verify(v =>
                v.StopSigning(
                    It.Is<DocumentOnSAService.Contracts.StopSigningRequest>(t => t.DocumentOnSAId == 403),
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFound()
    {
        var request = new CancelCaseRequest
        {
            CaseId = -1
        };

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        await Assert.ThrowsAsync<CisNotFoundException>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_Should_Throw_DueToInvalidState()
    {
        var request = new CancelCaseRequest
        {
            CaseId = 2
        };

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var exception = await Assert.ThrowsAsync<CisValidationException>(() => handler.Handle(request, default));

        exception.Should().Match<CisValidationException>(e => e.FirstExceptionCode == Api.ErrorCodeMapper.UnableToCancelCase.ToString(CultureInfo.InvariantCulture));
    }

    private void setupExternalData()
    {
        MockDataSetupHelper.SetupSalesArrangementService(_salesArrangementService);

        _documentOnSAService
            .Setup(s => s.GetDocumentsOnSAList(It.Is<int>(x => x != MockDataSetupHelper.SalesArrangementIdCase1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new DocumentOnSAService.Contracts.GetDocumentsOnSAListResponse()));

        _documentOnSAService
            .Setup(s => s.GetDocumentsOnSAList(It.Is<int>(x => x == MockDataSetupHelper.SalesArrangementIdCase1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new DocumentOnSAService.Contracts.GetDocumentsOnSAListResponse
            {
                DocumentsOnSA = 
                {
                    new DocumentOnSAService.Contracts.DocumentOnSAToSign() { DocumentOnSAId = 400, IsSigned = true, HouseholdId = _mainHouseholdId, SignatureTypeId = 3 },
                    new DocumentOnSAService.Contracts.DocumentOnSAToSign() { DocumentOnSAId = 401, IsSigned = false, HouseholdId = 501, SignatureTypeId = 3 }
                }
            }));
        
        _documentOnSAService
            .Setup(s => s.GetDocumentsToSignList(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new DocumentOnSAService.Contracts.GetDocumentsToSignListResponse
                {
                    DocumentsOnSAToSign =
                    {
                        new DocumentOnSAService.Contracts.DocumentOnSAToSign { DocumentOnSAId = 402, IsSigned = true },
                        new DocumentOnSAService.Contracts.DocumentOnSAToSign { DocumentOnSAId = 403, IsSigned = false }
                    }
                })
            );
        
        _householdService
            .Setup(s => s.GetHouseholdList(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<HouseholdService.Contracts.Household>
            {
                new() { HouseholdId = _mainHouseholdId, HouseholdTypeId = 1 },
                new() { HouseholdId = 501, HouseholdTypeId = 2 }
            }));

        _customerOnSAService
            .Setup(s => s.GetCustomerList(It.Is<int>(x => x == MockDataSetupHelper.SalesArrangementIdCase3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<HouseholdService.Contracts.CustomerOnSA>()));

        _customerOnSAService
            .Setup(s => s.GetCustomerList(It.Is<int>(x => x != MockDataSetupHelper.SalesArrangementIdCase3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<HouseholdService.Contracts.CustomerOnSA>
            {
                new() { CustomerOnSAId = 101, CustomerRoleId = 1, CustomerIdentifiers = { new SharedTypes.GrpcTypes.Identity { IdentityId = 123456, IdentityScheme = SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb } } },
                new() { CustomerOnSAId = 102, CustomerRoleId = 2 }
            }));
    }

    const int _mainHouseholdId = 500;

    private Api.Endpoints.v1.CancelCase.CancelCaseHandler createHandler(CaseServiceDbContext dbContext)
    {
        return new Api.Endpoints.v1.CancelCase.CancelCaseHandler(
            _auditLogger.Object, 
            _mediator.Object, 
            _currentUser.Object, 
            _riskBusinessCaseService.Object, 
            _customerOnSAService.Object, 
            _householdService.Object, 
            _documentOnSAService.Object, 
            _salesArrangementService.Object, 
            _productService.Object, 
            dbContext);
    }
}
