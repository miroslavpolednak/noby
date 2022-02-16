namespace FOMS.Api.Endpoints.Case;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseModelConverter
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

		return models.Select(t => convert(t, productTypes, caseStates)).ToList();
	}

	private Dto.CaseModel convert(DomainServices.CaseService.Contracts.Case model, 
		List<DomainServices.CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeItem> productTypes,
		List<DomainServices.CodebookService.Contracts.Endpoints.CaseStates.CaseStateItem> caseStates)
		=> new Dto.CaseModel
        {
			CaseId = model.CaseId,
			State = (CIS.Foms.Enums.CaseStates)model.State,
			StateName = caseStates.First(x => x.Id == model.State).Name,
			StateUpdated = model.StateUpdatedOn,
			ActionRequired = model.ActionRequired,
			ContractNumber = model.Data.ContractNumber,
			TargetAmount = model.Data.TargetAmount,
			CreatedBy = model.Created.UserName,
			CreatedTime = model.Created.DateTime,
			DateOfBirth = model.Customer?.DateOfBirthNaturalPerson,
			ProductName = productTypes.First(x => x.Id == model.Data.ProductTypeId).Name,
			FirstName = model.Customer?.FirstNameNaturalPerson,
			LastName = model.Customer?.Name
		};

	private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CaseModelConverter(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
    }
}
