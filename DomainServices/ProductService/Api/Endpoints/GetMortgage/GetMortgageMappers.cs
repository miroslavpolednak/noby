using DomainServices.ProductService.Api.Database.Models;

namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal static class GetMortgageMappers
{
	public static MortgageData MapToProductServiceContract(this LoanDetail loan)
	{
		var mortgage = new MortgageData
		{
			PartnerId = loan.PartnerId,
			BranchConsultantId = loan.ServiceBranchConsultantId,
			CaseOwnerUserCurrentId = loan.ConsultantId,
			CaseOwnerUserOrigId = loan.OriginalConsultantId,
			ContractNumber = loan.ContractNumber,
			LoanAmount = loan.LoanAmount.ToDecimal(),
			LoanInterestRate = loan.InterestRate.ToDecimal(),
			FixedRatePeriod = loan.FixationPeriod,
			ProductTypeId = loan.ProductUvCode.GetValueOrDefault(),
			LoanPaymentAmount = loan.MonthInstallment.ToDecimal(),
			CurrentAmount = loan.AmountLeft.HasValue ? Math.Abs(loan.AmountLeft.ToDecimal(0)) : default,
			DrawingDateTo = loan.DrawEndDate,
			ContractSignedDate = loan.ContractDate,
			FixedRateValidTo = loan.InterestFixationDate,
			AvailableForDrawing = loan.BalanceTotal.ToDecimal(),
			Principal = loan.Principal.HasValue ? Math.Abs(loan.Principal.ToDecimal(0)) : null,
			LoanKindId = loan.LoanKindId,
			PaymentAccount = parsePaymentAccount(loan),
			CurrentOverdueAmount = loan.TotalDebtOverdue.ToDecimal(),
			AllOverdueFees = loan.ReceivableFeeAfter.ToDecimal(),
			OverdueDaysNumber = loan.BankingDaysOverdue,
			ExpectedDateOfDrawing = loan.Estimated1stDrawDate,
			InterestInArrears = loan.RateFromOverdue.ToDecimal(),
			LoanDueDate = loan.EstimatedDuePaymentDate,
			PaymentDay = loan.InstallmentDay,
			LoanInterestRateRefix = null, // ???
			LoanInterestRateValidFromRefix = null, // ???
			FixedRatePeriodRefix = null, // ???
			FirstAnnuityPaymentDate = loan.FirstAnnuityInstallmentDate,
			FirstSignatureDate = loan.ApplicationSignDate,
			RepaymentAccount = parseRepaymentAccount(loan),
			Statement = parseStatementObject(loan),
			IsCancelled = loan.Inactive,
			MortgageState = loan.HfStatus,
			DrawingFinishedDate = loan.FinalDrawDate,
			PcpId = loan.PcpInstId
		};

		if (loan.Purposes?.Any() ?? false)
		{
			mortgage.LoanPurposes.AddRange(loan.Purposes.Select(t => new Contracts.LoanPurpose
			{
				LoanPurposeId = t.LoanPurposeId,
				Sum = t.Amount.ToDecimal()
			}));
		}

		if (loan.RealEstates?.Any() ?? false)
		{
			mortgage.LoanRealEstates.AddRange(loan.RealEstates.Select(t => new Contracts.LoanRealEstate
			{
				IsCollateral = false,//???
				RealEstatePurchaseTypeId = t.PurposeCode.GetValueOrDefault(),
				RealEstateTypeId = 1//???
			}));
		}

		if (relationships.Count != 0)
		{
			mortgage.Relationships.AddRange(relationships.Select(ToRelationship));
		}

		return mortgage;
	}

	private static PaymentAccount? parsePaymentAccount(LoanDetail loan) =>
		string.IsNullOrEmpty(loan.AccountNumber)
			? null
			: new PaymentAccount
			{
				Prefix = loan.AccountPrefixNumber ?? string.Empty,
				Number = loan.AccountNumber ?? string.Empty,
				BankCode = "0100" //ma byt hardcoded
			};

	private static PaymentAccount? parseRepaymentAccount(LoanDetail loan) =>
		string.IsNullOrEmpty(loan.DirectDebitAccountNumber)
			? null
			: new PaymentAccount
			{
				Prefix = loan.DirectDebitAccountPrefix ?? string.Empty,
				Number = loan.DirectDebitAccountNumber ?? string.Empty,
				BankCode = loan.DirectDebitAccountBank ?? string.Empty
			};

	private static StatementObject parseStatementObject(this LoanDetail loan) => new()
	{
		TypeId = loan.HfStatementType,
		SubscriptionTypeId = loan.HfStatementZodb,
		FrequencyId = loan.HfStatementFrequency,
		EmailAddress1 = loan.StatementEmail1,
		EmailAddress2 = loan.StatementEmail2,
		Address = new SharedTypes.GrpcTypes.GrpcAddress
		{
			Street = loan.Street ?? string.Empty,
			StreetNumber = loan.StreetNumber ?? string.Empty,
			HouseNumber = loan.HouseNumber ?? string.Empty,
			Postcode = loan.PostCode ?? string.Empty,
			City = loan.City ?? string.Empty,
			AddressPointId = loan.AddressPointId ?? string.Empty,
			CountryId = loan.CountryId
		}
	};
}