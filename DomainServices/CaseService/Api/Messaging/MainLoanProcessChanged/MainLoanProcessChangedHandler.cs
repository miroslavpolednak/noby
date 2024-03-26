using DomainServices.CaseService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class MainLoanProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged>
{
    private readonly IMediator _mediator;
    private readonly ILogger<MainLoanProcessChangedHandler> _logger;
    
    public MainLoanProcessChangedHandler(
        IMediator mediator,
        ILogger<MainLoanProcessChangedHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.MainLoanProcessChanged message)
    {
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(MainLoanProcessChangedHandler), message.@case.caseId.id);
            return;
        }

        var caseState = message.processData.@private.mainLoanProcessData.processPhase.code;
        _logger.UpdateCaseStateStart(caseId, caseState);

        try
        {
            await _mediator.Send(new UpdateCaseStateRequest
            {
                CaseId = caseId,
                State = caseState,
                StateUpdatedInStarbuild = UpdatedInStarbuildStates.Ok
            });
        }
        catch (CisValidationException)
        {
            //Ignore
        }
    }
}