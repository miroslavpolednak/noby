using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

internal class UpdateIncomeHandler
    : AsyncRequestHandler<UpdateIncomeRequest>
{
    protected override async Task Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        var incomeInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.Income>(await _customerService.GetIncome(request.IncomeId, cancellationToken));

        var model = new _HO.UpdateIncomeRequest
        {
            IncomeId = request.IncomeId,
            BaseData = new _HO.IncomeBaseData
            {
                CurrencyCode = request.CurrencyCode,
                Sum = request.Sum
            }
        };

        // detail prijmu
        if (request.Data is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Data).GetRawText();

            switch ((CIS.Foms.Enums.CustomerIncomeTypes)incomeInstance.IncomeTypeId)
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

                case CIS.Foms.Enums.CustomerIncomeTypes.Rent:
                    // RENT nema zadna data
                    model.Rent = new _HO.IncomeDataRent();
                    break;

                default:
                    throw new NotImplementedException($"IncomeType {incomeInstance.IncomeTypeId} cast to domain service is not implemented");
            }
        }

        ServiceCallResult.Resolve(await _customerService.UpdateIncome(model, cancellationToken));
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private readonly ICustomerOnSAServiceClient _customerService;
    private readonly ILogger<UpdateIncomeHandler> _logger;

    public UpdateIncomeHandler(
        ICustomerOnSAServiceClient customerService,
        ILogger<UpdateIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
