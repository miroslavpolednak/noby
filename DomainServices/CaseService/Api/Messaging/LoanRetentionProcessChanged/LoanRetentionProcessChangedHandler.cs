using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal sealed class LoanRetentionProcessChangedHandler(
    ISalesArrangementServiceClient _salesArrangementService, 
    ILogger<LoanRetentionProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged>
{
    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged message)
    {
        if (message.state is not (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED
            or cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED))
            return;

        if (message.state is cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED && message.processData?.@private?.loanRetentionProcessData?.processPhase?.code != 3)
            return;

        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(LoanRetentionProcessChangedHandler), message.@case.caseId.id);
            return;
        }

        if (!long.TryParse(message.id, out var processId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(LoanRetentionProcessChangedHandler), message.id);
            return;
        }

        SalesArrangement? sa;
        try
        {
            var salesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId);
            sa = salesArrangements.SalesArrangements.FirstOrDefault(t => t.ProcessId == processId);
        }
        catch (CisNotFoundException)
        {
            _logger.KafkaCaseIdNotFound(nameof(LoanRetentionProcessChangedHandler), caseId);

            return;
        }
        

        if (sa is not null)
        {
            var newState = message.state == cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED ? (int)EnumSalesArrangementStates.Finished : (int)EnumSalesArrangementStates.Cancelled;
            await _salesArrangementService.UpdateSalesArrangementState(sa.SalesArrangementId, newState);
        }
        else
        {
            _logger.KafkaLoanRetentionProcessChangedSkipped(caseId, processId);
        }
    }
}