using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class IndividualPricingProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged>
{
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ActiveTasksService _activeTasksService;
    private readonly ILogger<IndividualPricingProcessChangedHandler> _logger;

    public IndividualPricingProcessChangedHandler(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        ActiveTasksService activeTasksService,
        ILogger<IndividualPricingProcessChangedHandler> logger)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _activeTasksService = activeTasksService;
        _logger = logger;
        
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.@case.caseId.id);
        }
        
        // detail tasku
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = currentTaskId });

        if (taskDetail.TaskObject.ProcessTypeId != 1)
        {
            _logger.KafkaIndividualPricingProcessChangedSkipped(caseId, currentTaskId, taskDetail.TaskObject.ProcessTypeId);
            return;
        }

        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, currentTaskId, CancellationToken.None);

        var flowSwitches = (message.state switch
        {
            ProcessStateEnum.COMPLETED => GetFlowSwitchesForCompleted(taskDetail.TaskDetail.PriceException.DecisionId),
            ProcessStateEnum.ACTIVE => GetFlowSwitchesForActive(),
            ProcessStateEnum.TERMINATED => GetFlowSwitchesForTerminated(),
            _ => [],
        }).ToList();
        
        if (flowSwitches.Count != 0)
        {
            var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId);
            var productSaleArrangements = salesArrangementResponse.SalesArrangements
                .Where(t => t.IsProductSalesArrangement())
                .ToList();
            
            foreach (var salesArrangement in productSaleArrangements)
            {
                await _salesArrangementService.SetFlowSwitches(salesArrangement.SalesArrangementId, flowSwitches);
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
}