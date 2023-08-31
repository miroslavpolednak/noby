using CIS.Foms.Enums;
using DomainServices.CaseService.Contracts;
using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.MainLoanProcessChanged;

internal sealed class MainLoanProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged> context)
    {
        var message = context.Message;
        var token = context.CancellationToken;
        
        var caseState = message.state switch
        {
            cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED => CaseStates.Cancelled,
            cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED => CaseStates.Finished,
            _ => (CaseStates) message.processData.@private.mainLoanProcessData.processPhase.code
        };
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(message.@case.caseId.id);
        }

        await _mediator.Send(new UpdateCaseStateRequest
        {
            CaseId = caseId,
            State = (int)caseState,
            StateUpdatedInStarbuild = UpdatedInStarbuildStates.Ok
        }, token);
    }

    private readonly IMediator _mediator;
    private readonly ILogger<MainLoanProcessChangedConsumer> _logger;
    
    public MainLoanProcessChangedConsumer(
        IMediator mediator,
        ILogger<MainLoanProcessChangedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}
