﻿using CIS.Foms.Enums;
using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
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
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
        var caseId = long.Parse(message.@case.caseId.id, CultureInfo.InvariantCulture);

        await _linkTaskToCase.Link(caseId, currentTaskId, token);
        
        if (message.state != ProcessStateEnum.COMPLETED)
        {
            return;
        }
        
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = currentTaskId }, token);
        var decisionId = taskDetail.TaskDetail.PriceException.DecisionId;
        var flowSwitchId = decisionId switch
        {
            1 => (int)FlowSwitches.IsWflTaskForIPApproved,
            2 => (int)FlowSwitches.IsWflTaskForIPNotApproved,
            _ => 0
        };

        if (flowSwitchId == 0)
        {
            return;
        }
        
        var flowSwitch = new EditableFlowSwitch
        {
            Value = true,
            FlowSwitchId = flowSwitchId
        };

        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId, token);
        foreach (var salesArrangement in salesArrangementResponse.SalesArrangements.Where(t => t.IsProductSalesArrangement()))
        {
            await _salesArrangementService.SetFlowSwitches(salesArrangement.SalesArrangementId, new() { flowSwitch }, token);
        }
    }

    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly Services.LinkTaskToCaseService _linkTaskToCase;

    public IndividualPricingProcessChangedConsumer(
        IMediator mediator,
        Services.LinkTaskToCaseService linkTaskToCase,
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService)
    {
        _linkTaskToCase = linkTaskToCase;
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
    }
}