using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class WithdrawalProcessChangedHandler(
	IMediator _mediator,
	ISalesArrangementServiceClient _salesArrangementService,
	IDocumentOnSAServiceClient _documentOnSAService,
	ILogger<WithdrawalProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged>
{
	public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged message)
    {
        _logger.TempMessageHeaderLog(context, message.eventId, message.state.ToString(), message.processData?.@private?.withdrawalProcessData?.processPhase?.code);

        var code = message.processData.@private.withdrawalProcessData.processPhase.code;
        if (code != 1 && code != 3)
        {
            return;
        }
        
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(WithdrawalProcessChangedHandler), message.@case.caseId.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(WithdrawalProcessChangedHandler), message.@case.caseId.id);
        }
        
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = currentTaskId });
        var taskDocumentIds = taskDetail.TaskDetail.TaskDocumentIds.ToHashSet();

        GetSalesArrangementListResponse salesArrangementResponse;
        try
        {
            salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId);
        }
        catch (CisNotFoundException)
        {
            _logger.KafkaCaseIdNotFound(nameof(WithdrawalProcessChangedHandler), caseId);

            return;
        }
        
        foreach (var salesArrangement in salesArrangementResponse.SalesArrangements.Where(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.Drawing))
        {
            var salesArrangementId = salesArrangement.SalesArrangementId;
            var state = code switch
            {
                1 => EnumSalesArrangementStates.Disbursed,
                3 => EnumSalesArrangementStates.Cancelled,
                _ => throw new ArgumentException(nameof(code))
            };
            
            var documentResponse = await _documentOnSAService.GetDocumentsOnSAList(salesArrangementId);
            var documentsOnSa = documentResponse.DocumentsOnSA
                .Where(d => taskDocumentIds.Contains(d.EArchivId))
                .ToList();
            
            foreach (var documentOnSa in documentsOnSa)
            {
                await _salesArrangementService.UpdateSalesArrangementState(documentOnSa.SalesArrangementId, state);
            }
        }
    }
}