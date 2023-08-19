﻿using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.WithdrawalProcessChanged;

internal sealed class WithdrawalProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged>
{
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ILogger<WithdrawalProcessChangedConsumer> _logger;

    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged> context)
    {
        var message = context.Message;
        var token = context.CancellationToken;

        var code = message.processData.@private.withdrawalProcessData.processPhase.code;
        if (code != 1 && code != 3)
        {
            return;
        }
        
        if (!int.TryParse(context.Message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(context.Message.@case.caseId.id);
        }
        
        if (!long.TryParse(context.Message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(context.Message.@case.caseId.id);
        }
        
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = currentTaskId }, token);
        var taskDocumentIds = taskDetail.TaskDetail.TaskDocumentIds.ToHashSet();
        
        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId, token);
        
        foreach (var salesArrangement in salesArrangementResponse.SalesArrangements)
        {
            var salesArrangementId = salesArrangement.SalesArrangementId;
            var state = code switch
            {
                1 => 6,
                3 => 3,
                _ => throw new ArgumentException()
            };
            
            var documentResponse = await _documentOnSAService.GetDocumentsOnSAList(salesArrangementId, token);
            var documentsOnSa = documentResponse.DocumentsOnSA
                .Where(d => d.IsFinal && taskDocumentIds.Contains(d.EArchivId))
                .ToList();
            
            foreach (var documentOnSa in documentsOnSa)
            {
                await _salesArrangementService.UpdateSalesArrangementState(documentOnSa.SalesArrangementId, state, token);
            }
        }
    }
    
    public WithdrawalProcessChangedConsumer(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        IDocumentOnSAServiceClient documentOnSAService,
        ILogger<WithdrawalProcessChangedConsumer> logger)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _documentOnSAService = documentOnSAService;
        _logger = logger;
    }
}