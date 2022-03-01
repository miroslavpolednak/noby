using DomainServices.SalesArrangementService.Abstraction;
using FOMS.Api.Endpoints.Household.Dto;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.SaveHouseholds;

internal class SaveHouseholdsHandler
: IRequestHandler<SaveHouseholdsRequest, List<int>>
{
    public async Task<List<int>> Handle(SaveHouseholdsRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(SaveHouseholdsHandler), request.SalesArrangementId);

        // docist aktualne ulozene domacnosti
        var households = ServiceCallResult.Resolve<List<contracts.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        _logger.FoundItems(households.Count, nameof(Household));
        
        // ID vsech jiz existujicich domacnosti poslanych z FE
        var newHouseholdIds = request.Households?.Where(t => t.Id.HasValue).Select(t => t.Id!.Value).ToArray() ?? Array.Empty<int>();
        
        // smazat puvodni smazane householdy
        foreach (var h in households.Where(t => !newHouseholdIds.Contains(t.HouseholdId)))
        {
            ServiceCallResult.Resolve(await _householdService.DeleteHousehold(h.HouseholdId, cancellationToken));
        }
        
        if (request.Households is not null)
        {
            // vytvorit nove householdy
            foreach (var h in request.Households)
            {
                if (h.Id.GetValueOrDefault() == 0)
                {
                    var householdRequest = _mapper.MapToRequest(request.SalesArrangementId, h);

                    if (h.Customers is not null)
                    {
                        for (int c = 0; c < h.Customers.Count; c++)
                        {
                            int customerId = await createOrUpdateCustomer(request.SalesArrangementId, h.Customers[c], cancellationToken);
                            if (c == 0)
                                householdRequest.CustomerOnSAId1 = customerId;
                            else
                                householdRequest.CustomerOnSAId2 = customerId;
                        }
                    }

                    // zalozit household
                    h.Id = ServiceCallResult.Resolve<int>(await _householdService.CreateHousehold(householdRequest, cancellationToken));
                }
                else // update stavajicich
                {
                    var householdRequest = _mapper.MapToRequest(h);
            
                    if (h.Customers is not null)
                    {
                        for (int c = 0; c < h.Customers.Count; c++)
                        {
                            int customerId = await createOrUpdateCustomer(request.SalesArrangementId, h.Customers[c], cancellationToken);
                            if (c == 0)
                                householdRequest.CustomerOnSAId1 = customerId;
                            else
                                householdRequest.CustomerOnSAId2 = customerId;
                        }
                    }
            
                    // update household
                    ServiceCallResult.Resolve(await _householdService.UpdateHousehold(householdRequest, cancellationToken));
                }
            }

            return request.Households.Select(t => t.Id!.Value).ToList();
        }
        else
            return new List<int>(0);
    }

    private async Task<int> createOrUpdateCustomer(int salesArrangementId, CustomerInHousehold customer, CancellationToken cancellationToken)
    {
        if (customer.Id.GetValueOrDefault() > 0)
        {
            ServiceCallResult.Resolve(await _customerOnSAService.UpdateCustomer(_mapper.MapToRequest(customer), cancellationToken));
            return customer.Id!.Value;
        }
        else
            return ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(_mapper.MapToRequest(salesArrangementId, customer), cancellationToken));
    }

    private readonly Mapper _mapper;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<SaveHouseholdsHandler> _logger;
    
    public SaveHouseholdsHandler(
        Mapper mapper,
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<SaveHouseholdsHandler> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}