﻿using DomainServices.CodebookService.Clients;
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
            throw new CisAlreadyExistsException(12005, nameof(Loan), loanId);
        }

        if (!createNewOne && !loanExists)
        {
            throw new CisNotFoundException(12001, nameof(Loan), loanId);
        }

        // create request
        var mortgageRequest = mortgage.ToMortgageRequest();

        // call endpoint
        await _mpHomeClient.UpdateLoan(loanId, mortgageRequest);
    }

    /// <summary>
    /// Returns mapping LoanType -> ProductTypeId
    /// </summary>
    protected async Task<Dictionary<int, int>> GetMapLoanTypeToProductTypeId()
    {
        var list = await _codebookService.ProductTypes();
#pragma warning disable CS8629 // Nullable value type may be null.
        return list.Where(i => i.KonsDbLoanType.HasValue).ToDictionary(i => i.KonsDbLoanType.Value, i => i.Id);
#pragma warning restore CS8629 // Nullable value type may be null.
    }
}