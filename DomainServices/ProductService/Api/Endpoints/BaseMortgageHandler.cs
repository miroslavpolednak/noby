using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.ProductService.Contracts;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Api.Database.Entities;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.ProductService.Api.Endpoints;

internal class BaseMortgageHandler
{

    #region Construction

    protected readonly ICodebookServiceClients _codebookService;
    protected readonly LoanRepository _repository;
    protected readonly IMpHomeClient _mpHomeClient;
    protected readonly ILogger _logger;

    public BaseMortgageHandler(
        ICodebookServiceClients codebookService,
        LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _mpHomeClient = mpHomeClient;
        _logger = logger;
    }

    #endregion


    /// <summary>
    /// Calls MpHome.UpdateMortgage endpoint according to params
    /// </summary>
    protected async Task UpdateLoan(long loanId, MortgageData mortgage, bool createNewOne, CancellationToken cancellation)
    {
        var loanExists = await _repository.ExistsLoan(loanId, cancellation);

        if (createNewOne && loanExists)
        {
            throw ErrorCodeMapper.CreateAlreadyExistsException(ErrorCodeMapper.AlreadyExists12005, loanId);
        }

        if (!createNewOne && !loanExists)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, loanId);
        }

        // create request
        var mortgageRequest = mortgage.ToMortgageRequest();

        // call endpoint
        await _mpHomeClient.UpdateLoan(loanId, mortgageRequest);
    }
}
