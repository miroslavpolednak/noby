using CIS.Core.Attributes;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.ApiServices;

[ScopedService, SelfService]
internal sealed class MortgageRefinancingSalesArrangementCreateService(
	ISalesArrangementServiceClient _salesArrangementService,
	IMediator _mediator)
{
	public async Task<DomainServices.SalesArrangementService.Contracts.SalesArrangement> GetOrCreateSalesArrangement(
		long caseId, 
		SalesArrangementTypes salesArrangementType, 
		CancellationToken cancellationToken)
	{
		// zjistit zda existuje aktivni SA refixacni
		var saList = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
		int? saId = saList
			.SalesArrangements
			.FirstOrDefault(sa => sa.SalesArrangementTypeId == (int)salesArrangementType && sa.State is ((int)SharedTypes.Enums.EnumSalesArrangementStates.NewArrangement or (int)SharedTypes.Enums.EnumSalesArrangementStates.InProgress))
			?.SalesArrangementId;

		// pokud SA neexistuje
		if (!saId.HasValue)
		{
			var createdSAId = await _mediator.Send(new SalesArrangementCreateSalesArrangementRequest
			{
				CaseId = caseId,
				SalesArrangementTypeId = (int)salesArrangementType
			}, cancellationToken);

			saId = createdSAId.SalesArrangementId;
		}

		return await _salesArrangementService.GetSalesArrangement(saId.Value, cancellationToken);
	}
}
