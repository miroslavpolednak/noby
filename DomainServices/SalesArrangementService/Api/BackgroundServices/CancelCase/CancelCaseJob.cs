using CIS.Core;
using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Api.Database.Queries;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.CancelCase;

internal sealed class CancelCaseJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
	private const string _sqlQuery =
@"
SELECT
	CaseId,
	SA.SalesArrangementId,
	SalesArrangementTypeId,
	LoanApplicationAssessmentId,
	State,
	Parametersbin,
	SalesArrangementParametersType
FROM
	[SalesArrangementService].[dbo].[SalesArrangement] SA left join
	[SalesArrangementService].[dbo].[SalesArrangementParameters] SAP on SA.SalesArrangementId = SAP.SalesArrangementId
WHERE
	SalesArrangementTypeId = 1
";
	
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        var caseSaParameters = await _dbContext.CaseSaParametersQuery
	        .FromSqlRaw(_sqlQuery)
	        .ToListAsync(cancellationToken);

        foreach (var caseSaParameter in caseSaParameters)
        {
	        try
	        {
		        var toCancel = await ToCancel(caseSaParameter, cancellationToken);
		        if (toCancel)
		        {
			        await _caseServiceClient.CancelCase(caseSaParameter.CaseId, false, cancellationToken);
		        }
	        }
	        catch (CisNotFoundException e)
	        {
		        _logger.EntityNotFound(e);
	        }
        }
    }

    private async Task<bool> ToCancel(CaseSaParametersQuery caseSaParameter, CancellationToken cancellationToken)
    {
	    var firstSignatureDate = GetFirstSignatureDate(caseSaParameter.ParametersBin);
	    var emptyLoanAppAssessment = string.IsNullOrEmpty(caseSaParameter.LoanApplicationAssessmentId);

	    if (firstSignatureDate != null)
		    return
			    (firstSignatureDate < _dateTime.Now.AddDays(-40) && emptyLoanAppAssessment) ||
			    (firstSignatureDate < _dateTime.Now.AddDays(-140) && !emptyLoanAppAssessment && caseSaParameter.State != 2);
	    
	    var caseDetail = await _caseServiceClient.GetCaseDetail(caseSaParameter.CaseId, cancellationToken);
	    return caseDetail.Created.DateTime < _dateTime.Now.AddDays(-90);
    }
    
    private static DateTime? GetFirstSignatureDate(byte[]? parametersBin)
    {
	    if (parametersBin == null)
		    return null;

	    var salesArrangementParameters = SalesArrangementParametersMortgage.Parser.ParseFrom(parametersBin);
	    return salesArrangementParameters.FirstSignatureDate;
    }
    
    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly ILogger<CancelCaseJob> _logger;

    public CancelCaseJob(
	    SalesArrangementServiceDbContext dbContext,
	    IDateTime dateTime,
	    ICaseServiceClient caseServiceClient,
	    ILogger<CancelCaseJob> logger)
    {
	    _dbContext = dbContext;
	    _dateTime = dateTime;
	    _caseServiceClient = caseServiceClient;
	    _logger = logger;
    }
}