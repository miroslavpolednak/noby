namespace FOMS.Api.Endpoints.Cases.GetTotalsByStates;

public sealed record class GetDashboardFiltersResponse(int FilterId, string? Text, int CaseCount)
{ 
}