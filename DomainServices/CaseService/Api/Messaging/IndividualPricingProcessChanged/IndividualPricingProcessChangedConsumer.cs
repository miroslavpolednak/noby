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
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.IndividualPricingProcessChanged> context)
    {
        var message = context.Message;
        var token = context.CancellationToken;

        if (message.state != ProcessStateEnum.COMPLETED)
        {
            return;
        }
        
        var currentTaskId = int.Parse(message.currentTask.id, CultureInfo.InvariantCulture);
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
        
        var flowSwitch = new FlowSwitch
        {
            Value = true,
            FlowSwitchId = flowSwitchId
        };

        var caseId = int.Parse(message.@case.caseId.id, CultureInfo.InvariantCulture);
        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId, token);
        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(token);

        foreach (var salesArrangement in salesArrangementResponse.SalesArrangements)
        {
            var salesArrangementType = salesArrangementTypes.Single(t => t.Id == salesArrangement.SalesArrangementTypeId);
            if (salesArrangementType.SalesArrangementCategory == (int) SalesArrangementCategories.ProductRequest)
            {
                await _salesArrangementService.SetFlowSwitches(salesArrangement.SalesArrangementId, new() { flowSwitch }, token);
            }
        }
    }

    public IndividualPricingProcessChangedConsumer(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
    }
}