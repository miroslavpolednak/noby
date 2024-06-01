using DomainServices.ProductService.ExternalServices.Pcp;

namespace DomainServices.ProductService.Api.Endpoints.CancelMortgage;

internal sealed class CancelMortgageHandler(
    IMpHomeClient _mpHomeClient, 
    IPcpClient _pcpClient,
    ILogger<CancelMortgageHandler> _logger) 
    : IRequestHandler<CancelMortgageRequest>
{
	public async Task Handle(CancelMortgageRequest request, CancellationToken cancellationToken)
    {
        var mortgage = await _mpHomeClient.GetMortgage(request.ProductId, cancellationToken);

        try
        {
            await _mpHomeClient.CancelLoan(request.ProductId, cancellationToken);

            // klienti s KB ID na produktu
			var clientIds = mortgage
				.LoanRelationships?
				.Where(t => t.KbId.HasValue)
				.Select(t => t.KbId!.Value)
				.ToList();

			// update v KB
			if (!string.IsNullOrEmpty(mortgage.PcpInstId) && (clientIds?.Any() ?? false))
            {
				await _pcpClient.UpdateProduct(mortgage.PcpInstId, clientIds, cancellationToken);
			}
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