using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.CreateHousehold;

internal class CreateHouseholdHandler
    : IRequestHandler<CreateHouseholdRequest, Dto.HouseholdInList>
{
    public async Task<Dto.HouseholdInList> Handle(CreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        // nazev typu domacnosti
        string householdTypeName = (await _codebookService.HouseholdTypes(cancellationToken)).FirstOrDefault(x => x.Id == request.HouseholdTypeId)?.Name ??
            throw new CisNotFoundException(ErrorCodes.HouseholdTypeNotFound, nameof(CIS.Foms.Enums.HouseholdTypes), request.HouseholdTypeId);

        // vytvorit domacnost
        var requestModel = new _HO.CreateHouseholdRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            HouseholdTypeId = request.HouseholdTypeId
        };
        int householdId = await _householdService.CreateHousehold(requestModel, cancellationToken);

        // vytvorit customera
        var customerResponse = await _customerOnSAService.CreateCustomer(new _HO.CreateCustomerRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            CustomerRoleId = request.HouseholdTypeId,
            Customer = new _HO.CustomerOnSABase()
        }, cancellationToken);

        // vlozit customera na household
        await _householdService.UpdateHousehold(new _HO.UpdateHouseholdRequest
        {
            HouseholdId = householdId,
            CustomerOnSAId1 = customerResponse.CustomerOnSAId
        }, cancellationToken);

        return new Dto.HouseholdInList
        {
            HouseholdId = householdId,
            HouseholdTypeId = request.HouseholdTypeId,
            HouseholdTypeName = householdTypeName
        };
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public CreateHouseholdHandler(
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService,
        ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
    }
}
