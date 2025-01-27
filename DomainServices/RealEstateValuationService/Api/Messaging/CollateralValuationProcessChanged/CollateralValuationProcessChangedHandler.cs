﻿using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.Entities;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using KafkaFlow;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Messaging.CollateralValuationProcessChanged;

internal sealed class CollateralValuationProcessChangedHandler(
    IDocumentDataStorage _documentDataStorage,
    RealEstateValuationServiceDbContext _dbContext,
    ICaseServiceClient _caseService,
    IPreorderServiceClient _preorderServiceClient,
    ILogger<CollateralValuationProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged>
{
    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.CollateralValuationProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(message.currentTask.id);
            return;
        }

        var taskDetail = await _caseService.GetTaskDetail(currentTaskId);
        var orderId = taskDetail.TaskDetail.RealEstateValuation.OrderId;

        if (message.state != ProcessStateEnum.COMPLETED && message.state != ProcessStateEnum.TERMINATED)
        {
            return;
        }
        
        var realEstateValuation = await _dbContext
                                        .RealEstateValuations
                                        .FirstOrDefaultAsync(t => t.OrderId == orderId);

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
                await HandleCompleted(realEstateValuation, taskDetail.TaskDetail);
                break;
        }

        await _dbContext.SaveChangesAsync();
    }

    private static void HandleTerminated(RealEstateValuation realEstateValuation)
    {
        realEstateValuation.ValuationStateId = (int)WorkflowTaskStates.Cancelled;
    }

    private async Task HandleCompleted(RealEstateValuation realEstateValuation, TaskDetailItem taskDetail)
    {
        if (!taskDetail.RealEstateValuation.OnlineValuation)
        {
            var orderResultResponse = await _preorderServiceClient.GetOrderResult(realEstateValuation.OrderId!.Value);

            // ulozeni novych cen
            realEstateValuation.Prices = orderResultResponse
                                         ?.Select(t => new Database.Entities.PriceDetail
                                         {
                                             Price = t.Price,
                                             PriceSourceType = t.PriceSourceType
                                         })
                                         .ToList();
            
            var loadedDetail = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.RealEstateValudationData, int>(realEstateValuation.RealEstateValuationId);
            loadedDetail!.Data!.Documents =
            [
                new Database.DocumentDataEntities.RealEstateValudationData.RealEstateValuationDocument
                {
                    DocumentInfoPrice = taskDetail.RealEstateValuation.DocumentInfoPrice,
                    DocumentRecommendationForClient = taskDetail.RealEstateValuation.DocumentRecommendationForClient
                }
            ];
            await _documentDataStorage.UpdateByEntityId(realEstateValuation.RealEstateValuationId, loadedDetail.Data);
        }

        realEstateValuation.ValuationStateId = (int)WorkflowTaskStates.KontrolaUdaju;
    }
}