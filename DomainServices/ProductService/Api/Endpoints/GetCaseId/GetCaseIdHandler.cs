﻿using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdHandler
    : IRequestHandler<GetCaseIdRequest, GetCaseIdResponse>
{
    public async Task<GetCaseIdResponse> Handle(GetCaseIdRequest request, CancellationToken cancellation)
    {
        switch (request.RequestParametersCase)
        {
            case GetCaseIdRequest.RequestParametersOneofCase.ContractNumber:
                var caseId1 = (await _dbContext.Loans
                    .AsNoTracking()
                    .Where(t => t.CisloSmlouvy == request.ContractNumber.ContractNumber && !t.Neaktivni)
                    .Select(t => new { t.Id })
                    .FirstOrDefaultAsync(cancellation))
                    ?.Id;

                if (!caseId1.HasValue)
                {
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound, request.ContractNumber.ContractNumber);
                }
                return new GetCaseIdResponse { CaseId = caseId1!.Value };

            case GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount:
                var caseId2 = (await _dbContext.Loans
                    .AsNoTracking()
                    .Where(t => t.PredcisliUctu == request.PaymentAccount.Prefix && t.CisloUctu == request.PaymentAccount.AccountNumber && !t.Neaktivni)
                    .Select(t => new { t.Id })
                    .FirstOrDefaultAsync(cancellation))
                    ?.Id;

                if (!caseId2.HasValue)
                {
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PaymentAccountNotFound, request.PaymentAccount.AccountNumber);
                }
                return new GetCaseIdResponse { CaseId = caseId2!.Value };

            case GetCaseIdRequest.RequestParametersOneofCase.PcpId:
            {
                var caseId3 = await _dbContext.LoanReservations
                                              .AsNoTracking()
                                              .Where(r => r.PcpInstId == request.PcpId.PcpId).Select(r => (long?)r.UverId)
                                              .FirstOrDefaultAsync(cancellation);

                if (!caseId3.HasValue)
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PcpIdNotFound, request.PcpId.PcpId);

                return new GetCaseIdResponse { CaseId = caseId3.Value };
            }

            default:
                throw new NotImplementedException();
        }
    }

    private readonly ProductServiceDbContext _dbContext;

    public GetCaseIdHandler(ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
