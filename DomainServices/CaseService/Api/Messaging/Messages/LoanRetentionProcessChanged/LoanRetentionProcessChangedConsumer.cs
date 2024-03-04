using DomainServices.SalesArrangementService.Clients;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.LoanRetentionProcessChanged;

internal sealed class LoanRetentionProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged> context)
    {
        if (context.Message.state is cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED or cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED)
        {
            if (!long.TryParse(context.Message.@case.caseId.id, out long caseId))
            {
                _logger.KafkaMessageCaseIdIncorrectFormat(nameof(LoanRetentionProcessChangedConsumer), context.Message.@case.caseId.id);
                return;
            }

            if (!long.TryParse(context.Message.id, out long processId))
            {
                _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(LoanRetentionProcessChangedConsumer), context.Message.id);
                return;
            }

            var salesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, context.CancellationToken);
            var sa = salesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == processId);
            if (sa is not null)
            {
                int newState = context.Message.state == cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED ? (int)SalesArrangementStates.Finished : (int)SalesArrangementStates.Cancelled;
                await _salesArrangementService.UpdateSalesArrangementState(sa.SalesArrangementId, newState, context.CancellationToken);
            }
            else
            {
                _logger.KafkaLoanRetentionProcessChangedSkipped(caseId, processId);
            }
        }
    }

    private readonly ILogger<LoanRetentionProcessChangedConsumer> _logger;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public LoanRetentionProcessChangedConsumer(ISalesArrangementServiceClient salesArrangementService, ILogger<LoanRetentionProcessChangedConsumer> logger)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
    }
}
