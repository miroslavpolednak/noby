namespace FOMS.Api.Endpoints.Cases;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CasesModelConverter
{
    public async Task<Dto.CaseModel> FromContract(DomainServices.CaseService.Contracts.Case model)
    {
		await initCodebooks();

		return convert(model);
	}

	public async Task<List<Dto.CaseModel>> FromContracts(IEnumerable<DomainServices.CaseService.Contracts.Case> models)
    {
		await initCodebooks();
        
		return models.Select(t => convert(t)).ToList();
	}

	private Dto.CaseModel convert(DomainServices.CaseService.Contracts.Case model)
	{
		var converted = new Dto.CaseModel
		{

			CaseId = model.CaseId,
			State = (CIS.Foms.Enums.CaseStates)model.State,
			StateName = _caseStates.First(x => x.Id == model.State).Name,
			StateUpdated = model.StateUpdatedOn,
			ContractNumber = model.Data.ContractNumber,
			TargetAmount = model.Data.TargetAmount,
			CreatedBy = model.Created.UserName,
			CreatedTime = model.Created.DateTime,
			ProductName = _productTypes.First(x => x.Id == model.Data.ProductTypeId).Name,
			DateOfBirth = model.Customer?.DateOfBirthNaturalPerson,
			CustomerIdentity = model.Customer?.Identity,
			FirstName = model.Customer?.FirstNameNaturalPerson,
			LastName = model.Customer?.Name
		};

		if (model.ActiveTasks is not null && model.ActiveTasks.Any())
		{
			converted.ActiveTasks = model.ActiveTasks
				.Join(_taskTypes, i => i.TaskTypeId, o => o.Id, (task, i) => i.CategoryId.GetValueOrDefault())
				.GroupBy(k => k)
				.Select(t => new Dto.TaskModel
				{
					CategoryId = t.Key,
					TaskCount = t.Count()
				})
				.ToList();
		}
		//MOCK
		model.ActiveTasks.AddRange(new Google.Protobuf.Collections.RepeatedField<DomainServices.CaseService.Contracts.ActiveTask>
		{
			new DomainServices.CaseService.Contracts.ActiveTask { TaskId = 1, TaskTypeId = 1 },
			new DomainServices.CaseService.Contracts.ActiveTask { TaskId = 2, TaskTypeId = 2 }
		});

		return converted;
	}

	private async Task initCodebooks()
    {
		_productTypes = await _codebookService.ProductTypes();
		_caseStates = await _codebookService.CaseStates();
		_taskTypes = await _codebookService.WorkflowTaskTypes();
	}

	private List<DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem> _productTypes;
	private List<DomainServices.CodebookService.Contracts.Endpoints.CaseStates.CaseStateItem> _caseStates;
	private List<DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskTypes.WorkflowTaskTypeItem> _taskTypes;

	private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CasesModelConverter(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _codebookService = codebookService;
    }
}
