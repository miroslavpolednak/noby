﻿using SharedTypes.GrpcTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SetContractNumber;

internal sealed class SetContractNumberHandler : IRequestHandler<SetContractNumberRequest, SetContractNumberResponse>
{
    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly Eas.IEasClient _easClient;
    private readonly ICaseServiceClient _caseService;
    private readonly IMediator _mediator;

    public SetContractNumberHandler(SalesArrangementServiceDbContext dbContext,
                                    ICustomerOnSAServiceClient customerOnSAService,
                                    Eas.IEasClient easClient,
                                    ICaseServiceClient caseService,
                                    IMediator mediator)
    {
        _dbContext = dbContext;
        _customerOnSAService = customerOnSAService;
        _easClient = easClient;
        _caseService = caseService;
        _mediator = mediator;
    }

    public async Task<SetContractNumberResponse> Handle(SetContractNumberRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await LoadSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (!salesArrangement.IsProductSalesArrangement())
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.SATypeNotSupported, salesArrangement.SalesArrangementTypeId);
        }
        
        if (string.IsNullOrWhiteSpace(salesArrangement.ContractNumber))
        {
            var contractNumber = await GetContractNumber(request.CustomerOnSaId, salesArrangement.CaseId, cancellationToken);

            await UpdateSalesArrangement(salesArrangement, contractNumber, cancellationToken);
            await UpdateCase(salesArrangement.CaseId, contractNumber, cancellationToken);

            return new SetContractNumberResponse { ContractNumber = contractNumber };
        }
        else
        {
            return new SetContractNumberResponse { ContractNumber = salesArrangement.ContractNumber };
        }
    }

    private async Task<SalesArrangement> LoadSalesArrangement(int salesArrangementId, CancellationToken cancellationToken)
    {
        return await _dbContext.SalesArrangements
                               .AsNoTracking()
                               .Where(t => t.SalesArrangementId == salesArrangementId)
                               .Select(DatabaseExpressions.SalesArrangementDetail())
                               .FirstOrDefaultAsync(cancellationToken)
               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, salesArrangementId);
    }

    private async Task<string> GetContractNumber(int customerOnSaId, long caseId, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);

        var identityMp = customerOnSa.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp) ??
                         throw new InvalidOperationException($"CustomerOnSa {customerOnSaId} does not have MP ID");

        return await _easClient.GetContractNumber(identityMp.IdentityId, (int)caseId, cancellationToken);
    }

    private async Task UpdateSalesArrangement(SalesArrangement salesArrangement, string contractNumber, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateSalesArrangementRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ContractNumber = contractNumber,
            RiskBusinessCaseId = salesArrangement.RiskBusinessCaseId
        }, cancellationToken);

        salesArrangement.ContractNumber = contractNumber;
    }

    private async Task UpdateCase(long caseId, string contractNumber, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);

        caseDetail.Data.ContractNumber = contractNumber;

        await _caseService.UpdateCaseData(caseId, caseDetail.Data, cancellationToken);
    }
}