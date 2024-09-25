namespace DomainServices.RealEstateValuationService.Api.Dto;

public sealed record GetProductPropertiesResult(decimal? CollateralAmount, decimal? LoanAmount, int? LoanDuration, string? LoanPurpose)
{
}
