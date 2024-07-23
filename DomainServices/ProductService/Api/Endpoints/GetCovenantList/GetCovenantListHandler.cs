namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        var (conditions, phases) = await _mpHomeClient.GetCovenants(request.CaseId, cancellationToken);
        
        var mappedCovenants = conditions?.Select(t => new CovenantListItem
        {
            Name = t.TextNameForClient ?? string.Empty,
            FulfillDate = t.DueDate,
            IsFulfilled = (t.DoneFlag ?? 0) != 0,
            Order = t.SequenceNumber,
            OrderLetter = t.ContractTypeOrderLetter ?? string.Empty,
            PhaseOrder = t.PhaseOrder ?? 0,
            CovenantTypeId = t.ContractType ?? 0,
        });

        var mappedPhases = phases?.Select(t => new PhaseListItem
        {
            Name = t.Name ?? string.Empty,
            Order = t.Order,
            OrderLetter = t.OrderLetter ?? string.Empty
        });

		return new GetCovenantListResponse
        {
            Covenants = { mappedCovenants ?? [] },
            Phases = { mappedPhases ?? [] }
        };
	}
}