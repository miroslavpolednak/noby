using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

internal class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, GetIncomeResponse>
{
    public async Task<GetIncomeResponse> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var incomeInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.Income>(await _customerService.GetIncome(request.IncomeId, cancellationToken));

        return new GetIncomeResponse
        {
            CurrencyCode = incomeInstance.BaseData.CurrencyCode,
            IncomeTypeId = incomeInstance.IncomeTypeId,
            Sum = incomeInstance.BaseData.Sum,
            Data = getData(incomeInstance)
        };
    }

    static object? getData(_SA.Income incomeInstance)
        => incomeInstance.DataCase switch
        {
            _SA.Income.DataOneofCase.Employement => incomeInstance.Employement.ToApiResponse(),
            _SA.Income.DataOneofCase.Other => incomeInstance.Other.ToApiResponse(),
            _SA.Income.DataOneofCase.Entrepreneur => incomeInstance.Entrepreneur.ToApiResponse(),
            _ => null
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
