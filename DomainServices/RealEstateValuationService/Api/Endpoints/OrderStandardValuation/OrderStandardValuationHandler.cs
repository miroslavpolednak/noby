using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;
using DomainServices.UserService.Clients;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderStandardValuation;

internal sealed class OrderStandardValuationHandler
    : IRequestHandler<OrderStandardValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(OrderStandardValuationRequest request, CancellationToken cancellationToken)
    {
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly IUserServiceClient _userService;
    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerServiceClient _customerService;

    public OrderStandardValuationHandler(
        IPreorderServiceClient preorderService,
        RealEstateValuationServiceDbContext dbContext,
        IUserServiceClient userService,
        ICaseServiceClient caseService,
        ICustomerServiceClient customerService)
    {
        _preorderService = preorderService;
        _userService = userService;
        _caseService = caseService;
        _customerService = customerService;
        _dbContext = dbContext;
    }
}
