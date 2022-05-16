using DomainServices.CodebookService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.Household.CreateHousehold;

internal class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdRequest, Dto.HouseholdInList>
{
    public async Task<Dto.HouseholdInList> Handle(CreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        // nazev typu domacnosti
        string householdTypeName = (await _codebookService.HouseholdTypes(cancellationToken)).FirstOrDefault(x => x.Id == request.HouseholdTypeId)?.Name ??
            throw new CisNotFoundException(Core.ErrorCodes.HouseholdTypeNotFound, nameof(CIS.Foms.Enums.HouseholdTypes), request.HouseholdTypeId);

        // vytvorit domacnost
        var requestModel = new DomainServices.SalesArrangementService.Contracts.CreateHouseholdRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            HouseholdTypeId = request.HouseholdTypeId
        };
        int householdId = ServiceCallResult.ResolveAndThrowIfError<int>(await _householdService.CreateHousehold(requestModel, cancellationToken));

        return new Dto.HouseholdInList
        {
            HouseholdId = householdId,
            HouseholdTypeId = request.HouseholdTypeId,
            HouseholdTypeName = householdTypeName
        };
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<CreateHouseholdHandler> _logger;

    public CreateHouseholdHandler(
        IHouseholdServiceAbstraction householdService,
        ICodebookServiceAbstraction codebookService,
        ILogger<CreateHouseholdHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _householdService = householdService;
    }
}
