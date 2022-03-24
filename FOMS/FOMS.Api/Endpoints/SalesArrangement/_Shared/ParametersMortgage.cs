namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public class ParametersMortgage
{
	public DateTime? ExpectedDateOfDrawing { get; set; }

	/// <example>CZK</example>
	public string? IncomeCurrencyCode { get; set; }

	/// <example>CZK</example>
	public string? ResidencyCurrencyCode { get; set; }
	
	/// <example>1</example>
	public int SignatureTypeId { get; set; }
	
	public List<LoanRealEstate>? LoanRealEstates { get; set; }

	public class LoanRealEstate
    {
		/// <example>1</example>
		public int RealEstateTypeId { get; set; }
		
		/// <example>false</example>
		public bool IsCollateral { get; set; }
		
		/// <example>1</example>
		public int RealEstatePurchaseTypeId { get; set; }
	}
}
