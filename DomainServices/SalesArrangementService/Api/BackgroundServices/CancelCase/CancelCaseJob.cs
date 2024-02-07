using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.CancelCase;

internal sealed class CancelCaseJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
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
	
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        var caseIds = await _dbContext.Database
			.SqlQueryRaw<long>(_sqlQuery)
	        .ToListAsync(cancellationToken);

        foreach (var caseId in caseIds)
        {
	        try
	        {
				var caseInstance = await _caseServiceClient.ValidateCaseId(caseId, false, cancellationToken);

				if (caseInstance.Exists && caseInstance.State == (int)CaseStates.InProgress)
				{
					await _caseServiceClient.CancelCase(caseId, false, cancellationToken);
				}

				_logger.CancelCaseJobFinished(caseId);
            }
			catch (Exception e)
			{
                _logger.CancelCaseJobFailed(caseId, e.Message, e);
            }
        }
    }

    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly ILogger<CancelCaseJob> _logger;

    public CancelCaseJob(
	    SalesArrangementServiceDbContext dbContext,
	    ICaseServiceClient caseServiceClient,
	    ILogger<CancelCaseJob> logger)
    {
	    _dbContext = dbContext;
	    _caseServiceClient = caseServiceClient;
	    _logger = logger;
    }
}