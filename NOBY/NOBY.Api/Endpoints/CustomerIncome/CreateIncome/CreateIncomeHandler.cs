using DomainServices.HouseholdService.Clients.v1;
using NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

internal sealed class CreateIncomeHandler(
    ICustomerOnSAServiceClient _customerService,
    FlowSwitchAtLeastOneIncomeMainHouseholdService _flowSwitchMainHouseholdService)
        : IRequestHandler<CustomerIncomeCreateIncomeRequest, int>
{
    public async Task<int> Handle(CustomerIncomeCreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var model = new _HO.CreateIncomeRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            IncomeTypeId = (int)request.IncomeTypeId,
            BaseData = new _HO.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum
            }
        };

        // detail prijmu
        switch (request.IncomeTypeId)
        {
            case EnumIncomeTypes.Employement when request.Data?.Employment is not null:
                model.Employement = request.Data.Employment.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Other when request.Data?.Other is not null:
                model.Other = request.Data.Other.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Entrepreneur when request.Data?.Entrepreneur is not null:
                model.Entrepreneur = request.Data.Entrepreneur.ToDomainServiceRequest();
                break;

            case EnumIncomeTypes.Rent:
                // RENT nema zadna data
                model.Rent = new _HO.IncomeDataRent();
                break;

            default:
                throw new NotImplementedException($"IncomeType {request.IncomeTypeId} cast to domain service is not implemented");
        }

        int incomeId = await _customerService.CreateIncome(model, cancellationToken);

        await _flowSwitchMainHouseholdService.SetFlowSwitchByCustomerOnSAId(request.CustomerOnSAId.Value, cancellationToken: cancellationToken);

        return incomeId;
    }
}
