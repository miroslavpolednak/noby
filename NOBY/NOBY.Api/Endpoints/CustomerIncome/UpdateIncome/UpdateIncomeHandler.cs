using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

internal sealed class UpdateIncomeHandler(ICustomerOnSAServiceClient _customerService)
        : IRequestHandler<CustomerIncomeUpdateIncomeRequest>
{
    public async Task Handle(CustomerIncomeUpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        var model = new _HO.UpdateIncomeRequest
        {
            IncomeId = request.IncomeId,
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

        await _customerService.UpdateIncome(model, cancellationToken);
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };
}
