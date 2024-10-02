using CIS.Core.Security;
using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Api.Messaging.MessageHandlers;
using DomainServices.CaseService.Api.Services;
using DomainServices.SalesArrangementService.Clients;
using KafkaFlow;
using MediatR;
using Moq;
using Microsoft.Extensions.Logging.Testing;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.CaseService.Tests.Messaging;

public class IndividualPricingProcessChangedHandlerTests
{
    private readonly IMessageContext _messageContext;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ISalesArrangementServiceClient> _salesArrangementService;
    private readonly Mock<IActiveTasksService> _activeTasksService;
    private readonly Mock<ICurrentUserAccessor> _currentUser;
    private readonly FakeLogger<IndividualPricingProcessChangedHandler> _logger;

    public IndividualPricingProcessChangedHandlerTests()
    {
        _messageContext = new MockMessageContext();
        _mediator = setupMediatr();
        _currentUser = new Mock<ICurrentUserAccessor>();
        _activeTasksService = new Mock<IActiveTasksService>();
        _salesArrangementService = MockDataSetupHelper.SetupSalesArrangementService();
        _logger = new FakeLogger<IndividualPricingProcessChangedHandler>();
        
    }

    [Fact]
    public async Task Handle_WrongStatus_ShouldQuit()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(ProcessStateEnum.SUSPENDED);

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaHandlerSkippedDueToState));
        _logger.Collector.LatestRecord.StructuredState.Should().Contain(t => t.Key == "State" && t.Value == ProcessStateEnum.SUSPENDED.ToString());
    }

    [Fact]
    public async Task Handle_CaseNotFound_ShouldQuit()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(ProcessStateEnum.ACTIVE, 95);

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaCaseIdNotFound));
    }

    [Fact]
    public async Task Handle_Step1_ShouldQuit()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(caseId: 2, taskIdSB: 56781);

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaIndividualPricingProcessChangedSkipped));
        _logger.Collector.LatestRecord.StructuredState.Should().Contain(t => t.Key == "Step" && t.Value == "1");
    }

    [Fact]
    public async Task Handle_Step3_ShouldQuit()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(processId: 9999);

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaIndividualPricingProcessChangedSkipped));
        _logger.Collector.LatestRecord.StructuredState.Should().Contain(t => t.Key == "Step" && t.Value == "3");
    }

    [Fact]
    public async Task Handle_Step4_ShouldQuit()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(caseId: 3);

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaIndividualPricingProcessChangedSkipped));
        _logger.Collector.LatestRecord.StructuredState.Should().Contain(t => t.Key == "Step" && t.Value == "4");
    }

    [Fact]
    public async Task Handle_TerminatedProcessTypeId1_ShouldSetFlowSwitches()
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(state: ProcessStateEnum.TERMINATED, taskIdSB: 56781);

        await handle(dbContext, message);

        _salesArrangementService.Verify(x => x.SetFlowSwitches(It.IsAny<int>(), It.Is<List<EditableFlowSwitch>>(z => 
            z.Count(y => y.FlowSwitchId == 8 && !y.Value.GetValueOrDefault()) == 1
            && z.Count(y => y.FlowSwitchId == 9 && !y.Value.GetValueOrDefault()) == 1
            && z.Count(y => y.FlowSwitchId == 10 && !y.Value.GetValueOrDefault()) == 1), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Theory]
    [InlineData(3, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    public async Task Handle_Step6_ShouldQuit(int decisionId, int phaseTypeId)
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(taskIdSB: 567811);

        _mediator
           .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 567811), It.IsAny<CancellationToken>()))
           .Returns(() => Task.FromResult(new GetTaskDetailResponse
           {
               TaskObject = new()
               {
                   DecisionId = decisionId,
                   PhaseTypeId = phaseTypeId,
                   ProcessTypeId = 2
               }
           }));

        await handle(dbContext, message);

        _logger.Collector.LatestRecord.Should().NotBeNull();
        _logger.Collector.LatestRecord.Id.Name.Should().Be(nameof(Api.LoggerExtensions.KafkaIndividualPricingProcessChangedSkipped));
        _logger.Collector.LatestRecord.StructuredState.Should().Contain(t => t.Key == "Step" && t.Value == "6");
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    public async Task Handle_Active_ShouldSetFlowSwitches(int decisionId, int phaseTypeId)
    {
        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var message = createBaseMessage(taskIdSB: 56781);

        _mediator
           .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 567811), It.IsAny<CancellationToken>()))
           .Returns(() => Task.FromResult(new GetTaskDetailResponse
           {
               TaskObject = new()
               {
                   DecisionId = decisionId,
                   PhaseTypeId = phaseTypeId,
                   ProcessTypeId = 1
               }
           }));

        await handle(dbContext, message);

        _salesArrangementService.Verify(x => x.SetFlowSwitches(It.IsAny<int>(), It.Is<List<EditableFlowSwitch>>(z => 
            z.Count(y => y.FlowSwitchId == 8 && y.Value.GetValueOrDefault()) == 1), It.IsAny<CancellationToken>()), Times.Once());
    }

    private async Task handle(CaseServiceDbContext dbContext, IndividualPricingProcessChanged message)
    {
        var handler = new IndividualPricingProcessChangedHandler(_mediator.Object, _salesArrangementService.Object, _activeTasksService.Object, _logger, dbContext);
        await handler.Handle(_messageContext, message);
    }

    private static IndividualPricingProcessChanged createBaseMessage(ProcessStateEnum state = ProcessStateEnum.ACTIVE, long? caseId = 1, int? processId = 321, int? taskIdSB = 5678)
    {
        return new IndividualPricingProcessChanged
        {
            id = "1234",
            currentTask = new()
            {
                id = taskIdSB.ToString()
            },
            @case = new()
            {
                caseId = new()
                {
                    id = caseId.ToString()
                }
            },
            state = state,
            currentParentProcess = new cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.CurrentParentProcess
            {
                id = processId.ToString()
            }
        };
    }

    private static Mock<IMediator> setupMediatr()
    {
        var mediator = new Mock<IMediator>();

        mediator
            .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 5678), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new GetTaskDetailResponse
            {
                TaskObject = new WorkflowTask
                {
                    ProcessTypeId = 2
                },
                TaskDetail = new TaskDetailItem()
            }));

        mediator
            .Setup(s => s.Send(It.Is<GetTaskDetailRequest>(x => x.TaskIdSb == 56781), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new GetTaskDetailResponse
            {
                TaskObject = new WorkflowTask
                {
                    ProcessTypeId = 1
                },
                TaskDetail = new TaskDetailItem()
            }));

        return mediator;
    }
}
