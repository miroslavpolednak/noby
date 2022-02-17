using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.ProductService.Contracts;
using DomainServices.ProductService.Api.Repositories;
using DomainServices.ProductService.Api.Repositories.Entities;
using ExternalServices.MpHome.V1._1;
using CIS.Core.Results;
using CIS.Infrastructure.gRPC;


namespace DomainServices.ProductService.Api.Handlers;

internal class BaseMortgageHandler
{

    #region Construction

    protected readonly ICodebookServiceAbstraction _codebookService;
    protected readonly LoanRepository _repository;
    protected readonly IMpHomeClient _mpHomeClient;
    protected readonly ILogger _logger;

    public BaseMortgageHandler(
        ICodebookServiceAbstraction codebookService,
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
    /// Checks if ProductTypeId matches ProductTypeCategory Mortgage
    /// </summary>
    private async Task CheckProductTypeCategory(long productTypeId)
    {
        var list = await _codebookService.ProductTypes();
        var item = list.FirstOrDefault(t => t.Id == productTypeId);

        if (item == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductTypeItem), productTypeId);
        }

        if (item.ProductCategory != ProductTypeCategory.Mortgage)
        {
            throw new CisArgumentException(1, $"ProductTypeId '{productTypeId}' doesn't match ProductTypeCategory '{ProductTypeCategory.Mortgage}'.", "ProductTypeId");
        }
    }

    /// <summary>
    /// Calls MpHome.UpdateMortgage endpoint according to params
    /// </summary>
    protected async Task<bool> UpdateLoan(long loanId, MortgageData mortgage, bool createNewOne, CancellationToken cancellation)
    {
        var loanExists = await _repository.ExistsLoan(loanId, cancellation);

        // TODO: check existension of KB entities (CaseId) 
        if (createNewOne && loanExists)
        {
            throw new CisAlreadyExistsException(13015, nameof(Loan), loanId); //TODO: error code
        }

        if (!createNewOne && !loanExists)
        {
            throw new CisNotFoundException(13014, nameof(Loan), loanId); //TODO: error code
        }

        // validate ProductTypeId
        await CheckProductTypeCategory(mortgage.ProductTypeId);

        // create request
        var mortgageRequest = mortgage.ToMortgageRequest();

        // call endpoint
        return resolveResult(await _mpHomeClient.UpdateLoan(loanId, mortgageRequest));
    }

    private bool resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult r => true,
            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
            _ => throw new NotImplementedException()
        };

}
