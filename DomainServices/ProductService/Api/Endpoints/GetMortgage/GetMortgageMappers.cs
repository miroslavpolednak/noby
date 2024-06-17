namespace DomainServices.ProductService.Api.Endpoints.GetMortgage;

internal static class GetMortgageMappers
{
	public static MortgageData MapToProductServiceContract(this LoanDetail loan)
	{
		var mortgage = new MortgageData
		{
			PartnerId = loan.PartnerId.GetValueOrDefault(),
			BranchConsultantId = loan.ServiceBranchConsultantId,
			CaseOwnerUserCurrentId = loan.ConsultantId,
			CaseOwnerUserOrigId = loan.OriginalConsultantId,
			ContractNumber = loan.ContractNumber,
			LoanAmount = loan.LoanAmount.ToDecimal(),
			LoanInterestRate = loan.InterestRate.ToDecimal(),
			FixedRatePeriod = loan.FixationPeriod,
			ProductTypeId = loan.ProductUvCode.GetValueOrDefault(),
			LoanPaymentAmount = loan.MonthInstallment.ToDecimal(),
			CurrentAmount = loan.BalanceTotal.HasValue ? Math.Abs(loan.BalanceTotal.ToDecimal(0)) : default,
			DrawingDateTo = loan.DrawEndDate,
			ContractSignedDate = loan.ContractDate,
			FixedRateValidTo = loan.InterestFixationDate,
			AvailableForDrawing = loan.AmountLeft.ToDecimal(),
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
				IsCollateral = t.IsSecured,
				RealEstatePurchaseTypeId = t.PurposeCode.GetValueOrDefault(),
				RealEstateTypeId = t.TypeCode.GetValueOrDefault(),
			}));
		}

		if (loan.LoanRelationships?.Any() ?? false)
		{
			mortgage.Relationships.AddRange(loan.LoanRelationships.Select(t => new Contracts.Relationship
			{
				ContractRelationshipTypeId = t.PartnerRelationshipId,
				PartnerId = t.PartnerId
			}));
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
		EmailAddress2 = loan.StatementEmail2
	};
}