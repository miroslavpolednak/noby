using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients.v1;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.Entities;
using DomainServices.RealEstateValuationService.Contracts;
using KafkaFlow;

namespace DomainServices.RealEstateValuationService.Api.Messaging.InformationRequestProcessChanged;

internal sealed class InformationRequestProcessChangedHandler(
    RealEstateValuationServiceDbContext _dbContext,
    ICaseServiceClient _caseService,
    ILogger<InformationRequestProcessChangedHandler> _logger) 
    : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var currentTaskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(message.currentTask.id);
        }
        
        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(message.@case.caseId.id);
        }
        
        var taskDetail = await _caseService.GetTaskDetail(currentTaskId);
        var orderId = taskDetail.TaskDetail.Request.OrderId;

        if (orderId is null)
        {
            return;
        }

        var realEstateValuation = await _dbContext.RealEstateValuations
                                                  .FirstOrDefaultAsync(r => r.CaseId == caseId && r.OrderId == orderId);

        if (realEstateValuation == null || realEstateValuation.ValuationStateId == (int)WorkflowTaskStates.Cancelled)
        {
            return;
        }

        switch (message.state)
        {
            case ProcessStateEnum.ACTIVE or ProcessStateEnum.SUSPENDED:
                HandleStateActiveOrSuspended(realEstateValuation);
                break;
            case ProcessStateEnum.COMPLETED or ProcessStateEnum.TERMINATED:
                HandleStateCompletedOrTerminated(realEstateValuation);
                break;
        }
        
        await _dbContext.SaveChangesAsync();
    }

    private void HandleStateActiveOrSuspended(RealEstateValuation realEstateValuationListItem)
    {
        if (realEstateValuationListItem.ValuationStateId is not ((int)WorkflowTaskStates.ProbihaOceneni or (int)WorkflowTaskStates.KontrolaUdaju))
            return;

        realEstateValuationListItem.ValuationStateId = (int)WorkflowTaskStates.Dozadani;

        _logger.RealEstateValuationStateIdChanged(realEstateValuationListItem.RealEstateValuationId, realEstateValuationListItem.ValuationStateId);
    }

    private void HandleStateCompletedOrTerminated(RealEstateValuation realEstateValuationListItem)
    {
        if (realEstateValuationListItem.ValuationStateId is (int)WorkflowTaskStates.Completed)
            return;

        realEstateValuationListItem.ValuationStateId = realEstateValuationListItem.ValuationTypeId switch
        {
            (int)ValuationTypes.Online => (int)WorkflowTaskStates.KontrolaUdaju,
            _ => (int)WorkflowTaskStates.ProbihaOceneni
        };

        _logger.RealEstateValuationStateIdChanged(realEstateValuationListItem.RealEstateValuationId, realEstateValuationListItem.ValuationStateId);
    } 
}