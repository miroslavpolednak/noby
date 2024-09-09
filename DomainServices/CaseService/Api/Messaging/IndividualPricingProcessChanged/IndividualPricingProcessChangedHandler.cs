using cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using KafkaFlow;

namespace DomainServices.CaseService.Api.Messaging.MessageHandlers;

internal class IndividualPricingProcessChangedHandler(
    IMediator _mediator,
    ISalesArrangementServiceClient _salesArrangementService,
    ActiveTasksService _activeTasksService,
    ILogger<IndividualPricingProcessChangedHandler> _logger,
    Database.CaseServiceDbContext _dbContext) 
    : IMessageHandler<IndividualPricingProcessChanged>
{
    public async Task Handle(IMessageContext context, IndividualPricingProcessChanged message)
    {
        _logger.TempMessageHeaderLog(context, message.eventId, message.state.ToString(), message.processData?.@private?.individualPricingProcessData?.processPhase?.code);

        var (taskIdSB, taskId, caseId, caseState, isValid) = initialValidations(message);
        if (!isValid)
        {
            return;
        }
        
        // detail tasku
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = taskIdSB });
        
        // dalsi validace
        if (taskDetail.TaskObject.ProcessTypeId == 1 && caseState != EnumCaseStates.InProgress)
        {
            _logger.KafkaIndividualPricingProcessChangedSkipped(caseId, taskIdSB, taskDetail.TaskObject.ProcessTypeId);
            return;
        }
        else if (taskDetail.TaskObject.ProcessTypeId != 1)
        {
            var p = (await getSalesArrangements(caseId)).FirstOrDefault(t => t.ProcessId == taskId);

        }

        if (message.state == ProcessStateEnum.TERMINATED && taskDetail.TaskObject.ProcessTypeId != 1)
        {
            // ukonceni
            await _dbContext.ConfirmedPriceExceptions.Where(t => t.TaskId == taskId).ExecuteDeleteAsync(CancellationToken.None);

            _logger.KafkaIndividualPricingProcessChangedSkipped(caseId, taskIdSB, taskDetail.TaskObject.ProcessTypeId);
            return;
        }
        //Schválené/zamítnuté IC
        else if (message.state is (ProcessStateEnum.COMPLETED or ProcessStateEnum.ACTIVE) 
            && taskDetail.TaskObject.DecisionId is (1 or 2) 
            && taskDetail.TaskObject.PhaseTypeId == 2) 
        {
            //Jde o schválené/zamítnuté IC k jinému než Hlavnímu úvěrovému procesu
            if (taskDetail.TaskObject.ProcessTypeId != 1 && await salesArrangementExists(caseId, taskId))
            {
                await saveEntity(taskId, taskIdSB, caseId, message.occurredOn, null);
            }
        }
        else if (message.state is (ProcessStateEnum.COMPLETED or ProcessStateEnum.ACTIVE)
            && taskDetail.TaskObject.ProcessTypeId != 1)
        {
            _logger.KafkaIndividualPricingProcessChangedSkipped(caseId, taskIdSB, taskDetail.TaskObject.ProcessTypeId);
            return;
        }

        await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, taskDetail, CancellationToken.None);

        // nastaveni flow switches
        var flowSwitches = (message.state switch
        {
            ProcessStateEnum.COMPLETED => GetFlowSwitchesForCompleted(taskDetail.TaskObject.DecisionId),
            ProcessStateEnum.ACTIVE => GetFlowSwitchesForActive(),
            ProcessStateEnum.TERMINATED => GetFlowSwitchesForTerminated(),
            _ => [],
        }).ToList();

        if (flowSwitches.Count != 0)
        {
            var productSaleArrangements = await _salesArrangementService.GetProductSalesArrangements(caseId);
            foreach (var salesArrangement in productSaleArrangements)
            {
                await _salesArrangementService.SetFlowSwitches(salesArrangement.SalesArrangementId, flowSwitches);
            }
        }
    }

    private (int TaskIdSB, long TaskId, long CaseId, EnumCaseStates CaseState, bool IsValid) initialValidations(IndividualPricingProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var taskIdSB))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.currentTask.id);
            return (0, 0, 0, EnumCaseStates.Unknown, false);
        }

        if (!int.TryParse(message.id, out var taskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.id);
            return (0, 0, 0, EnumCaseStates.Unknown, false);
        }

        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.@case.caseId.id);
            return (0, 0, 0, EnumCaseStates.Unknown, false);
        }

        var caseState = _dbContext.Cases.FirstOrDefault(t => t.CaseId == caseId)?.State;
        if (!caseState.HasValue)
        {
            _logger.KafkaCaseIdNotFound(nameof(IndividualPricingProcessChangedHandler), caseId);
            return (0, 0, 0, EnumCaseStates.Unknown, false);
        }

        if (message.state is not (ProcessStateEnum.ACTIVE or ProcessStateEnum.TERMINATED or ProcessStateEnum.COMPLETED))
        {
            _logger.KafkaHandlerSkippedDueToState(nameof(IndividualPricingProcessChangedHandler), caseId, taskIdSB, message.state.ToString());
            return (0, 0, 0, (EnumCaseStates)caseState, false);
        }

        return (taskIdSB, taskId, caseId, (EnumCaseStates)caseState.Value, true);
    }

    private async Task<List<SalesArrangement>> getSalesArrangements(long caseId)
    {
        if (_salesArrangementList is null)
        {
            _salesArrangementList = (await _salesArrangementService.GetSalesArrangementList(caseId)).SalesArrangements.ToList();
        }
        return _salesArrangementList;
    }

    private async Task<bool> salesArrangementExists(long caseId, long taskId)
    {
        var list = await getSalesArrangements(caseId);
        return list.Any(t => t.ProcessId == taskId);
    }

    private async Task saveEntity(long taskId, int taskIdSB, long caseId, DateTime? confirmedDate, DateTime? declinedDate)
    {
        var existingConfirmedEntity = _dbContext.ConfirmedPriceExceptions.FirstOrDefault(t => t.TaskId == taskId);

        if (existingConfirmedEntity is null)
        {
            existingConfirmedEntity = new Database.Entities.ConfirmedPriceException
            {
                CaseId = caseId,
                TaskId = taskId
            };
            _dbContext.ConfirmedPriceExceptions.Add(existingConfirmedEntity);
        }
        
        existingConfirmedEntity.ConfirmedDate = confirmedDate.HasValue ? DateOnly.FromDateTime(confirmedDate.Value) : null;
        existingConfirmedEntity.DeclinedDate = declinedDate.HasValue ? DateOnly.FromDateTime(declinedDate.Value) : null;
        existingConfirmedEntity.CreatedTime = DateTime.Now;
        existingConfirmedEntity.TaskIdSB = taskIdSB;

        await _dbContext.SaveChangesAsync();
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForCompleted(int? decisionId)
    {
        if (decisionId != 1 && decisionId != 2) yield break;
        
        yield return  new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPApproved, // 9
            Value = decisionId == 1
        };
        
        yield return  new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPNotApproved, // 10
            Value = decisionId == 2
        };
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForActive()
    {
        yield return new EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist,
            Value = true
        };
    }

    private static IEnumerable<EditableFlowSwitch> GetFlowSwitchesForTerminated()
    {
        var flowSwitchIds = new[]
        {
            FlowSwitches.DoesWflTaskForIPExist,
            FlowSwitches.IsWflTaskForIPApproved,
            FlowSwitches.IsWflTaskForIPNotApproved
        };
        
        foreach (var flowSwitchId in flowSwitchIds)
        {
            yield return new EditableFlowSwitch
            {
                FlowSwitchId = (int)flowSwitchId,
                Value = false
            };
        }
    }

    private List<SalesArrangement>? _salesArrangementList;
}