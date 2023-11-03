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
        if (!await _repository.LoanExists(request.ProductId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        try
        {
            await _mpHomeClient.CancelLoan(request.ProductId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.CancelMortgageFailed(request.ProductId, ex);
        }
    }
}