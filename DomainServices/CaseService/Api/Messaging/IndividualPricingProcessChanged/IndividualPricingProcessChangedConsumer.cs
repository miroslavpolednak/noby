using CIS.Foms.Enums;
using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.IndividualPricingProcessChanged;

internal sealed class IndividualPricingProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(message.@case.caseId.id);
        }
        
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = currentTaskId }, token);
        var decisionId = taskDetail.TaskDetail.PriceException.DecisionId;
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, token);

        var flowSwitches = (message.state switch
        {
            ProcessStateEnum.COMPLETED => GetFlowSwitchesForCompleted(decisionId),
            ProcessStateEnum.ACTIVE => GetFlowSwitchesForActive(),
            ProcessStateEnum.TERMINATED => GetFlowSwitchesForTerminated(),
            _ => Enumerable.Empty<EditableFlowSwitch>(),
        }).ToList();
        
        if (flowSwitches.Any())
        {
            var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId, token);
            var productSaleArrangements = salesArrangementResponse.SalesArrangements
                .Where(t => t.IsProductSalesArrangement())
                .ToList();
            
            foreach (var salesArrangement in productSaleArrangements)
            {
                await _salesArrangementService.SetFlowSwitches(salesArrangement.SalesArrangementId, flowSwitches, token);
            }
        }
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForCompleted(int? decisionId)
    {
        if (decisionId != 1 && decisionId != 2) yield break;
        
        yield return  new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPApproved, // 9
            Value = decisionId == 1
        };
        
        yield return  new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPNotApproved, // 10
            Value = decisionId == 2
        };
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForActive()
    {
        yield return new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist,
            Value = true
        };
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForTerminated()
    {
        var flowSwitchIds = new[]
        {
            FlowSwitches.DoesWflTaskForIPExist,
            FlowSwitches.IsWflTaskForIPApproved,
            FlowSwitches.IsWflTaskForIPNotApproved
        };
        
        foreach (var flowSwitchId in flowSwitchIds)
        {
            yield return new EditableFlowSwitch
            {
                FlowSwitchId = (int)flowSwitchId,
                Value = false
            };
        }
    }
    
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<IndividualPricingProcessChangedConsumer> _logger;

    public IndividualPricingProcessChangedConsumer(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        ActiveTasksService activeTasksService,
        ILogger<IndividualPricingProcessChangedConsumer> logger)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _activeTasksService = activeTasksService;
        _logger = logger;
        
    }
}