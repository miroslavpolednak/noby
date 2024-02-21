﻿using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.GetCancelCaseJobIds;

internal sealed class GetCancelCaseJobIdsHandler
    : IRequestHandler<GetCancelCaseJobIdsRequest, GetCancelCaseJobIdsResponse>
{
    public async Task<GetCancelCaseJobIdsResponse> Handle(GetCancelCaseJobIdsRequest request, CancellationToken cancellationToken)
    {
        var caseIds = await _dbContext.Database
            .SqlQueryRaw<long>(_sqlQuery)
            .ToListAsync(cancellationToken);

        var response = new GetCancelCaseJobIdsResponse();
        response.CaseId.AddRange(caseIds);
        return response;
    }

    private const string _sqlQuery =
@"
SELECT
	CaseId
FROM
	[dbo].[SalesArrangement]
WHERE
	SalesArrangementTypeId = 1
	and (
		(FirstSignatureDate is null and CreatedTime<DATEADD(DAY, -90, GETDATE()))
		or (isnull(LoanApplicationAssessmentId, '')='' and CreatedTime<DATEADD(DAY, -40, GETDATE()))
		or (isnull(LoanApplicationAssessmentId, '')!='' and State!=2 and CreatedTime<DATEADD(DAY, -140, GETDATE()))
	)
";

    private readonly SalesArrangementServiceDbContext _dbContext;

    public GetCancelCaseJobIdsHandler(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}