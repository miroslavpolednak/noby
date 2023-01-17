using Grpc.Core;

namespace CIS.InternalServices.DataAggregatorService.Api.Endpoints;

[Microsoft.AspNetCore.Authorization.Authorize]
internal class DataAggregatorServiceGrpc : Contracts.V1.DataAggregatorService.DataAggregatorServiceBase
{
    private readonly IMediator _mediator;

    public DataAggregatorServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<GetDocumentDataResponse> GetDocumentData(GetDocumentDataRequest request, ServerCallContext context) => 
        _mediator.Send(request, context.CancellationToken);

    public override Task<GetEasFormResponse> GetEasForm(GetEasFormRequest request, ServerCallContext context) => 
        _mediator.Send(request, context.CancellationToken);
}