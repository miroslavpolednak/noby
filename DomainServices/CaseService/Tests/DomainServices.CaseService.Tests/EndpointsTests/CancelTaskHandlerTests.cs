using CIS.Core.Exceptions;
using CIS.Core.Security;
using DomainServices.CaseService.Api;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using ExternalServices.SbWebApi.V1;
using MediatR;
using Moq;
using NSubstitute.ReturnsExtensions;

namespace DomainServices.CaseService.Tests.EndpointsTests;

public class CancelTaskHandlerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ISbWebApiClient> _sbWebApiClient;
    private readonly Mock<ISalesArrangementServiceClient> _salesArrangementService;
    private readonly Mock<ICodebookServiceClient> _codebookService;
    private readonly Mock<ICurrentUserAccessor> _currentUser;

    public CancelTaskHandlerTests()
    {
        ErrorCodeMapper.Init();

        _currentUser = new Mock<ICurrentUserAccessor>();
        _mediator = new Mock<IMediator>();
        _sbWebApiClient = new Mock<ISbWebApiClient>();
        _salesArrangementService = new Mock<ISalesArrangementServiceClient>();
        _codebookService = new Mock<ICodebookServiceClient>();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFound()
    {
        var request = new CancelTaskRequest
        {
            CaseId = -1,
            TaskIdSB = -1
        };

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        await Assert.ThrowsAsync<CisNotFoundException>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyAndDoNotSetFlowSwitches()
    {
        var request = new CancelTaskRequest
        {
            CaseId = 1,
            TaskIdSB = 1
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.Should().BeOfType<Google.Protobuf.WellKnownTypes.Empty>();

        _mediator
            .Verify(v =>
                v.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == request.TaskIdSB), It.IsAny<CancellationToken>()),
                Times.Once);

        _sbWebApiClient
            .Verify(v =>
                v.CancelTask(It.Is<int>(x => x == request.TaskIdSB), It.IsAny<CancellationToken>()),
                Times.Once);

        _salesArrangementService
            .Verify(v =>
                v.SetFlowSwitches(It.IsAny<int>(), It.IsAny<List<SalesArrangementService.Contracts.EditableFlowSwitch>>(), It.IsAny<CancellationToken>()),
                Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyAndSetFlowSwitches()
    {
        var request = new CancelTaskRequest
        {
            CaseId = 1,
            TaskIdSB = 2
        };
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.Should().BeOfType<Google.Protobuf.WellKnownTypes.Empty>();

        _salesArrangementService
            .Verify(v =>
                v.SetFlowSwitches(
                    It.IsAny<int>(), 
                    It.Is<List<SalesArrangementService.Contracts.EditableFlowSwitch>>(
                        x => x.Count == 3 
                        && x.Any(z => z.FlowSwitchId == 8 && !z.Value.GetValueOrDefault())
                        && x.Any(z => z.FlowSwitchId == 9 && !z.Value.GetValueOrDefault())
                        && x.Any(z => z.FlowSwitchId == 10 && !z.Value.GetValueOrDefault())), 
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }

    private void setupExternalData()
    {
        MockDataSetupHelper.SetupSalesArrangementService(_salesArrangementService);

        _mediator
            .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new GetTaskDetailResponse
            {
                TaskDetail = new(),
                TaskObject = new()
                {
                    TaskTypeId = 7
                }
            }));

        _mediator
            .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 2), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new GetTaskDetailResponse
            {
                TaskDetail = new(),
                TaskObject = new()
                {
                    TaskTypeId = 2
                }
            }));

        _sbWebApiClient
            .Setup(s => s.CancelTask(It.IsAny<int>(), It.IsAny<CancellationToken>()));

        _codebookService
            .Setup(s => s.ProductTypes(It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<CodebookService.Contracts.v1.ProductTypesResponse.Types.ProductTypeItem>
            {
                new() { Id = 20001, MandantId = 2 },
                new() { Id = 20002, MandantId = 1 }
            }));
    }

    private Api.Endpoints.v1.CancelTask.CancelTaskHandler createHandler(CaseServiceDbContext dbContext)
    {
        return new Api.Endpoints.v1.CancelTask.CancelTaskHandler(
            _mediator.Object,
            _sbWebApiClient.Object,
            _salesArrangementService.Object,
            _codebookService.Object,
            dbContext);
    }
}
