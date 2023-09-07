using Grpc.Core;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints;

[Microsoft.AspNetCore.Authorization.Authorize]
internal class DataAggregatorServiceGrpc : Contracts.V1.DataAggregatorService.DataAggregatorServiceBase
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DataAggregatorServiceGrpc(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public override Task<GetDocumentDataResponse> GetDocumentData(GetDocumentDataRequest request, ServerCallContext context) => 
        SendViaMediatr(request, context.CancellationToken);

    public override Task<GetEasFormResponse> GetEasForm(GetEasFormRequest request, ServerCallContext context) => 
        SendViaMediatr(request, context.CancellationToken);

    public override Task<GetRiskLoanApplicationDataResponse> GetRiskLoanApplicationData(GetRiskLoanApplicationDataRequest request, ServerCallContext context) =>
        SendViaMediatr(request, context.CancellationToken);

    private async Task<TResponse> SendViaMediatr<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediatr.Send(request, cancellationToken);
    }
}