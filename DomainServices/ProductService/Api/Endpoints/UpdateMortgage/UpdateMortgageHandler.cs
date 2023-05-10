using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.ProductService.Api.Database.Entities;
using DomainServices.ProductService.Api.Endpoints.CreateMortgage;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler
    : IRequestHandler<Contracts.UpdateMortgageRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    private readonly Database.LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;
    private readonly ILogger<CreateMortgageHandler> _logger;

    public UpdateMortgageHandler(
        Database.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateMortgageHandler> logger)
    {
        _repository = repository;
        _mpHomeClient = mpHomeClient;
        _logger = logger;
    }
    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateMortgageRequest request, CancellationToken cancellation)
    {
        if (!(await _repository.ExistsLoan(request.ProductId, cancellation)))
        {
            throw new CisNotFoundException(12001, nameof(Loan), request.ProductId);
        }

        // create request
        var mortgageRequest = request.Mortgage.ToMortgageRequest();

        // call endpoint
        await _mpHomeClient.UpdateLoan(request.ProductId, mortgageRequest, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

}