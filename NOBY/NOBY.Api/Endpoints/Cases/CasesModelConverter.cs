using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Cases;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class CasesModelConverter
{
    public async Task<SharedDto.CaseModel> FromContract(DomainServices.CaseService.Contracts.Case model)
    {
		await initCodebooks();

		return convert(model);
	}

	public async Task<List<SharedDto.CaseModel>> FromContracts(IEnumerable<DomainServices.CaseService.Contracts.Case> models)
    {
		await initCodebooks();
        
		return models.Select(t => convert(t)).ToList();
	}

	private SharedDto.CaseModel convert(DomainServices.CaseService.Contracts.Case model)
	{
		var converted = new SharedDto.CaseModel
		{
			OfferContacts = new(),
			CaseId = model.CaseId,
			State = (SharedTypes.Enums.CaseStates)model.State,
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

        if (!string.IsNullOrEmpty(model.OfferContacts?.EmailForOffer))
            converted.OfferContacts.EmailAddress = new() { EmailAddress = model.OfferContacts?.EmailForOffer };

        if (!string.IsNullOrEmpty(model.OfferContacts?.PhoneNumberForOffer?.PhoneNumber))
            converted.OfferContacts.MobilePhone = new()
            {
                PhoneNumber = model.OfferContacts.PhoneNumberForOffer.PhoneNumber,
                PhoneIDC = model.OfferContacts.PhoneNumberForOffer.PhoneIDC
            };

        if (model.Tasks is not null && model.Tasks.Count != 0)
		{
			converted.ActiveTasks = model.Tasks.Where(t => t.TaskTypeId != 5 && t.TaskTypeId != 8)
				.Join(_taskTypes, i => i.TaskTypeId, o => o.Id, (task, i) => i.Id)
				.GroupBy(k => k)
				.Select(t => new SharedDto.TaskModel
				{
					CategoryId = t.Key,
					TaskCount = t.Count()
				})
				.ToList();
		}

		return converted;
	}

	private async Task initCodebooks()
    {
		_productTypes = await _codebookService.ProductTypes();
		_caseStates = await _codebookService.CaseStates();
		_taskTypes = await _codebookService.WorkflowTaskTypes();
	}

	private List<ProductTypesResponse.Types.ProductTypeItem> _productTypes;
	private List<GenericCodebookResponse.Types.GenericCodebookItem> _caseStates;
	private List<GenericCodebookResponse.Types.GenericCodebookItem> _taskTypes;

	private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CasesModelConverter(DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _codebookService = codebookService;
    }
}
