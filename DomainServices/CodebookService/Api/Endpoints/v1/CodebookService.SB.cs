using DomainServices.CodebookService.Contracts.v1;
using SharedTypes.GrpcTypes;

namespace DomainServices.CodebookService.Api.Endpoints.v1;

internal partial class CodebookService
{
    public override Task<BuildingSavingsMarketingActionsResponse> BuildingSavingsMarketingActions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<BuildingSavingsMarketingActionsResponse, BuildingSavingsMarketingActionsResponse.Types.BuildingSavingsMarketingActionsItem>(nameof(BuildingSavingsMarketingActions));

    public override Task<BuildingSavingsPropertiesResponse> BuildingSavingsProperties(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems< BuildingSavingsPropertiesResponse, BuildingSavingsPropertiesResponse.Types.BuildingSavingsPropertiesItem>(nameof(BuildingSavingsProperties));
    
    public override Task<GenericCodebookResponse> ExtraPaymentReasons(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GetNonBankingDaysResponse> GetNonBankingDays(GetNonBankingDaysRequest request, ServerCallContext context)
    {
        var items = _db.GetList<DateTime>(nameof(GetNonBankingDays), new { dateFrom = (DateTime)request.DateFrom, dateTo = (DateTime)request.DateTo });

        GetNonBankingDaysResponse response = new();
        response.NonBankingDays.AddRange(items.Select(t => (GrpcDate)t));
        return Task.FromResult(response);
    }
}
