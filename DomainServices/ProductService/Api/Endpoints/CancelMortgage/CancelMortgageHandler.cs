using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1;

namespace DomainServices.ProductService.Api.Endpoints.CancelMortgage;

internal class CancelMortgageHandler : IRequestHandler<CancelMortgageRequest>
{
    private readonly IMpHomeClient _mpHomeClient;
    private readonly LoanRepository _repository;
    private readonly ILogger<CancelMortgageHandler> _logger;

    public CancelMortgageHandler(IMpHomeClient mpHomeClient, LoanRepository repository, ILogger<CancelMortgageHandler> logger)
    {
        _mpHomeClient = mpHomeClient;
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(CancelMortgageRequest request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsLoan(request.ProductId, cancellationToken))
            throw new CisNotFoundException(12001, nameof(Database.Models.Loan), request.ProductId);

        try
        {
            await _mpHomeClient.CancelLoan(request.ProductId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cancel mortgage failed");
        }
    }
}