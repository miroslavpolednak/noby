using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients.v1;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.Entities;
using DomainServices.RealEstateValuationService.Contracts;
using KafkaFlow;

namespace DomainServices.RealEstateValuationService.Api.Messaging.InformationRequestProcessChanged;

internal sealed class InformationRequestProcessChangedHandler  : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.InformationRequestProcessChanged>
{
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    private readonly ILogger<InformationRequestProcessChangedHandler> _logger;

    public InformationRequestProcessChangedHandler(RealEstateValuationServiceDbContext dbContext,
                                                   ICaseServiceClient caseService,
                                                   ILogger<InformationRequestProcessChangedHandler> logger)
    {
        _dbContext = dbContext;
        _caseService = caseService;
        _logger = logger;
    }

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

        if (realEstateValuation == null || realEstateValuation.ValuationStateId == 5)
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

    private static void HandleStateActiveOrSuspended(RealEstateValuation realEstateValuationListItem)
    {
        if (realEstateValuationListItem is { ValuationStateId: 4, ValuationTypeId: (int)ValuationTypes.Online } or { ValuationStateId: 8 })
        {
            realEstateValuationListItem.ValuationStateId = 9;
        }
    }

    private static void HandleStateCompletedOrTerminated(RealEstateValuation realEstateValuationListItem)
    {
        realEstateValuationListItem.ValuationStateId = realEstateValuationListItem.ValuationTypeId switch
        {
            (int)ValuationTypes.Online => 4,
            (int)ValuationTypes.Dts or (int)ValuationTypes.Standard
                when realEstateValuationListItem.ValuationStateId != 4 => 8,
            _ => realEstateValuationListItem.ValuationStateId
        };
    } 
}