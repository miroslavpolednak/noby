using DomainServices.CodebookService.Contracts.v1;
using SharedTypes.GrpcTypes;

namespace DomainServices.CodebookService.Api.Endpoints.v1;

internal partial class CodebookService
{
    public override Task<GenericCodebookResponse> ExtraPaymentReasons(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GetBankingDaysResponse> GetBankingDays(GetBankingDaysRequest request, ServerCallContext context)
    {
        var items = _db.GetList<DateTime>(nameof(GetBankingDays), new { dateFrom = (DateTime)request.DateFrom, dateTo = (DateTime)request.DateTo });

        GetBankingDaysResponse response = new();
        response.NonBankingDays.AddRange(items.Select(t => (GrpcDate)t));
        return Task.FromResult(response);
    }
}
