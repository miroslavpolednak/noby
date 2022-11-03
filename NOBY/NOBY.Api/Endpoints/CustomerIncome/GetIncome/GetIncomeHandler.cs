using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, GetIncomeResponse>
{
    public async Task<GetIncomeResponse> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var incomeInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.Income>(await _customerService.GetIncome(request.IncomeId, cancellationToken));

        return new GetIncomeResponse
        {
            CurrencyCode = incomeInstance.BaseData.CurrencyCode,
            IncomeTypeId = incomeInstance.IncomeTypeId,
            Sum = incomeInstance.BaseData.Sum,
            Data = getData(incomeInstance)
        };
    }

    static object? getData(_HO.Income incomeInstance)
        => incomeInstance.DataCase switch
        {
            _HO.Income.DataOneofCase.Employement => incomeInstance.Employement.ToApiResponse(),
            _HO.Income.DataOneofCase.Other => incomeInstance.Other.ToApiResponse(),
            _HO.Income.DataOneofCase.Entrepreneur => incomeInstance.Entrepreneur.ToApiResponse(),
            _ => null
        };

    private readonly ICustomerOnSAServiceClient _customerService;
    private readonly ILogger<GetIncomeHandler> _logger;

    public GetIncomeHandler(
        ICustomerOnSAServiceClient customerService,
        ILogger<GetIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
