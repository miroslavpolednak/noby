using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

internal class UpdateIncomeHandler
    : AsyncRequestHandler<UpdateIncomeRequest>
{
    protected override async Task Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeHandler), request.IncomeId);

        var incomeInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.Income>(await _customerService.GetIncome(request.IncomeId, cancellationToken));

        var model = new DomainServices.SalesArrangementService.Contracts.UpdateIncomeRequest
        {
            IncomeId = request.IncomeId,
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

            switch ((CIS.Foms.Enums.CustomerIncomeTypes)incomeInstance.IncomeTypeId)
            {
                case CIS.Foms.Enums.CustomerIncomeTypes.Employement:
                    var o = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataEmployement>(dataString, _jsonSerializerOptions);
                    if (o is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Employement = o.ToDomainServiceRequest();
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

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<UpdateIncomeHandler> _logger;

    public UpdateIncomeHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ILogger<UpdateIncomeHandler> logger)
    {
        _logger = logger;
        _customerService = customerService;
    }
}
