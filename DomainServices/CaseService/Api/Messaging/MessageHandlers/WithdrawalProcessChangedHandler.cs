using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class WithdrawalProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged>
{
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ILogger<WithdrawalProcessChangedHandler> _logger;

    public WithdrawalProcessChangedHandler(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        IDocumentOnSAServiceClient documentOnSAService,
        ILogger<WithdrawalProcessChangedHandler> logger)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _documentOnSAService = documentOnSAService;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.WithdrawalProcessChanged message)
    {
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
        
        var salesArrangementResponse = await _salesArrangementService.GetSalesArrangementList(caseId);
        
        foreach (var salesArrangement in salesArrangementResponse.SalesArrangements.Where(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.Drawing))
        {
            var salesArrangementId = salesArrangement.SalesArrangementId;
            var state = code switch
            {
                1 => 6,
                3 => 3,
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