namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public class LoanRealEstateDto
{
	/// <example>1</example>
	public int RealEstateTypeId { get; set; }

	/// <example>false</example>
	public bool IsCollateral { get; set; }

	/// <example>1</example>
	public int RealEstatePurchaseTypeId { get; set; }
}
