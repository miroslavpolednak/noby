namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal sealed class GetMortgageHandler(IMpHomeClient _mpHomeClient)
    : IRequestHandler<GetMortgageRequest, GetMortgageResponse>
{
    public async Task<GetMortgageResponse> Handle(GetMortgageRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        return new GetMortgageResponse 
        { 
            Mortgage = loan.MapToProductServiceContract() 
        };
    }
}