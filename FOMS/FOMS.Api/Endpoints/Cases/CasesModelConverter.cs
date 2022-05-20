namespace FOMS.Api.Endpoints.Cases;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CasesModelConverter
{
    public async Task<Dto.CaseModel> FromContract(DomainServices.CaseService.Contracts.Case model)
    {
		var productTypes = await _codebookService.ProductTypes();
		var caseStates = await _codebookService.CaseStates();
		
		return convert(model, productTypes, caseStates);
	}

	public async Task<List<Dto.CaseModel>> FromContracts(IEnumerable<DomainServices.CaseService.Contracts.Case> models)
    {
		var productTypes = await _codebookService.ProductTypes();
		var caseStates = await _codebookService.CaseStates();

		var list = models.Select(t => convert(t, productTypes, caseStates)).ToList();

		//TODO docasne mock
		list.ForEach(t => t.ActiveTasks = new List<Dto.TaskModel>
		{
			new Dto.TaskModel { CategoryId = 1, TaskCount = 2 }
		});

		return list;
	}

	static Dto.CaseModel convert(DomainServices.CaseService.Contracts.Case model, 
		List<DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem> productTypes,
		List<DomainServices.CodebookService.Contracts.Endpoints.CaseStates.CaseStateItem> caseStates)
		=> new()
        {
			
			CaseId = model.CaseId,
			State = (CIS.Foms.Enums.CaseStates)model.State,
			StateName = caseStates.First(x => x.Id == model.State).Name,
			StateUpdated = model.StateUpdatedOn,
			ContractNumber = model.Data.ContractNumber,
			TargetAmount = model.Data.TargetAmount,
			CreatedBy = model.Created.UserName,
			CreatedTime = model.Created.DateTime,
			ProductName = productTypes.First(x => x.Id == model.Data.ProductTypeId).Name,
			DateOfBirth = model.Customer?.DateOfBirthNaturalPerson,
			CustomerIdentity = model.Customer?.Identity,
			FirstName = model.Customer?.FirstNameNaturalPerson,
			LastName = model.Customer?.Name,
			ActiveTasks = model.ActiveTasks is null || !model.ActiveTasks.Any() ? null : model.ActiveTasks.Select(x => new Dto.TaskModel { CategoryId = x.CategoryId, TaskCount = x.TaskCount }).ToList()
		};

	private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CasesModelConverter(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
    }
}
