using DomainServices.CaseService.Clients.v1;
using DomainServices.ProductService.Api.Database.Models;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler(
    IMpHomeClient _mpHomeClient,
	ICaseServiceClient _caseService)
    : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var covenants = await _mpHomeClient.GetCovenants(request.CaseId, cancellationToken);
        var covenantPhases = await _repository.GetCovenantPhases(request.CaseId, cancellationToken);

        GetCovenantListResponse response = new();

        if (covenants is not null)
        {
            response.Covenants.AddRange(covenants.Select(t => new CovenantListItem
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

        return response;
    }

    private static PhaseListItem MapCovenantPhase(CovenantPhase covenantPhase) =>
        new()
        {
            Name = covenantPhase.Name ?? string.Empty,
            Order = covenantPhase.Order,
            OrderLetter = covenantPhase.OrderLetter ?? string.Empty
        };
}