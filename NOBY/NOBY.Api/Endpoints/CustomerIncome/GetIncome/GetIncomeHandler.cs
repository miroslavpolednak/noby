using DomainServices.HouseholdService.Clients.v1;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal sealed class GetIncomeHandler(ICustomerOnSAServiceClient _customerService)
    : IRequestHandler<GetIncomeRequest, CustomerIncomeGetIncomeResponse>
{
    public async Task<CustomerIncomeGetIncomeResponse> Handle(GetIncomeRequest request, CancellationToken cancellationToken)
    {
        var incomeInstance = await _customerService.GetIncome(request.IncomeId, cancellationToken);

        return new CustomerIncomeGetIncomeResponse
        {
            CurrencyCode = incomeInstance.BaseData.CurrencyCode,
            IncomeTypeId = incomeInstance.IncomeTypeId,
            Sum = incomeInstance.BaseData.Sum,
            Data = getData(incomeInstance)
        };
    }

    static CustomerIncomeDataOneOf? getData(_HO.Income incomeInstance)
        => incomeInstance.DataCase switch
        {
            _HO.Income.DataOneofCase.Employement => CustomerIncomeDataOneOf.Create(incomeInstance.Employement.ToApiResponse()),
            _HO.Income.DataOneofCase.Other => CustomerIncomeDataOneOf.Create(incomeInstance.Other.ToApiResponse()),
            _HO.Income.DataOneofCase.Entrepreneur => CustomerIncomeDataOneOf.Create(incomeInstance.Entrepreneur.ToApiResponse()),
            _ => null
        };
}
