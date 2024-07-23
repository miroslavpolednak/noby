namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler(IMpHomeClient _mpHomeClient) 
    : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
	public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        var (covenants, _) = await _mpHomeClient.GetCovenants(request.CaseId, cancellationToken);

		var covenant = covenants?.FirstOrDefault(t => t.SequenceNumber == request.Order)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12024);

        return new GetCovenantDetailResponse
        {
            Description = covenant.TextExplanatoryDocument ?? string.Empty,
            Name = covenant.TextNameForClient ?? string.Empty,
            Text = covenant.TextLoanContract ?? string.Empty,
            FulfillDate = covenant.DueDate,
            IsFulfilled = (covenant.DoneFlag ?? 0 ) != 0
        };
    }
}