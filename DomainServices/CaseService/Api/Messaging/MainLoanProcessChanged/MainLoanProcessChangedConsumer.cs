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
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MainLoanProcessChangedConsumer), message.@case.caseId.id);
        }

        var caseState = message.processData.@private.mainLoanProcessData.processPhase.code;
        _logger.UpdateCaseStateStart(caseId, caseState);
        
        await _mediator.Send(new UpdateCaseStateRequest
        {
            CaseId = caseId,
            State = caseState,
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
