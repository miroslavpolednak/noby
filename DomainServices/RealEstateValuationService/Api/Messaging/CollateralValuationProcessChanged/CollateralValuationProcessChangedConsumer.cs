using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using MassTransit;

namespace DomainServices.RealEstateValuationService.Api.Messaging.CollateralValuationProcessChanged;

internal sealed class CollateralValuationProcessChangedConsumer
    : IConsumer<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> 
{
    public async Task Consume(ConsumeContext<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged> context)
    {
        var token = context.CancellationToken;
        var message = context.Message;
        
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(message.currentTask.id);
        }

        var taskDetail = await _caseService.GetTaskDetail(currentTaskId, token);
        var realEstateValuation = taskDetail.TaskDetail.RealEstateValuation;
        
        await (message.state switch
        {
            ProcessStateEnum.TERMINATED => HandleTerminated(realEstateValuation, token),
            ProcessStateEnum.COMPLETED => HandleCompleted(realEstateValuation, token),
            _ => Task.CompletedTask
        });
    }

    private async Task HandleTerminated(AmendmentRealEstateValuation realEstateValuation, CancellationToken token)
    {
        var getRequest = new GetRealEstateValuationDetailByOrderIdRequest { OrderId = realEstateValuation.OrderId };
        var realEstateValuationDetail = await _mediator.Send(getRequest, token);

        var updateRequest = new UpdateStateByRealEstateValuationRequest
        {
            RealEstateValuationId = realEstateValuationDetail.RealEstateValuationId,
            ValuationStateId = 5
        };

        await _mediator.Send(updateRequest, token);
    }

    private async Task HandleCompleted(AmendmentRealEstateValuation realEstateValuation, CancellationToken token)
    {
        var getRequest = new GetRealEstateValuationDetailByOrderIdRequest { OrderId = realEstateValuation.OrderId };
        var realEstateValuationDetail = await _mediator.Send(getRequest, token);
        
        if (!realEstateValuation.OnlineValuation)
        {
            var orderResultResponse = await _preorderServiceClient.GetOrderResult(realEstateValuation.OrderId, token);
            var valuationResultCurrentPrice = orderResultResponse.ValuationResultCurrentPrice;
            var valuationResultFuturePrice = orderResultResponse.ValuationResultFuturePrice;
            // todo:
        }

        var updateRequest = new UpdateStateByRealEstateValuationRequest
        {
            RealEstateValuationId = realEstateValuationDetail.RealEstateValuationId,
            ValuationStateId = 4
        };
        
        await _mediator.Send(updateRequest, token);
    }

    private readonly IMediator _mediator;
    private readonly ICaseServiceClient _caseService;
    private readonly IPreorderServiceClient _preorderServiceClient;
    private readonly ILogger<CollateralValuationProcessChangedConsumer> _logger;

    public CollateralValuationProcessChangedConsumer(
        IMediator mediator,
        ICaseServiceClient caseService,
        IPreorderServiceClient preorderServiceClient,
        ILogger<CollateralValuationProcessChangedConsumer> logger)
    {
        _mediator = mediator;
        _caseService = caseService;
        _preorderServiceClient = preorderServiceClient;
        _logger = logger;
    }
}