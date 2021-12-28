namespace FOMS.Api.Endpoints.Case;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseModelConverter
{
    public async Task<Dto.CaseModel> FromContract(DomainServices.CaseService.Contracts.CaseModel model)
    {
		var productTypes = await _codebookService.ProductInstanceTypes();
		var caseStates = await _codebookService.CaseStates();
		
		return convert(model, productTypes, caseStates);
	}

	public async Task<List<Dto.CaseModel>> FromContracts(IEnumerable<DomainServices.CaseService.Contracts.CaseModel> models)
    {
		var productTypes = await _codebookService.ProductInstanceTypes();
		var caseStates = await _codebookService.CaseStates();

		return models.Select(t => convert(t, productTypes, caseStates)).ToList();
	}

	private Dto.CaseModel convert(DomainServices.CaseService.Contracts.CaseModel model, 
		List<DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeItem> productTypes,
		List<DomainServices.CodebookService.Contracts.Endpoints.CaseStates.CaseStateItem> caseStates)
		=> new Dto.CaseModel
        {
			CaseId = model.CaseId,
			State = model.State,
			StateName = caseStates.First(x => x.Id == model.State).Name,
			ActionRequired = model.ActionRequired,
			ContractNumber = model.ContractNumber,
			TargetAmount = model.TargetAmount,
			CreatedBy = model.Created.UserName,
			CreatedTime = model.Created.DateTime,
			DateOfBirth = model.DateOfBirthNaturalPerson,
			ProductName = productTypes.First(x => x.Id == model.ProductInstanceType).Name,
			FirstName = model.FirstNameNaturalPerson,
			LastName = model.Name
		};

	private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CaseModelConverter(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
    }
}
