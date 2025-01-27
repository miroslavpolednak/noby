CREATE TABLE RiskLoanApplicationDataField
(
	DataFieldId INT NOT NULL,
	JsonPropertyName NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_RiskLoanApplicationDataField PRIMARY KEY (DataFieldId, JsonPropertyName),
	CONSTRAINT FK_RiskLoanApplicationDataField_DataField FOREIGN KEY (DataFieldId) REFERENCES DataField (DataFieldId)
)

CREATE TABLE RiskLoanApplicationSpecialDataField
(
	JsonPropertyName NVARCHAR(250) NOT NULL,
	DataServiceId INT NOT NULL,
	FieldPath NVARCHAR(250) NOT NULL,

	CONSTRAINT PK_RiskLoanApplicationSpecialDataField PRIMARY KEY (JsonPropertyName),
	CONSTRAINT FK_RiskLoanApplicationSpecialDataField_DataService FOREIGN KEY (DataServiceId) REFERENCES DataService (DataServiceId)
)


SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (207, 1, 'SalesArrangement.SalesArrangementId', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (208, 3, 'Offer.SimulationInputs.Developer.DeveloperId', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (209, 3, 'Offer.SimulationInputs.Developer.ProjectId', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (210, 1, 'SalesArrangement.Mortgage.LoanRealEstates[].RealEstatePurchaseTypeId', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (211, 3, 'Offer.SimulationInputs.LoanPurposes[].LoanPurposeId', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (212, 3, 'Offer.SimulationInputs.LoanPurposes[].Sum', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF


INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (5, N'Product.RequiredAmount')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (6, N'Product.FixedRatePeriod')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (8, N'Product.LoanDuration')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (9, N'Product.LoanPaymentAmount')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (10, N'Product.Ltv')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (20, N'Product.RepaymentPeriodStart')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (65, N'DistributionChannelId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (66, N'Product.DrawingPeriodEnd')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (68, N'Product.LoanInterestRate')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (72, N'Product.DrawingPeriodStart')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (84, N'Product.HomeCurrencyIncome')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (85, N'Product.HomeCurrencyResidence')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (93, N'Product.RepaymentPeriodEnd')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (94, N'Product.InstallmentCount')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (100, N'Product.LoanKindId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (101, N'Product.OwnResourcesAmount')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (102, N'Product.ForeignResourcesAmount')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (194, N'Product.ProductTypeId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (207, N'SalesArrangementId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (208, N'Product.DeveloperId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (209, N'Product.DeveloperProjectId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (210, N'Product.FinancingTypes[]')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (211, N'Product.Purposes[].LoanPurposeId')
INSERT [dbo].[RiskLoanApplicationDataField] ([DataFieldId], [JsonPropertyName]) VALUES (212, N'Product.Purposes[].Amount')

INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].ChildrenOverTenYearsCount', 1, N'Households[].Household.Data.ChildrenOverTenYearsCount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].ChildrenUpToTenYearsCount', 1, N'Households[].Household.Data.ChildrenUpToTenYearsCount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].AcademicTitlePrefix', 1, N'Households[].Customers[].DegreeBefore')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.City', 1, N'Households[].Customers[].PermanentAddress.City')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.CountryId', 1, N'Households[].Customers[].PermanentAddress.CountryId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.EvidenceNumber', 1, N'Households[].Customers[].PermanentAddress.EvidenceNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.HouseNumber', 1, N'Households[].Customers[].PermanentAddress.HouseNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.Postcode', 1, N'Households[].Customers[].PermanentAddress.Postcode')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.Street', 1, N'Households[].Customers[].PermanentAddress.Street')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Address.StreetNumber', 1, N'Households[].Customers[].PermanentAddress.StreetNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].BirthDate', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.DateOfBirth')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].BirthName', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.BirthName')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].BirthNumber', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.BirthNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].BirthPlace', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.PlaceOfBirth')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].CustomerRoleId', 1, N'Households[].Customers[].CustomerRoleId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].EducationLevelId', 1, N'Households[].Customers[].EducationLevelId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Firstname', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.FirstName')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].GenderId', 1, N'Households[].Customers[].GenderId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].HasEmail', 1, N'Households[].Customers[].HasEmail')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].IdentificationDocument.DocumentNumber', 1, N'Households[].Customers[].CustomerDetail.IdentificationDocument.Number')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].IdentificationDocument.IdentificationDocumentTypeId', 1, N'Households[].Customers[].CustomerDetail.IdentificationDocument.IdentificationDocumentTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].IdentificationDocument.IssuedOn', 1, N'Households[].Customers[].CustomerDetail.IdentificationDocument.IssuedOn')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].IdentificationDocument.ValidTo', 1, N'Households[].Customers[].CustomerDetail.IdentificationDocument.ValidTo')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].ConfirmationByCompany', 1, N'Households[].Customers[].Incomes.Employments[].Income.IncomeConfirmation.IsIssuedByExternalAccountant')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].ConfirmationContactPhone', 1, N'Households[].Customers[].Incomes.Employments[].Income.IncomeConfirmation.ConfirmationContact')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].ConfirmationDate', 1, N'Households[].Customers[].Incomes.Employments[].Income.IncomeConfirmation.ConfirmationDate')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].ConfirmationPerson', 1, N'Households[].Customers[].Incomes.Employments[].Income.IncomeConfirmation.ConfirmationPerson')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].CountryId', 1, N'Households[].Customers[].Incomes.Employments[].Income.Employement.Employer.CountryId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].CurrentWorkContractSince', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.CurrentWorkContractSince')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].CurrentWorkContractTo', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.CurrentWorkContractTo')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].EmployerIdentificationNumber', 1, N'Households[].Customers[].Incomes.Employments[].EmployerIdentificationNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].EmployerName', 1, N'Households[].Customers[].Incomes.Employments[].EmployerName')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].EmploymentTypeId', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.EmploymentTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].FirstWorkContractSince', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.FirstWorkContractSince')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].IncomeDeduction.Execution', 1, N'Households[].Customers[].Incomes.Employments[].Income.WageDeduction.DeductionDecision')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].IncomeDeduction.Installments', 1, N'Households[].Customers[].Incomes.Employments[].Income.WageDeduction.DeductionPayments')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].IncomeDeduction.Other', 1, N'Households[].Customers[].Incomes.Employments[].Income.WageDeduction.DeductionOther')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].IncomeForeignTypeId', 1, N'Households[].Customers[].Incomes.Employments[].Income.ForeignIncomeTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].JobDescription', 1, N'Households[].Customers[].Incomes.Employments[].Income.Employement.Job.JobDescription')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].JobTrialPeriod', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.IsInTrialPeriod')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].MonthlyIncomeAmount.Amount', 1, N'Households[].Customers[].Incomes.Employments[].Sum')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].NoticePeriod', 1, N'Households[].Customers[].Incomes.Employments[].Income.Job.IsInProbationaryPeriod')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EmploymentIncomes[].ProofTypeId', 1, N'Households[].Customers[].Incomes.Employments[].ProofTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EntrepreneurIncome.AnnualIncomeAmount.Amount', 1, N'Households[].Customers[].Incomes.Entrepreneur.Sum')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EntrepreneurIncome.CountryId', 1, N'Households[].Customers[].Incomes.Entrepreneur.Income.CountryOfResidenceId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EntrepreneurIncome.EntrepreneurIdentificationNumber', 1, N'Households[].Customers[].Incomes.Entrepreneur.EntrepreneurIdentificationNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.EntrepreneurIncome.ProofTypeId', 1, N'Households[].Customers[].Incomes.Entrepreneur.ProofTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.IsIncomeConfirmed', 1, N'Households[].Customers[].Incomes.IsIncomeConfirmed')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.LastConfirmedDate', 1, N'Households[].Customers[].Incomes.LastConfirmedDate')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.OtherIncomes[].IncomeOtherTypeId', 1, N'Households[].Customers[].Incomes.Others[].IncomeOtherTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.OtherIncomes[].MonthlyIncomeAmount.Amount', 1, N'Households[].Customers[].Incomes.Others[].Sum')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Income.RentIncome.MonthlyIncomeAmount.Amount', 1, N'Households[].Customers[].Incomes.Rent.Sum')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].InternalCustomerId', 1, N'Households[].Customers[].InternalCustomerId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].IsPartner', 1, N'Households[].Customers[].IsPartner')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].MaritalStateId', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.MaritalStatusStateId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].MobilePhoneNumber', 1, N'Households[].Customers[].PhoneNumber')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Obligations[].Amount', 1, N'Households[].Customers[].Obligations[].Amount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Obligations[].AmountConsolidated', 1, N'Households[].Customers[].Obligations[].AmountConsolidated')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Obligations[].Installment', 1, N'Households[].Customers[].Obligations[].Installment')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Obligations[].InstallmentConsolidated', 1, N'Households[].Customers[].Obligations[].InstallmentConsolidated')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Obligations[].ObligationTypeId', 1, N'Households[].Customers[].Obligations[].ObligationTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].PrimaryCustomerId', 1, N'Households[].Customers[].CustomerId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].SpecialRelationsWithKB', 1, N'Households[].Customers[].SpecialRelationsWithKB')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Surname', 1, N'Households[].Customers[].CustomerDetail.NaturalPerson.LastName')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Customers[].Taxpayer', 1, N'Households[].Customers[].IsTaxPayer')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Expenses.Insurance', 1, N'Households[].Household.Expenses.InsuranceExpenseAmount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Expenses.Other', 1, N'Households[].Household.Expenses.OtherExpenseAmount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Expenses.Rent', 1, N'Households[].Household.Expenses.HousingExpenseAmount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].Expenses.Saving', 1, N'Households[].Household.Expenses.SavingExpenseAmount')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].HouseholdId', 1, N'Households[].Household.HouseholdId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].HouseholdTypeId', 1, N'Households[].Household.HouseholdTypeId')
INSERT [dbo].[RiskLoanApplicationSpecialDataField] ([JsonPropertyName], [DataServiceId], [FieldPath]) VALUES (N'Households[].PropertySettlementId', 1, N'Households[].Household.Data.PropertySettlementId')