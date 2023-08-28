using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using MassTransit;

namespace DomainServices.RealEstateValuationService.Api.Messaging.InformationRequestProcessChanged;

internal sealed class InformationRequestProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(message.@case.caseId.id);
        }
        
        var taskDetail = await _caseService.GetTaskDetail(currentTaskId, token);
        var orderId = taskDetail.TaskDetail.Request.OrderId;

        if (orderId is null) return;
        
        var getListRequest = new GetRealEstateValuationListRequest { CaseId = caseId };
        var getListResponse = await _mediator.Send(getListRequest, token);
        var realEstateValuationListItem = getListResponse.RealEstateValuationList.FirstOrDefault(i => i.OrderId == orderId);
        
        if (realEstateValuationListItem == null || realEstateValuationListItem.ValuationStateId == 5) return;
        
        if (message.state is ProcessStateEnum.ACTIVE or ProcessStateEnum.SUSPENDED)
        {
            if (realEstateValuationListItem is { ValuationStateId: 4, ValuationTypeId: ValuationTypes.Online })
            {
                await UpdateState(realEstateValuationListItem, 8, token);
            }
            else if (realEstateValuationListItem.ValuationStateId == 8)
            {
                await UpdateState(realEstateValuationListItem, 9, token);
            }
        }

        if (message.state is ProcessStateEnum.COMPLETED or ProcessStateEnum.TERMINATED)
        {
            if (realEstateValuationListItem.ValuationTypeId  == ValuationTypes.Online)
            {
                await UpdateState(realEstateValuationListItem, 4, token);
            }
            else if (realEstateValuationListItem.ValuationTypeId  is ValuationTypes.Dts or ValuationTypes.Standard)
            {
                if (realEstateValuationListItem.ValuationStateId == 4) return;
                await UpdateState(realEstateValuationListItem, 8, token);
            }
        }
    }

    private async Task UpdateState(RealEstateValuationListItem realEstateValuationListItem, int valuationStateId, CancellationToken token)
    {
        var updateRequest = new UpdateStateByRealEstateValuationRequest
        {
            RealEstateValuationId = realEstateValuationListItem.RealEstateValuationId,
            ValuationStateId = valuationStateId
        };

        await _mediator.Send(updateRequest, token);
    }
    
    private readonly IMediator _mediator;
    private readonly ICaseServiceClient _caseService;
    private readonly ILogger<InformationRequestProcessChangedConsumer> _logger;

    public InformationRequestProcessChangedConsumer(
        IMediator mediator,
        ICaseServiceClient caseService,
        ILogger<InformationRequestProcessChangedConsumer> logger)
    {
        _mediator = mediator;
        _caseService = caseService;
        _logger = logger;
    }
}