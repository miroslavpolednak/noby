using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

internal class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, object>
{
    public async Task<object> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncomeHandler), request.IncomeId);

        var incomeInstance = ServiceCallResult.Resolve<_SA.Income>(await _customerService.GetIncome(request.IncomeId, cancellationToken));

        return getData(incomeInstance);
    }

    static object getData(_SA.Income incomeInstance)
        => incomeInstance.DataCase switch
        {
            _SA.Income.DataOneofCase.Employement => incomeInstance.Employement.ToApiResponse(),
            _ => throw new NotImplementedException()
        };

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<GetIncomeHandler> _logger;

    public GetIncomeHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ILogger<GetIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
