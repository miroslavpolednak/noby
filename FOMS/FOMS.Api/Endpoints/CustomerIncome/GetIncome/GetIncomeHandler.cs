using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

internal class GetIncomeHandler
    : IRequestHandler<GetIncomeRequest, GetIncomeResponse>
{
    public async Task<GetIncomeResponse?> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
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
            _ => null // tohle je asi spravne, protoze data o prijmu nemusi byt vyplnena a v tu chvili nebude naplnen object detailu
            //_ => throw new NotImplementedException($"Income type '{incomeInstance}' is not implemented")
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
