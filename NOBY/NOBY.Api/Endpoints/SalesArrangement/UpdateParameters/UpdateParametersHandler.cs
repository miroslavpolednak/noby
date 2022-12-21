using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal class UpdateParametersHandler
    : AsyncRequestHandler<UpdateParametersRequest>
{
    protected override async Task Handle(UpdateParametersRequest request, CancellationToken cancellationToken)
    {
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var updateRequest = new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = request.SalesArrangementId
        };

        if (request.Parameters is not null)
        {
            string dataString = ((System.Text.Json.JsonElement)request.Parameters).GetRawText();

            switch (saInstance.SalesArrangementTypeId)
            {
                case >= 1 and <= 5:
                    var o1 = System.Text.Json.JsonSerializer.Deserialize<Dto.ParametersMortgage>(dataString, _jsonSerializerOptions);
                    if (o1 is not null)
                        updateRequest.Mortgage = o1.ToDomainService();
                    break;

                case 6:
                    var o2 = System.Text.Json.JsonSerializer.Deserialize<Dto.ParametersDrawing>(dataString, _jsonSerializerOptions);
                    if (o2 is not null)
                        updateRequest.Drawing = o2.ToDomainService();
                    break;

                default:
                    throw new NotImplementedException($"SalesArrangementTypeId {saInstance.SalesArrangementTypeId} parameters model cast to domain service is not implemented");
            }
        }

        await _salesArrangementService.UpdateSalesArrangementParameters(updateRequest, cancellationToken);
    }

    static System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true
    };

    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public UpdateParametersHandler(
        ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
