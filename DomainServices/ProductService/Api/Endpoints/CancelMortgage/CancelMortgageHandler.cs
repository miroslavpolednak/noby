namespace DomainServices.ProductService.Api.Endpoints.CancelMortgage;

internal sealed class CancelMortgageHandler(
    IMpHomeClient _mpHomeClient, 
    ILogger<CancelMortgageHandler> _logger) 
    : IRequestHandler<CancelMortgageRequest>
{
	public async Task Handle(CancelMortgageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mpHomeClient.CancelLoan(request.ProductId, cancellationToken);
        }
        catch (CisNotFoundException)
        {
			throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);
		}
        catch (Exception ex)
        {
            _logger.CancelMortgageFailed(request.ProductId, ex);
        }
    }
}