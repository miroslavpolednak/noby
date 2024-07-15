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
        var (taskIdSB, taskId, caseId, isValid) = initialValidations(message);
        if (!isValid)
        {
            return;
        }
        
        // detail tasku
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = taskIdSB });
        
        if (message.state == ProcessStateEnum.TERMINATED)
        {
            await _dbContext.ConfirmedPriceExceptions.Where(t => t.TaskId == taskId).ExecuteDeleteAsync(CancellationToken.None);
        }
        else if (taskDetail.TaskObject.DecisionId == 1 && taskDetail.TaskObject.PhaseTypeId == 2) // schvalene IC
        {
            if (taskDetail.TaskObject.ProcessTypeId == 1 || await salesArrangementExists(caseId, taskId))
            {
                await saveEntity(taskId, taskIdSB, caseId, message.occurredOn, null);
            }
        }
        else if (taskDetail.TaskObject.DecisionId == 2 && taskDetail.TaskObject.PhaseTypeId == 2)
        {
            if (taskDetail.TaskObject.ProcessTypeId != 1 || await salesArrangementExists(caseId, taskId))
            {
                await saveEntity(taskId, taskIdSB, caseId, null, message.occurredOn);
            }
        }
        else
        {
            // ukonceni
            return;
        }

        if (taskDetail.TaskObject.ProcessTypeId != 1)
        {
            _logger.KafkaIndividualPricingProcessChangedSkipped(caseId, taskIdSB, taskDetail.TaskObject.ProcessTypeId);
        }
        else
        {
            await _activeTasksService.UpdateActiveTaskByTaskIdSb(caseId, taskIdSB, CancellationToken.None);

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
    }

    private (int TaskIdSB, long TaskId, long CaseId, bool IsValid) initialValidations(IndividualPricingProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out var taskIdSB))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.currentTask.id);
            return (0, 0, 0, false);
        }

        if (!int.TryParse(message.id, out var taskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.id);
            return (0, 0, 0, false);
        }

        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.@case.caseId.id);
            return (0, 0, 0, false);
        }
        else if (!_dbContext.Cases.Any(t => t.CaseId == caseId))
        {
            _logger.KafkaCaseIdNotFound(nameof(IndividualPricingProcessChangedHandler), caseId);
            return (0, 0, 0, false);
        }

        if (message.state is not (ProcessStateEnum.ACTIVE or ProcessStateEnum.TERMINATED or ProcessStateEnum.COMPLETED))
        {
            _logger.KafkaIndividualPricingProcessChangedSkippedState(caseId, taskIdSB, message.state.ToString());
            return (0, 0, 0, false);
        }

        return (taskIdSB, taskId, caseId, true);
    }

    private async Task<bool> salesArrangementExists(long caseId, long taskId)
    {
        var list = await _salesArrangementService.GetSalesArrangementList(caseId);
        return list.SalesArrangements.Any(t => t.ProcessId == taskId);
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
}