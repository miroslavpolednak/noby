using SharedTypes.Enums;

namespace DomainServices.SalesArrangementService.Clients;

public static class SalesArrangementHelpers
{
	public static bool IsSalesArrangementInState(IEnumerable<EnumSalesArrangementStates> allowedStates, in EnumSalesArrangementStates salesArrangementState)
	{
		return allowedStates.Contains(salesArrangementState);
	}

	public static bool IsInState(this Contracts.SalesArrangement salesArrangement, IEnumerable<EnumSalesArrangementStates> allowedStates)
		=> IsSalesArrangementInState(allowedStates, (EnumSalesArrangementStates)salesArrangement.State);

	/// <summary>
	/// Bez NewArrangement
	/// </summary>
	public static readonly EnumSalesArrangementStates[] AllExceptNewSalesArrangementStates =
	[
		EnumSalesArrangementStates.InProgress,
		EnumSalesArrangementStates.InApproval,
		EnumSalesArrangementStates.Disbursed,
		EnumSalesArrangementStates.InSigning,
		EnumSalesArrangementStates.ToSend,
		EnumSalesArrangementStates.Finished,
		EnumSalesArrangementStates.Cancelled,
		EnumSalesArrangementStates.RC2
	];

	/// <summary>
	/// Bez Finished a Cancelled
	/// </summary>
	public static readonly EnumSalesArrangementStates[] ActiveSalesArrangementStates =
	[
		EnumSalesArrangementStates.InProgress,
		EnumSalesArrangementStates.InApproval,
		EnumSalesArrangementStates.NewArrangement,
		EnumSalesArrangementStates.Disbursed,
		EnumSalesArrangementStates.InSigning,
		EnumSalesArrangementStates.ToSend,
		EnumSalesArrangementStates.RC2
	];
}
