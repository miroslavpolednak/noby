using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Database.Queries;

[Keyless]
public class CaseSaParametersQuery
{
    public long CaseId { get; set; }
    public int SalesArrangementId { get; set; }
    public int SalesArrangementTypeId { get; set; }
    public string? LoanApplicationAssessmentId { get; set; }
    public int State { get; set; }
    public byte[]? ParametersBin { get; set; } = null!;
}