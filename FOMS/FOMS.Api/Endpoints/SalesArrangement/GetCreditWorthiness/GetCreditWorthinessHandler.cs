using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal class GetCreditWorthinessHandler
    : IRequestHandler<GetCreditWorthinessRequest, GetCreditWorthinessResponse>
{
    public async Task<GetCreditWorthinessResponse> Handle(GetCreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCreditWorthinessHandler), request.SalesArrangementId);

        // seznam domacnosti na SA
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_SA.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));

        foreach (var household in households)
        {
            var customer = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSaService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken));
        }
        
        return new GetCreditWorthinessResponse();
    }

    private readonly ILogger<GetCreditWorthinessHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction _householdService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSaService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetCreditWorthinessHandler(
        ILogger<GetCreditWorthinessHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction householdService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSaService)
    {
        _householdService = householdService;
        _customerService = customerService;
        _customerOnSaService = customerOnSaService;
        _logger = logger;
    }
}
