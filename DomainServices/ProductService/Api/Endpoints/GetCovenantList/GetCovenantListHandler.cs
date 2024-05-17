using DomainServices.CaseService.Clients.v1;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler(
    IMpHomeClient _mpHomeClient,
	ICaseServiceClient _caseService)
    : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var (conditions, phases) = await _mpHomeClient.GetCovenants(request.CaseId, cancellationToken);
        
        GetCovenantListResponse response = new();

        if (conditions is not null)
        {
            response.Covenants.AddRange(conditions.Select(t => new CovenantListItem
            {
                Name = t.TextNameForClient ?? string.Empty,
                FulfillDate = t.DueDate,
                IsFulfilled = (t.DoneFlag ?? 0) != 0,
                Order = t.SequenceNumber,
                OrderLetter = t.ContractTypeOrderLetter ?? string.Empty,
                PhaseOrder = t.PhaseOrder ?? 0,
                CovenantTypeId = t.ContractType ?? 0,
            }));
        }

        if (phases is not null)
        {
            response.Phases.AddRange(phases.Select(t => new PhaseListItem
            {
                Name = t.Name ?? string.Empty,
                Order = t.Order,
                OrderLetter = t.OrderLetter ?? string.Empty
			}));
        }

        return response;
    }
}