using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal sealed class LoanExtraPaymentProcessChangedHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ILogger<LoanRetentionProcessChangedHandler> _logger)
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanExtraPaymentProcessChanged>
{
    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanExtraPaymentProcessChanged message)
    {
        if (message.state is not (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED
            or cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED))
            return;

        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(LoanExtraPaymentProcessChangedHandler), message.@case.caseId.id);
            return;
        }

        if (!long.TryParse(message.id, out var processId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(LoanExtraPaymentProcessChangedHandler), message.id);
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
            _logger.KafkaCaseIdNotFound(nameof(LoanExtraPaymentProcessChangedHandler), caseId);

            return;
        }


        if (sa is not null)
        {
            var newState = message.state == cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED ? (int)SalesArrangementStates.Finished : (int)SalesArrangementStates.Cancelled;
            await _salesArrangementService.UpdateSalesArrangementState(sa.SalesArrangementId, newState);
        }
        else
        {
            _logger.KafkaLoanRetentionProcessChangedSkipped(caseId, processId);
        }
    }
}
