using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.SalesArrangementService.Contracts;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHouseholds;

internal class GetHouseholdsHandler
    : IRequestHandler<GetHouseholdsRequest, List<Dto.Household>>
{
    public async Task<List<Dto.Household>> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdsHandler), request.SalesArrangementId);

        // vsechny households
        var households = ServiceCallResult.Resolve<List<contracts.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        _logger.FoundItems(households.Count, nameof(Household));
        
        // dotahnout customers
        var customersOnSA = ServiceCallResult.Resolve<List<CustomerOnSA>>(await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellationToken));
        _logger.FoundItems(customersOnSA.Count, nameof(CustomerOnSA));

        return await _mapper.MapToResponse(households, customersOnSA);
    }
    
    private readonly Mapper _mapper;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<GetHouseholdsHandler> _logger;
    
    public GetHouseholdsHandler(
        IHouseholdServiceAbstraction householdService, 
        ICustomerOnSAServiceAbstraction customerOnSAService,
        Mapper mapper, 
        ILogger<GetHouseholdsHandler> logger)
    {
        _logger = logger;
        _customerOnSAService = customerOnSAService;
        _mapper = mapper;
        _householdService = householdService;
    }
}