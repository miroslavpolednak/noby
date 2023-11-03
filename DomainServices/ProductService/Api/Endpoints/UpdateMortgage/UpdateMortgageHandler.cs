namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler : IRequestHandler<UpdateMortgageRequest>
{
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateMortgageHandler(LoanRepository repository, IMpHomeClient mpHomeClient)
    {
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }

    public async Task Handle(UpdateMortgageRequest request, CancellationToken cancellationToken)
    {
        if (!await _repository.LoanExists(request.ProductId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);

        // create request
        var mortgageRequest = request.Mortgage.ToMortgageRequest(request.Mortgage.PcpId);

        await _mpHomeClient.UpdateLoan(request.ProductId, mortgageRequest, cancellationToken);
    }

}