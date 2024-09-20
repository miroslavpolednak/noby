using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients.v1;

namespace NOBY.Api.Endpoints.Household.GetHouseholds;

internal sealed class GetHouseholdsHandler(
    IHouseholdServiceClient _householdService,
    IDocumentOnSAServiceClient _documentOnSAService,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetHouseholdsRequest, List<HouseholdInList>>
{
    public async Task<List<HouseholdInList>> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        // vsechny households
        var households = await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken);

        var householdTypes = await _codebookService.HouseholdTypes(cancellationToken);

        var documentsOnSa = (await _documentOnSAService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken)).DocumentsOnSA;

        return households
            .Select(t => new HouseholdInList
            {
                HouseholdId = t.HouseholdId,
                HouseholdTypeId = t.HouseholdTypeId,
                HouseholdTypeName = householdTypes.First(x => x.Id == t.HouseholdTypeId).Name,
                IsNewSigningRequired = documentsOnSa.Any(document => document.HouseholdId == t.HouseholdId && document is { IsValid: true, IsSigned: true })
            })
            .OrderBy(t => t.HouseholdTypeId)
            .ToList();
    }
}