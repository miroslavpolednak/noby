namespace NOBY.Api.Endpoints.Cases;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class CasesModelConverter(DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService)
{
    public async Task<CasesSharedCaseModel> FromContract(DomainServices.CaseService.Contracts.Case model)
    {
		return await convert(model);
	}

	public async Task<List<CasesSharedCaseModel>> FromContracts(IEnumerable<DomainServices.CaseService.Contracts.Case> models)
    {
        List<CasesSharedCaseModel> result = [];

		foreach (var p in models)
		{
			result.Add(await convert(p));
		}
		return result;
	}

	private async Task<CasesSharedCaseModel> convert(DomainServices.CaseService.Contracts.Case model)
	{
        var productTypes = await _codebookService.ProductTypes();
        var caseStates = await _codebookService.CaseStates();
        var taskTypes = await _codebookService.WorkflowTaskTypes();

        var converted = new CasesSharedCaseModel
		{
			OfferContacts = new(),
			CaseId = model.CaseId,
			State = (CasesSharedCaseModelState)model.State,
			StateName = caseStates.First(x => x.Id == model.State).Name,
			StateUpdated = model.StateUpdatedOn,
			ContractNumber = model.Data.ContractNumber,
			TargetAmount = model.Data.TargetAmount,
			CreatedBy = model.Created.UserName,
			CreatedTime = model.Created.DateTime,
			ProductName = productTypes.First(x => x.Id == model.Data.ProductTypeId).Name,
			CustomerIdentity = model.Customer?.Identity,
			FirstName = model.Customer?.FirstNameNaturalPerson,
			LastName = model.Customer?.Name
		};

		if (model.Customer?.DateOfBirthNaturalPerson is not null)
		{
			converted.DateOfBirth = DateOnly.FromDateTime(model.Customer.DateOfBirthNaturalPerson);
        }

        if (!string.IsNullOrEmpty(model.OfferContacts?.EmailForOffer))
            converted.OfferContacts.EmailAddress = new() { EmailAddress = model.OfferContacts?.EmailForOffer ?? "" };

        if (!string.IsNullOrEmpty(model.OfferContacts?.PhoneNumberForOffer?.PhoneNumber))
            converted.OfferContacts.MobilePhone = new()
            {
                PhoneNumber = model.OfferContacts.PhoneNumberForOffer.PhoneNumber,
                PhoneIDC = model.OfferContacts.PhoneNumberForOffer.PhoneIDC
            };

        if (model.Tasks is not null && model.Tasks.Count != 0)
		{
			converted.ActiveTasks = model.Tasks.Where(t => t.TaskTypeId != 5 && t.TaskTypeId != 8)
				.Join(taskTypes, i => i.TaskTypeId, o => o.Id, (task, i) => i.Id)
				.GroupBy(k => k)
				.Select(t => new CasesSharedTaskModel
                {
					CategoryId = t.Key,
					TaskCount = t.Count()
				})
				.ToList();
		}

		return converted;
	}
}
