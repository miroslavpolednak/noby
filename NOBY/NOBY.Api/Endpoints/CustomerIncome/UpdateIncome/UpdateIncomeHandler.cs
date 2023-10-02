﻿using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeRequest>
{
    public async Task Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
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
        if (request.Data is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Data).GetRawText();

            switch (request.IncomeTypeId)
            {
                case SharedTypes.Enums.CustomerIncomeTypes.Employement:
                    var o1 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataEmployement>(dataString, _jsonSerializerOptions);
                    if (o1 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Employement = o1.ToDomainServiceRequest();
                    break;

                case SharedTypes.Enums.CustomerIncomeTypes.Other:
                    var o2 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataOther>(dataString, _jsonSerializerOptions);
                    if (o2 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Other = o2.ToDomainServiceRequest();
                    break;

                case SharedTypes.Enums.CustomerIncomeTypes.Enterprise:
                    var o3 = System.Text.Json.JsonSerializer.Deserialize<Dto.IncomeDataEntrepreneur>(dataString, _jsonSerializerOptions);
                    if (o3 is not null) //TODO kdyz je to null, mam resit nejakou validaci?
                        model.Entrepreneur = o3.ToDomainServiceRequest();
                    break;

                case SharedTypes.Enums.CustomerIncomeTypes.Rent:
                    // RENT nema zadna data
                    model.Rent = new _HO.IncomeDataRent();
                    break;

                default:
                    throw new NotImplementedException($"IncomeType {request.IncomeTypeId} cast to domain service is not implemented");
            }
        }

        await _customerService.UpdateIncome(model, cancellationToken);
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private readonly ICustomerOnSAServiceClient _customerService;

    public UpdateIncomeHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
