using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Api.Database.Entities;
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

        if (orderId is null)
        {
            return;
        }

        var realEstateValuation = await _dbContext.RealEstateValuations
            .FirstOrDefaultAsync(r => r.CaseId == caseId && r.OrderId == orderId, token);

        if (realEstateValuation == null || realEstateValuation.ValuationStateId == 5)
        {
            return;
        }

        switch (message.state)
        {
            case ProcessStateEnum.ACTIVE or ProcessStateEnum.SUSPENDED:
                await HandleStateActiveOrSuspended(realEstateValuation, token);
                break;
            case ProcessStateEnum.COMPLETED or ProcessStateEnum.TERMINATED:
                await HandleStateCompletedOrTerminated(realEstateValuation, token);
                break;
        }
    }
    
    private async Task HandleStateActiveOrSuspended(RealEstateValuation realEstateValuationListItem, CancellationToken token)
    {
        if (realEstateValuationListItem is { ValuationStateId: 4, ValuationTypeId: (int)ValuationTypes.Online } or { ValuationStateId: 8 })
        {
            realEstateValuationListItem.ValuationStateId = 9;
            await _dbContext.SaveChangesAsync(token);
        }
    }

    private async Task HandleStateCompletedOrTerminated(RealEstateValuation realEstateValuationListItem, CancellationToken token)
    {
        realEstateValuationListItem.ValuationStateId = realEstateValuationListItem.ValuationTypeId switch
        {
            (int)ValuationTypes.Online => 4,
            (int)ValuationTypes.Dts or (int)ValuationTypes.Standard
                when realEstateValuationListItem.ValuationStateId != 4 => 8,
            _ => realEstateValuationListItem.ValuationStateId
        };

        await _dbContext.SaveChangesAsync(token);
    }
    
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    private readonly ILogger<InformationRequestProcessChangedConsumer> _logger;

    public InformationRequestProcessChangedConsumer(
        RealEstateValuationServiceDbContext dbContext,
        ICaseServiceClient caseService,
        ILogger<InformationRequestProcessChangedConsumer> logger)
    {
        _caseService = caseService;
        _logger = logger;
        _dbContext = dbContext;
    }
}