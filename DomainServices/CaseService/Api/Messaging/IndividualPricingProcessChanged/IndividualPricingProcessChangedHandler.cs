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
    IActiveTasksService _activeTasksService,
    ILogger<IndividualPricingProcessChangedHandler> _logger,
    Database.CaseServiceDbContext _dbContext) 
    : IMessageHandler<IndividualPricingProcessChanged>
{
    public async Task Handle(IMessageContext context, IndividualPricingProcessChanged message)
    {
        _logger.TempMessageHeaderLog(context, message.eventId, message.state.ToString(), message.processData?.@private?.individualPricingProcessData?.processPhase?.code);

        if (!initialValidations(message))
        {
            return;
        }
        
        // detail tasku
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest { TaskIdSb = _taskIdSB });

        // neni v zadani, ale je to v poradku
        await _activeTasksService.UpdateActiveTaskByTaskIdSb(_caseId, taskDetail, CancellationToken.None);

        // Validovat, že daná KAFKA zpráva se má konzumovat
        if (await afterValidations(message, taskDetail) == false)
        {
            return;
        }

        // Získat detail workflow úkolu, ke kterému přišla notifikace
        if (message.state == ProcessStateEnum.TERMINATED)
        {
            if (taskDetail.TaskObject.ProcessTypeId != 1)
            {
                // ukonceni
                await _dbContext.ConfirmedPriceExceptions.Where(t => t.TaskId == _taskId).ExecuteDeleteAsync(CancellationToken.None);

                _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 5);
                return;
            }
        }
        else if (message.state is (ProcessStateEnum.ACTIVE or ProcessStateEnum.COMPLETED))
        {
            //Schválené/zamítnuté IC
            if (taskDetail.TaskObject.DecisionId is (1 or 2) && taskDetail.TaskObject.PhaseTypeId == 2)
            {
                //Jde o schválené/zamítnuté IC k jinému než Hlavnímu úvěrovému procesu
                if (taskDetail.TaskObject.ProcessTypeId != 1)
                {
                    await saveEntity(_taskId, _taskIdSB, _caseId, message.occurredOn, taskDetail.TaskObject.DecisionId == 1);
                }
            }
            else if (taskDetail.TaskObject.ProcessTypeId != 1)
            {
                _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 6);
                return;
            }
        }

        // nastaveni flow switches
        var productSaleArrangementId = (await getSalesArrangements())
            .FirstOrDefault(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.Mortgage)
            ?.SalesArrangementId;

        if (productSaleArrangementId.HasValue)
        {
            var flowSwitches = (message.state switch
            {
                ProcessStateEnum.COMPLETED => GetFlowSwitchesForCompleted(taskDetail.TaskObject.DecisionId),
                ProcessStateEnum.ACTIVE => GetFlowSwitchesForActive(),
                ProcessStateEnum.TERMINATED => GetFlowSwitchesForTerminated(),
                _ => throw new NotImplementedException("Unknown STATE")
            }).ToList();

            await _salesArrangementService.SetFlowSwitches(productSaleArrangementId.Value, flowSwitches);
        }
    }
    
    private bool initialValidations(IndividualPricingProcessChanged message)
    {
        if (!int.TryParse(message.currentTask.id, out _taskIdSB))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.currentTask.id);
            return false;
        }

        if (!int.TryParse(message.id, out _taskId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.id);
            return false;
        }

        if (!long.TryParse(message.@case.caseId.id, out _caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(IndividualPricingProcessChangedHandler), message.@case.caseId.id);
            return false;
        }

        if (message.state is not (ProcessStateEnum.ACTIVE or ProcessStateEnum.TERMINATED or ProcessStateEnum.COMPLETED))
        {
            _logger.KafkaHandlerSkippedDueToState(nameof(IndividualPricingProcessChangedHandler), _caseId, _taskIdSB, message.state.ToString());
            return false;
        }

        var caseState = _dbContext.Cases.FirstOrDefault(t => t.CaseId == _caseId)?.State;
        if (!caseState.HasValue)
        {
            _logger.KafkaCaseIdNotFound(nameof(IndividualPricingProcessChangedHandler), _caseId);
            return false;
        }
        else
        {
            _caseState = (EnumCaseStates)caseState.Value;
        }

        return true;
    }

    private async Task<bool> afterValidations(IndividualPricingProcessChanged message, GetTaskDetailResponse taskDetail)
    {
        if (taskDetail.TaskObject.ProcessTypeId == 1)
        {
            if (CaseHelpers.IsCaseInState(CaseHelpers.AllExceptInProgressStates, _caseState))
            {
                _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 1);
                return false;
            }
        }
        else
        {
            if (!long.TryParse(message.currentParentProcess.id, out long saProcessId))
            {
                _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 2);
                return false;
            }

            var p = (await getSalesArrangements()).FirstOrDefault(t => t.ProcessId == saProcessId);
            if (p is null)
            {
                _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 3);
                return false;
            }
            else if (p.SalesArrangementTypeId is 13 or 14)
            {
                var saToCheck = await _salesArrangementService.GetSalesArrangement(p.SalesArrangementId);
                if (saToCheck.Retention?.ManagedByRC2.GetValueOrDefault() ?? saToCheck.Refixation?.ManagedByRC2.GetValueOrDefault() ?? false)
                {
                    _logger.KafkaIndividualPricingProcessChangedSkipped(_caseId, _taskIdSB, taskDetail.TaskObject.ProcessTypeId, 4);
                    return false;
                }
            }
        }

        return true;
    }

    private async Task<List<SalesArrangement>> getSalesArrangements()
    {
        _salesArrangementList ??= (await _salesArrangementService.GetSalesArrangementList(_caseId)).SalesArrangements.ToList();
        return _salesArrangementList;
    }

    private async Task saveEntity(long taskId, int taskIdSB, long caseId, DateTime date, bool confirmed)
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
        
        existingConfirmedEntity.ConfirmedDate = confirmed ? DateOnly.FromDateTime(date) : null;
        existingConfirmedEntity.DeclinedDate = !confirmed ? DateOnly.FromDateTime(date) : null;
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
    private int _taskIdSB;
    private int _taskId;
    private long _caseId;
    private EnumCaseStates _caseState;
}