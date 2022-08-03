using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.CreateIncome;

internal sealed class CreateIncomeHandler
    : IRequestHandler<CreateIncomeRequest, int>
{
    public async Task<int> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var model = new DomainServices.SalesArrangementService.Contracts.CreateIncomeRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            IncomeTypeId = (int)request.IncomeTypeId,
            BaseData = new DomainServices.SalesArrangementService.Contracts.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum
            }
        };

        // detail prijmu
        if (request.Data is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Data).GetRawText();

            switch (request.IncomeTypeId)
            {
                case CIS.Foms.Enums.CustomerIncomeTypes.Employement:
                    var o1 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataEmployement>(dataString, _jsonSerializerOptions);
                    if (o1 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Employement = o1.ToDomainServiceRequest();
                    break;

                case CIS.Foms.Enums.CustomerIncomeTypes.Other:
                    var o2 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataOther>(dataString, _jsonSerializerOptions);
                    if (o2 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Other = o2.ToDomainServiceRequest();
                    break;

                case CIS.Foms.Enums.CustomerIncomeTypes.Enterprise:
                    var o3 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataEntrepreneur>(dataString, _jsonSerializerOptions);
                    if (o3 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Entrepreneur = o3.ToDomainServiceRequest();
                    break;

                default:
                    throw new NotImplementedException($"IncomeType {request.IncomeTypeId} cast to domain service is not implemented");
            }
        }

        int incomeId = ServiceCallResult.ResolveAndThrowIfError<int>(await _customerService.CreateIncome(model, cancellationToken));
        return incomeId;
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private readonly ICustomerOnSAServiceAbstraction _customerService;

    public CreateIncomeHandler(ICustomerOnSAServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
