using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.Entities;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using MassTransit;
using Newtonsoft.Json;

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
            return;
        }

        var taskDetail = await _caseService.GetTaskDetail(currentTaskId, token);
        var orderId = taskDetail.TaskDetail.RealEstateValuation.OrderId;

        if (message.state != ProcessStateEnum.COMPLETED && message.state != ProcessStateEnum.TERMINATED)
        {
            return;
        }
        
        var realEstateValuation = await _dbContext.RealEstateValuations
            .FirstOrDefaultAsync(t => t.OrderId == orderId, token);

        if (realEstateValuation is null)
        {
            _logger.RealEstateValuationByOrderIdNotFound(orderId);
            return;
        }

        switch (message.state)
        {
            case ProcessStateEnum.TERMINATED:
                HandleTerminated(realEstateValuation);
                break;
            case ProcessStateEnum.COMPLETED:
                await HandleCompleted(realEstateValuation, taskDetail.TaskDetail, token);
                break;
        }

        await _dbContext.SaveChangesAsync(token);
    }

    private static void HandleTerminated(RealEstateValuation realEstateValuation)
    {
        realEstateValuation.ValuationStateId = 5;
    }

    private async Task HandleCompleted(RealEstateValuation realEstateValuation, TaskDetailItem taskDetail, CancellationToken token)
    {
        if (!taskDetail.RealEstateValuation.OnlineValuation)
        {
            var orderResultResponse = await _preorderServiceClient.GetOrderResult(realEstateValuation.OrderId!.Value, token);
            realEstateValuation.ValuationResultCurrentPrice = ConvertToNullableInt32(orderResultResponse.ValuationResultCurrentPrice);
            realEstateValuation.ValuationResultFuturePrice = ConvertToNullableInt32(orderResultResponse.ValuationResultFuturePrice);

            var documents = new List<RealEstateValuationDocument>
            {
                new ()
                {
                    DocumentInfoPrice = taskDetail.RealEstateValuation.DocumentInfoPrice,
                    DocumentRecommendationForClient = taskDetail.RealEstateValuation.DocumentRecommendationForClient
                }
            };

            realEstateValuation.Documents = JsonConvert.SerializeObject(documents);
        }

        realEstateValuation.ValuationStateId = 4;
    }

    private static int? ConvertToNullableInt32(decimal? value)
    {
        return value.HasValue ? Convert.ToInt32(value.Value) : null;
    }
    
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    private readonly IPreorderServiceClient _preorderServiceClient;
    private readonly ILogger<CollateralValuationProcessChangedConsumer> _logger;

    public CollateralValuationProcessChangedConsumer(
        RealEstateValuationServiceDbContext dbContext,
        ICaseServiceClient caseService,
        IPreorderServiceClient preorderServiceClient,
        ILogger<CollateralValuationProcessChangedConsumer> logger)
    {
        _dbContext = dbContext;
        _caseService = caseService;
        _preorderServiceClient = preorderServiceClient;
        _logger = logger;
    }
}