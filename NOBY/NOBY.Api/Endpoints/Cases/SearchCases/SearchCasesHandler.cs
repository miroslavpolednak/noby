using CIS.Core.Security;
using CIS.Core.Types;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class SearchCasesHandler(
    ICurrentUserAccessor _userAccessor,
    CasesModelConverter _converter,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<CasesSearchCasesRequest, CasesSearchCasesResponse>
{
    public async Task<CasesSearchCasesResponse> Handle(CasesSearchCasesRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(_sortingMapper);

        var filterStates = getStatesFilter(request.FilterId);

        DomainServices.CaseService.Contracts.SearchCasesResponse result;

        if (filterStates?.Any() ?? true)
            result = await _caseService.SearchCases(paginable, _userAccessor.User!.Id, filterStates, _userAccessor.HasPermission(UserPermissions.CASE_ViewAfterDrawing) ? null : 90, request.Term, cancellationToken);
        else
            result = new DomainServices.CaseService.Contracts.SearchCasesResponse 
            { 
                Pagination = new() 
            };

            // transform
        return new CasesSearchCasesResponse
        {
            Rows = await _converter.FromContracts(result.Cases),
            Pagination = new(request.Pagination as IPaginableRequest ?? paginable, result.Pagination.RecordsTotalSize)
        };
    }

    static List<EnumCaseStates>? getStatesFilter(int? filterId)
        => filterId switch
        {
            1 => [EnumCaseStates.InProgress, EnumCaseStates.InProcessing, EnumCaseStates.InSigning, EnumCaseStates.InDisbursement, EnumCaseStates.InAdministration, EnumCaseStates.InProcessingConfirmed, EnumCaseStates.ToBeCancelled],
            2 => [EnumCaseStates.InProgress, EnumCaseStates.InProcessing, EnumCaseStates.InProcessingConfirmed],
            3 => [EnumCaseStates.InSigning],
            4 => [EnumCaseStates.InDisbursement],
            5 => [EnumCaseStates.InAdministration],
            _ => null
        };

    static readonly List<Paginable.MapperField> _sortingMapper =
    [
        new ("stateUpdated", "StateUpdatedOn"),
        new ("customerName", "Name")
    ];
}
