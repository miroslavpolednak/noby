INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (0, N'General')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (1, N'SalesArrangementService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (2, N'CaseService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (3, N'OfferService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (4, N'UserService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (5, N'CustomerService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (6, N'ProductService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (7, N'OfferPaymentScheduleService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (8, N'HouseholdService')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (9, N'CustomersOnSa')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (10, N'CustomersOnSaDetail')
GO
INSERT [dbo].[DataService] ([DataServiceId], [DataServiceName]) VALUES (11, N'HouseholdWithCustomersService')
GO
SET IDENTITY_INSERT [dbo].[DataField] ON 
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (1, 1, N'SalesArrangement.CaseId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (2, 1, N'SalesArrangement.OfferId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (3, 1, N'SalesArrangement.Created.UserId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (4, 2, N'Case.Customer.DateOfBirthNaturalPerson', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (5, 3, N'Offer.SimulationInputs.LoanAmount', N'{0:C0}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (6, 3, N'Offer.SimulationInputs.FixedRatePeriod', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (7, 3, N'Offer.SimulationResults.LoanInterestRateProvided', N'{0:P} p.a.')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (8, 3, N'Offer.SimulationResults.LoanDuration', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (9, 3, N'Offer.SimulationResults.LoanPaymentAmount', N'{0:C0}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (10, 3, N'Offer.SimulationResults.LoanToValue', N'{0:0.##} %')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (11, 3, N'Offer.SimulationInputs.RiskLifeInsurance.Sum', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (12, 3, N'Offer.SimulationInputs.RealEstateInsurance.Sum', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (13, 3, N'Offer.AdditionalSimulationResults.MarketingActions', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (14, 3, N'Offer.SimulationInputs.MarketingActions.IncomeLoanRatioDiscount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (15, 1, N'SalesArrangement.OfferGuaranteeDateTo', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (16, 3, N'Offer.SimulationResults.Aprc', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (17, 3, N'Offer.SimulationResults.LoanTotalAmount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (18, 3, N'Offer.SimulationInputs.ExpectedDateOfDrawing', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (19, 3, N'Offer.SimulationInputs.PaymentDay', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (20, 3, N'Offer.SimulationResults.AnnuityPaymentsDateFrom', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (22, 0, N'Custom.CurrentDateTime', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (23, 3, N'Offer.AdditionalSimulationResults.PaymentScheduleSimple[].PaymentNumber', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (24, 3, N'Offer.AdditionalSimulationResults.PaymentScheduleSimple[].Date', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (25, 3, N'Offer.AdditionalSimulationResults.PaymentScheduleSimple[].Type', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (26, 3, N'Offer.AdditionalSimulationResults.PaymentScheduleSimple[].Amount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (27, 4, N'User.FullName', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (28, 3, N'Offer.OfferId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (29, 5, N'Customer.NaturalPerson.BirthNumber', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (30, 5, N'Customer.NaturalPerson.DateOfBirth', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (31, 1, N'Custom.SalesArrangementPayoutList[].DrawingAmount', N'{0:#,#.##}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (32, 1, N'Custom.SalesArrangementPayoutList[].BankAccount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (33, 1, N'Custom.SalesArrangementPayoutList[].BankCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (34, 1, N'Custom.SalesArrangementPayoutList[].VariableSymbol', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (35, 1, N'Custom.SalesArrangementPayoutList[].ConstantSymbol', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (36, 1, N'Custom.SalesArrangementPayoutList[].SpecificSymbol', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (37, 1, N'SalesArrangement.Drawing.Applicant', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (38, 2, N'Case.Data.ContractNumber', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (39, 1, N'SalesArrangement.Drawing.DrawingDate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (40, 1, N'SalesArrangement.Drawing.IsImmediateDrawing', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (41, 6, N'Mortgage.PaymentAccount.Prefix', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (42, 6, N'Mortgage.PaymentAccount.Number', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (43, 6, N'Mortgage.PaymentAccount.BankCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (44, 1, N'SalesArrangement.Drawing.Applicant.IdentityId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (46, 5, N'Customer.NaturalPerson.LastName', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (47, 5, N'Customer.NaturalPerson.FirstName', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (48, 1, N'SalesArrangement.Drawing.RepaymentAccount.Prefix', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (49, 1, N'SalesArrangement.Drawing.RepaymentAccount.Number', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (50, 1, N'SalesArrangement.Drawing.RepaymentAccount.BankCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (52, 1, N'SalesArrangement.Drawing.PayoutList[].Order', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (53, 1, N'SalesArrangement.Drawing.PayoutList[].PrefixAccount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (54, 1, N'SalesArrangement.Drawing.PayoutList[].AccountNumber', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (55, 1, N'SalesArrangement.Drawing.PayoutList[].BankCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (56, 1, N'SalesArrangement.Drawing.PayoutList[].DrawingAmount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (57, 1, N'SalesArrangement.Drawing.PayoutList[].VariableSymbol', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (58, 1, N'SalesArrangement.Drawing.PayoutList[].ConstantSymbol', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (59, 1, N'SalesArrangement.Drawing.PayoutList[].SpecificSymbolUcetKeSplaceni', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (60, 1, N'SalesArrangement.ContractNumber', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (61, 1, N'SalesArrangement.RiskBusinessCaseId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (62, 1, N'SalesArrangement.RiskSegment', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (63, 1, N'SalesArrangement.LoanApplicationAssessmentId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (64, 1, N'SalesArrangement.RiskBusinessCaseExpirationDate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (65, 1, N'SalesArrangement.ChannelId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (66, 3, N'Offer.SimulationResults.DrawingDateTo', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (67, 3, N'Offer.SimulationResults.LoanInterestRateAnnounced', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (68, 3, N'Offer.SimulationResults.LoanInterestRate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (69, 3, N'Offer.SimulationResults.LoanInterestRateAnnouncedType', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (71, 3, N'Offer.SimulationResults.EmployeeBonusLoanCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (72, 1, N'SalesArrangement.Mortgage.ExpectedDateOfDrawing', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (73, 3, N'Offer.AdditionalSimulationResults.MarketingActions[].Code', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (74, 3, N'Offer.AdditionalSimulationResults.MarketingActions[].Requested', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (75, 3, N'Offer.AdditionalSimulationResults.MarketingActions[].Applied', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (76, 3, N'Offer.AdditionalSimulationResults.MarketingActions[].MarketingActionId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (77, 3, N'Offer.AdditionalSimulationResults.MarketingActions[].Deviation', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (78, 3, N'Offer.AdditionalSimulationResults.Fees[].FeeId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (79, 3, N'Offer.AdditionalSimulationResults.Fees[].TariffSum', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (80, 3, N'Offer.AdditionalSimulationResults.Fees[].ComposedSum', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (81, 3, N'Offer.AdditionalSimulationResults.Fees[].FinalSum', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (82, 3, N'Offer.AdditionalSimulationResults.Fees[].DiscountPercentage', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (83, 3, N'Offer.AdditionalSimulationResults.Fees[].MarketingActionId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (84, 1, N'SalesArrangement.Mortgage.IncomeCurrencyCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (85, 1, N'SalesArrangement.Mortgage.ResidencyCurrencyCode', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (86, 3, N'Offer.BasicParameters.StatementTypeId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (87, 3, N'Offer.SimulationInputs.CollateralAmount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (88, 1, N'SalesArrangement.OfferGuaranteeDateFrom', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (89, 3, N'Offer.BasicParameters.GuaranteeDateTo', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (90, 1, N'SalesArrangement.Mortgage.ContractSignatureTypeId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (91, 1, N'SalesArrangement.Mortgage.AgentConsentWithElCom', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (93, 3, N'Offer.SimulationResults.LoanDueDate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (94, 3, N'Offer.SimulationResults.AnnuityPaymentsCount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (96, 3, N'Offer.SimulationInputs.InterestRateDiscount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (97, 3, N'Offer.SimulationInputs.MarketingActions.Domicile', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (99, 7, N'OfferPaymentSchedule.PaymentScheduleFull[]', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (100, 3, N'Offer.SimulationInputs.LoanKindId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (101, 3, N'Offer.BasicParameters.FinancialResourcesOwn', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (102, 3, N'Offer.BasicParameters.FinancialResourcesOther', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (103, 3, N'Offer.SimulationInputs.MarketingActions.HealthRiskInsurance', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (104, 3, N'Offer.SimulationInputs.MarketingActions.RealEstateInsurance', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (105, 8, N'HouseholdMain.Household.Data.ChildrenUpToTenYearsCount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (106, 8, N'HouseholdMain.Household.Data.ChildrenOverTenYearsCount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (107, 8, N'HouseholdMain.Household.Expenses.HousingExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (108, 8, N'HouseholdMain.Household.Expenses.InsuranceExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (109, 8, N'HouseholdMain.Household.Expenses.SavingExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (110, 8, N'HouseholdMain.Household.Expenses.OtherExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (117, 11, N'HouseholdCodebtor.Household.Data.ChildrenUpToTenYearsCount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (118, 11, N'HouseholdCodebtor.Household.Data.ChildrenOverTenYearsCount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (119, 11, N'HouseholdCodebtor.Household.Expenses.HousingExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (120, 11, N'HouseholdCodebtor.Household.Expenses.InsuranceExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (121, 11, N'HouseholdCodebtor.Household.Expenses.SavingExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (122, 11, N'HouseholdCodebtor.Household.Expenses.OtherExpenseAmount', N'{0:C}')
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (123, 1, N'SalesArrangement.GeneralChange.Collateral.AddLoanRealEstateCollateral', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (124, 1, N'SalesArrangement.GeneralChange.Collateral.ReleaseLoanRealEstateCollateral', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (125, 1, N'SalesArrangement.GeneralChange.PaymentDay.NewPaymentDay', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (126, 1, N'SalesArrangement.GeneralChange.LoanPurpose.LoanPurposesComment', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (127, 1, N'SalesArrangement.GeneralChange.LoanPaymentAmount.NewLoanPaymentAmount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (128, 1, N'SalesArrangement.GeneralChange.DueDate.NewLoanDueDate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (129, 1, N'SalesArrangement.GeneralChange.DrawingDateTo.ExtensionDrawingDateToByMonths', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (130, 1, N'SalesArrangement.GeneralChange.DrawingAndOtherConditions.CommentToChangeContractConditions', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (131, 1, N'SalesArrangement.GeneralChange.CommentToChangeRequest.GeneralComment', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (132, 1, N'SalesArrangement.HUBN.LoanAmount.RequiredLoanAmount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (133, 1, N'SalesArrangement.HUBN.LoanAmount.AgreedLoanDueDate', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (134, 1, N'SalesArrangement.HUBN.LoanPurposes[].Sum', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (135, 1, N'SalesArrangement.HUBN.CollateralIdentification.RealEstateIdentification', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (136, 1, N'SalesArrangement.HUBN.DrawingDateTo.ExtensionDrawingDateToByMonths', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (137, 1, N'SalesArrangement.HUBN.ExpectedDateOfDrawing.NewExpectedDateOfDrawing', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (138, 1, N'SalesArrangement.HUBN.CommentToChangeRequest.GeneralComment', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (139, 1, N'SalesArrangement.GeneralChange.Applicant', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (140, 1, N'SalesArrangement.HUBN.Applicant', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (141, 6, N'Mortgage.PartnerId', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (142, 1, N'SalesArrangement.Drawing.RepaymentAccount', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (143, 1, N'SalesArrangement.Drawing', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (144, 1, N'SalesArrangement.Drawing.Agent.IsActive', NULL)
GO
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (145, 1, N'SalesArrangement.Drawing.Agent', NULL)
GO
SET IDENTITY_INSERT [dbo].[DataField] OFF
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (1, N'NABIDKA')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (2, N'KALKULHU')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (3, N'SPLKALHU')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (4, N'ZADOSTHU')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (5, N'ZADOSTHD')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (6, N'ZADOCERP')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (8, N'ZAOZMPAR')
GO
INSERT [dbo].[Document] ([DocumentId], [DocumentName]) VALUES (10, N'ZAODHUBN')
GO
SET IDENTITY_INSERT [dbo].[DocumentDataField] ON 
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (1, 1, N'001', 4, N'DatumNarozeni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (2, 1, N'001', 5, N'VyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (3, 1, N'001', 6, N'DelkaFixace', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (4, 1, N'001', 7, N'UrokovaSazba', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (5, 1, N'001', 8, N'SplatnostUveru', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (6, 1, N'001', 9, N'VyseSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (7, 1, N'001', 10, N'Ltv', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (8, 1, N'001', 11, N'RzpKb', N'{0} Kč/měsíčně', N'--')
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (9, 1, N'001', 12, N'PnKb', N'{0} Kč/ročně', N'--')
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (10, 1, N'001', 13, N'Domicilace', N'--', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (11, 1, N'001', 13, N'RZP', N'--', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (12, 1, N'001', 13, N'PN', N'--', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (13, 1, N'001', 14, N'DalsiPodminky', N'--', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (14, 1, N'001', 15, N'Text', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (15, 1, N'001', 16, N'RPSN', N'{0:P}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (16, 1, N'001', 17, N'SplatnaCastka', N'{0:C}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (17, 1, N'001', 18, N'ZahajeniCerpani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (18, 1, N'001', 19, N'DenSplaceni', N'{0}.dni v měsíci', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (19, 1, N'001', 20, N'PrvniSplatka', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (20, 1, N'001', 7, N'UrokovaSazba2', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (21, 1, N'001', 22, N'KeDni', N'{0}.', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (22, 1, N'001', 23, N'CisloSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (23, 1, N'001', 24, N'DatumSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (24, 1, N'001', 25, N'TypSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (25, 1, N'001', 26, N'VyseSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (26, 2, N'001', 5, N'VyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (27, 2, N'001', 6, N'DelkaFixace', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (28, 2, N'001', 8, N'SplatnostUveru', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (29, 2, N'001', 10, N'Ltv', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (30, 2, N'001', 97, N'Domicilace', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (31, 2, N'001', 103, N'RzpKb', N'ano', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (32, 2, N'001', 104, N'PnKb', N'ano', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (33, 2, N'001', 14, N'DalsiPodminky', N'výše prokázaného čistého příjmu od 50 000 Kč 
nebo výše úvěru od 5 000 000 Kč', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (34, 2, N'001', 7, N'UrokovaSazba', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (35, 2, N'001', 9, N'VyseSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (36, 2, N'001', 16, N'RPSN', N'{0:P}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (37, 2, N'001', 17, N'SplatnaCastka', N'{0:C}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (38, 2, N'001', 18, N'ZahajeniCerpani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (39, 2, N'001', 19, N'DenSplaceni', N'{0}.dni v měsíci', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (40, 2, N'001', 20, N'PrvniSplatka', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (41, 2, N'001', 7, N'UrokovaSazba2', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (42, 2, N'001', 23, N'CisloSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (43, 2, N'001', 24, N'DatumSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (44, 2, N'001', 25, N'TypSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (45, 2, N'001', 26, N'VyseSplatky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (46, 3, N'001', 5, N'VyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (47, 3, N'001', 6, N'DelkaFixace', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (48, 3, N'001', 7, N'UrokovaSazba', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (49, 3, N'001', 8, N'SplatnostUveru', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (90, 4, N'001', 60, N'RegCislo', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (91, 4, N'001', 5, N'VyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (92, 4, N'001', 101, N'VlastniZdroje', NULL, N'--')
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (93, 4, N'001', 102, N'OstatniZdroje', NULL, N'--')
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (94, 4, N'001', 6, N'DelkaFixace', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (96, 4, N'001', 19, N'DenSplaceni', N'{0}. dni v měsíci', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (97, 4, N'001', 97, N'Domicilace', N'ano', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (98, 4, N'001', 103, N'RZP', N'ano', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (99, 4, N'001', 104, N'PN', N'ano', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (100, 4, N'001', 18, N'ZahajeniCerpani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (101, 4, N'001', 105, N'PocetDetiDo10', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (102, 4, N'001', 106, N'PocetDetiNad10', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (103, 4, N'001', 107, N'NakladyBydleni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (104, 4, N'001', 108, N'Pojisteni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (105, 4, N'001', 109, N'Sporeni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (106, 4, N'001', 110, N'OstatniVydaje', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (107, 4, N'001', 84, N'MenaPrijmu', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (108, 4, N'001', 85, N'MenaBydliste', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (109, 4, N'001', 87, N'HodnotaZajisteni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (110, 5, N'001', 60, N'RegCislo', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (111, 5, N'001', 5, N'VyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (113, 5, N'001', 6, N'DelkaFixace', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (114, 5, N'001', 8, N'Splatnost', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (115, 5, N'001', 18, N'ZahajeniCerpani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (116, 5, N'001', 117, N'PocetDetiDo10', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (117, 5, N'001', 118, N'PocetDetiNad10', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (118, 5, N'001', 119, N'NakladyBydleni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (119, 5, N'001', 120, N'Pojisteni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (120, 5, N'001', 121, N'Sporeni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (121, 5, N'001', 122, N'OstatniVydaje', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (122, 6, N'001', 29, N'RodneCisloText', N'Rodné číslo:', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (123, 6, N'001', 29, N'RodneCislo', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (124, 6, N'001', 30, N'DatumNarozeniText', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (125, 6, N'001', 30, N'DatumNarozeni', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (126, 6, N'001', 31, N'CastkaVKc', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (127, 6, N'001', 32, N'CisloUctu', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (128, 6, N'001', 33, N'KodBanky', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (129, 6, N'001', 34, N'VariabilniSymbol', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (130, 6, N'001', 35, N'KonstantniSymbol', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (131, 6, N'001', 36, N'SpecifickySymbol', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (132, 8, N'001', 29, N'RodneCisloText', N'Rodné číslo: ', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (133, 8, N'001', 29, N'RodneCislo', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (134, 8, N'001', 30, N'DatumNarozeniText', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (135, 8, N'001', 30, N'DatumNarozeni', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (136, 8, N'001', 123, N'Vyvazani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (137, 8, N'001', 124, N'Pridani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (138, 8, N'001', 125, N'NovyDenSplaceni', N'{0}. den v měsíci', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (139, 8, N'001', 126, N'NovyUcel', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (140, 8, N'001', 127, N'PozadovanaVyseNoveSplatky', N'{0:C}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (141, 8, N'001', 128, N'NovaDobaSplatnosti', N'{0:Y}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (142, 8, N'001', 129, N'ProdlouzeniLhutyCerpani', N'o {0} měsíců', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (143, 8, N'001', 130, N'ZmenaPodminekText', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (144, 8, N'001', 131, N'DetailniPopisText', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (145, 10, N'001', 29, N'RodneCisloText', N'Rodné číslo: ', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (146, 10, N'001', 29, N'RodneCislo', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (147, 10, N'001', 30, N'DatumNarozeniText', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (148, 10, N'001', 30, N'DatumNarozeni', N'', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (149, 10, N'001', 132, N'CelkovaVyseUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (150, 10, N'001', 133, N'SplatnostUveru', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (151, 10, N'001', 134, N'UcelCaska', N'{0:C}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (152, 10, N'001', 135, N'Zajisteni', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (153, 10, N'001', 136, N'LhutaUkonceniCerpani', N'o {0} měsíců', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (154, 10, N'001', 137, N'TerminPrvnihoCerpani', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (155, 10, N'001', 138, N'DoplnujiciKomentar', NULL, NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (156, 4, N'001', 8, N'Splatnost', N'{0:MonthsToYears}', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (157, 1, N'001', 2, N'Zajisteni', N'zástavním právem k pojištěné nemovitosti', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (158, 2, N'001', 28, N'Zajisteni', N'zástavním právem k pojištěné nemovitosti', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (159, 6, N'001', 142, N'UcetKeSplaceniText', N'Jsem si vědom/a toho, že pokud nejsem majitelem účtu uvedeného pro splácení úvěru, je před prvním Čerpáním nutné doložit souhlas majitele tohoto účtu.', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (160, 6, N'001', 143, N'Podpis', N'Podpis', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (161, 6, N'001', 143, N'PodpisKlient', N'Jméno:', NULL)
GO
INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull]) VALUES (162, 6, N'001', 143, N'PodpisZmocnenec', N'Jméno zmocněnce:', NULL)
GO
SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF
GO
INSERT [dbo].[InputParameter] ([InputParameterId], [InputParameterName]) VALUES (1, N'SalesArrangementId')
GO
INSERT [dbo].[InputParameter] ([InputParameterId], [InputParameterName]) VALUES (2, N'CaseId')
GO
INSERT [dbo].[InputParameter] ([InputParameterId], [InputParameterName]) VALUES (3, N'OfferId')
GO
INSERT [dbo].[InputParameter] ([InputParameterId], [InputParameterName]) VALUES (4, N'UserId')
GO
INSERT [dbo].[InputParameter] ([InputParameterId], [InputParameterName]) VALUES (5, N'CustomerIdentity')
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, N'001', 2, 2, 1)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (6, N'001', 2, 6, 1)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (8, N'001', 2, 6, 1)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (10, N'001', 2, 6, 1)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, N'001', 3, 3, 2)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (4, N'001', 3, 3, 2)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (5, N'001', 3, 3, 2)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, N'001', 4, 4, 3)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (6, N'001', 5, 5, 37)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (8, N'001', 5, 5, 139)
GO
INSERT [dbo].[DocumentDynamicInputParameter] ([DocumentId], [DocumentVersion], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (10, N'001', 5, 5, 140)
GO
SET IDENTITY_INSERT [dbo].[DocumentTable] ON 
GO
INSERT [dbo].[DocumentTable] ([DocumentTableId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldPlaceholderName], [ConcludingParagraph]) VALUES (2, 3, N'001', 99, N'SplatkovyKalendar', N'Upozorňujeme, že tento dokument má pouze informativní charakter. Uvedené splátky Úvěru jsou platné po dobu platnosti sjednané úrokové sazby. Uvedené údaje se mohou změnit, neboť závisí zejména na tom, kdy a v jaké výši úvěr vyčerpáte, na úrokové sazbě, apod.')
GO
SET IDENTITY_INSERT [dbo].[DocumentTable] OFF
GO
SET IDENTITY_INSERT [dbo].[DynamicStringFormat] ON 
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (1, 10, N'směřování Vašich příjmů na bankovní účet vedený u nás, a to alespoň ve výši 1,5 násobku výše splátky úvěru', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (2, 10, N'ne', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (4, 11, N'ano', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (5, 11, N'ne', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (7, 12, N'ano', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (8, 12, N'ne', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (10, 13, N'Výše prokázaného čistého příjmu od 50 000 Kč 
nebo výše úvěru od 5 000 000 Kč', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (12, 14, N'Pokud nám předložíte veškeré požadované podklady k Žádosti o úvěr tak, abychom ji mohli schválit do {0} garantujeme Vám a za předpokladu schválení Žádosti o úvěr a splnění výše uvedených podmínek úrokovou sazbu uvedenou v této nabídce. Seznam podkladů je uveden na: www.kb.cz.', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (13, 14, N'Tato nabídka má pouze informační charakter a úroková sazba zde uvedená není námi garantována.


Seznam podkladů k Žádosti o úvěr je možné nalézt zde: www.kb.cz.', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (14, 30, N'směřování Vašich příjmů na bankovní účet vedený u nás, a to alespoň ve výši 1,5 násobku výše splátky úvěru', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (15, 30, N'ne', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (17, 33, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (18, 8, N'--', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (19, 9, N'--', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (20, 31, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (21, 32, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (22, 122, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (23, 124, N'Datum narození:', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (24, 125, N'{0}', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (25, 132, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (26, 134, N'Datum narození:', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (27, 135, N'{0}', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (28, 145, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (29, 147, N'Datum narození:', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (30, 148, N'{0}', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (31, 97, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (32, 98, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (33, 99, N'ne', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (34, 13, N'ne', 2)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (35, 159, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (36, 160, N'Podpis na základě plné moci', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (37, 161, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (38, 162, N'', 1)
GO
INSERT [dbo].[DynamicStringFormat] ([DynamicStringFormatId], [DocumentDataFieldId], [Format], [Priority]) VALUES (39, 162, N'', 2)
GO
SET IDENTITY_INSERT [dbo].[DynamicStringFormat] OFF
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (1, N'4', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (1, N'DOMICILACE', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (1, N'1', 75)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (2, N'4', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (2, N'DOMICILACE', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (4, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (4, N'RZP', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (4, N'1', 75)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (5, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (5, N'RZP', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (7, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (7, N'POJIST_NEM', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (7, N'1', 75)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (8, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (8, N'POJIST_NEM', 73)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (10, N'True', 14)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (10, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (10, N'1', 75)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (12, N'0', 96)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (14, N'True', 97)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (17, N'False', 14)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (18, N'0', 11)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (19, N'0', 12)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (20, N'False', 103)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (21, N'False', 104)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (22, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (23, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (24, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (25, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (26, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (27, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (28, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (29, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (30, NULL, 29)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (31, N'False', 97)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (32, N'False', 103)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (33, N'False', 104)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (34, N'True', 14)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (34, N'1', 69)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (35, NULL, 142)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (36, N'True', 144)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (37, N'True', 144)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (38, NULL, 145)
GO
INSERT [dbo].[DynamicStringFormatCondition] ([DynamicStringFormatId], [EqualToValue], [DataFieldId]) VALUES (39, N'False', 144)
GO
INSERT [dbo].[EasRequestType] ([EasRequestTypeId], [EasRequestTypeName]) VALUES (1, N'Service')
GO
INSERT [dbo].[EasRequestType] ([EasRequestTypeId], [EasRequestTypeName]) VALUES (2, N'Product')
GO
INSERT [dbo].[EasFormType] ([EasFormTypeId], [EasFormTypeName], [Version], [ValidFrom], [ValidTo]) VALUES (1, N'F3700', 1, CAST(N'2023-01-01T15:18:02.0400000' AS DateTime2), CAST(N'2033-01-01T15:19:38.9466667' AS DateTime2))
GO
INSERT [dbo].[EasFormType] ([EasFormTypeId], [EasFormTypeName], [Version], [ValidFrom], [ValidTo]) VALUES (2, N'F3601', 1, CAST(N'2023-01-01T15:18:02.0400000' AS DateTime2), CAST(N'2033-01-01T15:19:38.9466667' AS DateTime2))
GO
INSERT [dbo].[EasFormType] ([EasFormTypeId], [EasFormTypeName], [Version], [ValidFrom], [ValidTo]) VALUES (3, N'F3602', 1, CAST(N'2023-01-01T15:18:02.0400000' AS DateTime2), CAST(N'2033-01-01T15:19:38.9466667' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[EasFormDataField] ON 
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (1, 1, 38, 1, N'cislo_smlouvy')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (2, 1, 1, 1, N'case_id')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (3, 1, 39, 1, N'datum_cerpani')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (4, 1, 40, 1, N'cerpani_bezodkladne')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (5, 1, 22, 1, N'datum_vygenerovani_dokumentu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (6, 1, 41, 1, N'uv_ucet_predcisli')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (7, 1, 42, 1, N'uv_ucet_cislo')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (8, 1, 43, 1, N'uv_ucet_kod_banky')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (11, 1, 141, 1, N'zadatel_o_cerpani.mp_id')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (12, 1, 29, 1, N'zadatel_o_cerpani.rodne_cislo_ico')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (13, 1, 46, 1, N'zadatel_o_cerpani.prijmeni_nazev')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (14, 1, 47, 1, N'zadatel_o_cerpani.jmeno')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (15, 1, 30, 1, N'zadatel_o_cerpani.datum_narozeni')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (16, 1, 48, 1, N'ucet_splaceni.inkaso_predcisli_uctu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (17, 1, 49, 1, N'ucet_splaceni.inkaso_cislo_uctu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (18, 1, 50, 1, N'ucet_splaceni.inkaso_kod_banky')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (19, 1, 52, 1, N'seznam_vyplat[].poradove_cislo')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (20, 1, 53, 1, N'seznam_vyplat[].predcislo_uctu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (21, 1, 54, 1, N'seznam_vyplat[].cislo_uctu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (22, 1, 55, 1, N'seznam_vyplat[].kod_banky')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (23, 1, 56, 1, N'seznam_vyplat[].castka')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (24, 1, 57, 1, N'seznam_vyplat[].vs')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (25, 1, 58, 1, N'seznam_vyplat[].ks')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (26, 1, 59, 1, N'seznam_vyplat[].ss')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (27, 2, 60, 2, N'cislo_smlouvy')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (28, 2, 1, 2, N'case_id')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (29, 2, 61, 2, N'business_case_ID')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (30, 2, 62, 2, N'risk_segment')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (31, 2, 63, 2, N'laa_id')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (32, 2, 64, 2, N'max_datum_uzavreni_obchodu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (33, 2, 65, 2, N'kanal_ziskani')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (34, 2, 22, 2, N'datum_vygenerovani_dokumentu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (35, 2, 10, 2, N'indikativni_LTV')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (36, 2, 66, 2, N'termin_cerpani_do')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (37, 2, 67, 2, N'sazba_vyhlasovana')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (38, 2, 68, 2, N'sazba_skladacka')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (39, 2, 7, 2, N'sazba_poskytnuta')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (40, 2, 69, 2, N'vyhlasovanaTyp')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (41, 2, 5, 2, N'vyse_uveru')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (42, 2, 9, 2, N'anuitni_splatka')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (43, 2, 71, 2, N'uv_zvyhodneni')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (44, 2, 8, 2, N'splatnost_uv_mesice')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (45, 2, 6, 2, N'fixace_uv_mesice')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (46, 2, 72, 2, N'predp_termin_cerpani')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (47, 2, 19, 2, N'den_splaceni')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (48, 2, 73, 2, N'seznam_mark_akci[].typMaAkce')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (49, 2, 74, 2, N'seznam_mark_akci[].zaskrtnuto')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (50, 2, 75, 2, N'seznam_mark_akci[].uplatnena')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (51, 2, 76, 2, N'seznam_mark_akci[].kodMaAkce')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (52, 2, 77, 2, N'seznam_mark_akci[].odchylkaSazby')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (53, 2, 78, 2, N'seznam_poplatku[].kod_poplatku')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (54, 2, 79, 2, N'seznam_poplatku[].suma_poplatku_sazebnik')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (55, 2, 80, 2, N'seznam_poplatku[].suma_poplatku_skladacka')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (56, 2, 81, 2, N'seznam_poplatku[].suma_poplatku_vysledna')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (57, 2, 82, 2, N'seznam_poplatku[].slevaIC')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (58, 2, 83, 2, N'seznam_poplatku[].kodMaAkce')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (59, 2, 84, 2, N'mena_prijmu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (60, 2, 85, 2, N'mena_bydliste')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (61, 2, 86, 2, N'zpusob_zasilani_vypisu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (62, 2, 87, 2, N'predp_hodnota_nem_zajisteni')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (63, 2, 88, 2, N'datum_garance_us')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (64, 2, 89, 2, N'garance_us_platnost_do')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (65, 2, 90, 2, N'zpusob_podpisu_smluv_dok')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (66, 2, 91, 2, N'souhlas_el_forma_komunikace')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (67, 2, 20, 2, N'datum_zahajeni_anuitniho_splaceni')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (68, 2, 93, 2, N'splatnost_uveru_datum')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (69, 2, 94, 2, N'pocet_anuitnich_splatek')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (70, 2, 11, 2, N'RZP_suma')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (71, 2, 60, 3, N'cislo_smlouvy')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (72, 2, 1, 3, N'case_id')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (73, 2, 61, 3, N'business_case_ID')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (74, 2, 22, 3, N'datum_vygenerovani_dokumentu')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (75, 2, 90, 3, N'zpusob_podpisu_smluv_dok')
GO
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (76, 2, 100, 2, N'uv_druh')
GO
SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF
GO
INSERT [dbo].[EasFormDynamicInputParameter] ([EasRequestTypeId], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, 2, 2, 1)
GO
INSERT [dbo].[EasFormDynamicInputParameter] ([EasRequestTypeId], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, 2, 6, 1)
GO
INSERT [dbo].[EasFormDynamicInputParameter] ([EasRequestTypeId], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (2, 3, 3, 2)
GO
INSERT [dbo].[EasFormDynamicInputParameter] ([EasRequestTypeId], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, 5, 5, 37)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'JmenoPrijmeni', 2, N'PersonName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'OstatniJmenoPrijmeni', 4, N'UserName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'OstatniTelEmail', 4, N'UserContacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'RpsnCastky', 3, N'FeeFinalSums', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'RpsnText', 3, N'FeeNames', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'TelEmail', 2, N'PersonContact', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'UcelUveru', 3, N'LoanPurposes', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'Zahlavi1Radek', 3, N'OfferHeader3', N'NABÍDKA – {0}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (1, N'ZahlaviDalsiStranky', 3, N'OfferHeader3', N'NABÍDKA – {0}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'OstaniTelEmail', 4, N'UserContacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'OstatniJmenoPrijmeni', 4, N'UserName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'RpsnCastky', 3, N'FeeFinalSums', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'RpsnText', 3, N'FeeNames', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'Zahlavi1Radek', 3, N'OfferHeader1', N'KALKULACE – {0}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'Zahlavi2Radky', 3, N'OfferHeader2', N'KALKULACE – {0}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (2, N'ZahlaviDalsiStranky', 3, N'OfferHeader3', N'KALKULACE – {0}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'DatumNarozeni', 10, N'DebtorCustomer.DateOfBirth', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'DobaCerpani', 3, N'DrawingDuration', N'{0:MonthsToYears}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'DokladTotoznosti', 10, N'DebtorCustomer.IdentificationType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'JmenoPrijmeni', 10, N'DebtorCustomer.FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'KontaktniAdresa', 10, N'DebtorCustomer.ContactAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'MesSplatkyHypUvery', 10, N'DebtorObligation.ObligationMLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'MesSplatkySpotrUvery', 10, N'DebtorObligation.ObligationCLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'MesSplatkyStavSpor', 10, N'DebtorObligation.ObligationML2Installment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'NesplJistinaHypUvery', 10, N'DebtorObligation.ObligationMLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'NesplJistinaSpotrUvery', 10, N'DebtorObligation.ObligationCLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'NesplJistinaStavSpor', 10, N'DebtorObligation.ObligationML2LoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'NovyLimitDebet', 10, N'DebtorObligation.CreditCardCorrectionAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'NovyLimitKreditniKarty', 10, N'DebtorObligation.CreditCardCorrectionCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'Ostatni', 10, N'DebtorIncome.IncomeOther', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'RodneCislo', 10, N'DebtorCustomer.BirthNumber', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimDebet', 10, N'DebtorObligation.ObligationADAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimKreditniKarty', 10, N'DebtorObligation.ObligationCCAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimPredHypUvery', 10, N'DebtorObligation.ObligationMLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimPredSpotrUvery', 10, N'DebtorObligation.ObligationCLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimPredStavSpor', 10, N'DebtorObligation.ObligationML2SumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimUveremHypUvery', 10, N'DebtorObligation.ObligationMLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimUveremSpotrUvery', 10, N'DebtorObligation.ObligationCLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SplatimUveremStavSpor', 10, N'DebtorObligation.ObligationML2AmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelDatumNarozeni', 10, N'CodebtorCustomer.DateOfBirth', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelDokladTotoznosti', 10, N'CodebtorCustomer.IdentificationType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelJmenoPrijmeni', 10, N'CodebtorCustomer.FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelKontaktniAdresa', 10, N'CodebtorCustomer.ContactAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelMesSplatkyHypUvery', 10, N'CodebtorObligation.ObligationMLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelMesSplatkySpotrUvery', 10, N'CodebtorObligation.ObligationCLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelMesSplatkyStavSpor', 10, N'CodebtorObligation.ObligationML2Installment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelNesplJistinaHypUvery', 10, N'CodebtorObligation.ObligationMLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelNesplJistinaSpotrUvery', 10, N'CodebtorObligation.ObligationCLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelNesplJistinaStavSpor', 10, N'CodebtorObligation.ObligationML2LoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelNovyLimitDebet', 10, N'CodebtorObligation.CreditCardCorrectionAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelNovyLimitKreditniKarty', 10, N'CodebtorObligation.CreditCardCorrectionCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelOstatni', 10, N'CodebtorIncome.IncomeOther', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelRodneCislo', 10, N'CodebtorCustomer.BirthNumber', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimDebet', 10, N'CodebtorObligation.ObligationADAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimKreditniKarty', 10, N'CodebtorObligation.ObligationCCAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimPredHypUvery', 10, N'CodebtorObligation.ObligationMLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimPredSpotrUvery', 10, N'CodebtorObligation.ObligationCLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimPredStavSpor', 10, N'CodebtorObligation.ObligationML2SumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimUveremHypUvery', 10, N'CodebtorObligation.ObligationMLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimUveremSpotrUvery', 10, N'CodebtorObligation.ObligationCLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelSplatimUveremStavSpor', 10, N'CodebtorObligation.ObligationML2AmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelStav', 10, N'CodebtorMaritalStatus', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelTelEmail', 10, N'CodebtorCustomer.Contacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelTrvalyPobyt', 10, N'CodebtorCustomer.PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelUpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelVyseLimituDebet', 10, N'CodebtorObligation.CreditCardLimitAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelVyseLimituKreditniKarty', 10, N'CodebtorObligation.CreditCardLimitCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelZeZamestnani', 10, N'CodebtorIncome.IncomeEmployment', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelZPodnikani', 10, N'CodebtorIncome.IncomeEnterprise', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelZPronajmu', 10, N'CodebtorIncome.IncomeRent', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelZrusimDebet', 10, N'CodebtorObligation.CreditCardCorrectionConsolidatedAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'SpoluzadatelZrusimKreditniKarty', 10, N'CodebtorObligation.CreditCardCorrectionConsolidatedCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'Stav', 10, N'DebtorMaritalStatus', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'TelEmail', 10, N'DebtorCustomer.Contacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'TrvalyPobyt', 10, N'DebtorCustomer.PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'TypUveru', 3, N'LoanType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'UcelUveru', 3, N'LoanPurposes', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'UdajeDomacnosti', 10, N'HeaderHouseholdData', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'UpravaSpolecnehoJmeni', 10, N'PropertySettlement', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'VydajeDomacnosti', 10, N'HeaderHouseholdExpenses', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'VyseLimituDebet', 10, N'DebtorObligation.CreditCardLimitAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'VyseLimituKreditniKarty', 10, N'DebtorObligation.CreditCardLimitCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'VyseUveruNeucelova', 3, N'LoanPurposes210', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'Zahlavi1Radek', 4, N'LoanApplicationHeader1', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'Zahlavi2Radky', 4, N'LoanApplicationHeader2', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZeZamestnani', 10, N'DebtorIncome.IncomeEmployment', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZPodnikani', 10, N'DebtorIncome.IncomeEnterprise', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZPronajmu', 10, N'DebtorIncome.IncomeRent', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZpusobCerpani', 3, N'DrawingTypeName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZrusimDebet', 10, N'DebtorObligation.CreditCardCorrectionConsolidatedAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (4, N'ZrusimKreditniKarty', 10, N'DebtorObligation.CreditCardCorrectionConsolidatedCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'DatumNarozeni', 11, N'Customer1.DateOfBirth', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'DokladTotoznosti', 11, N'Customer1.IdentificationType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'JmenoPrijmeni', 11, N'Customer1.FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'KontaktniAdresa', 11, N'Customer1.ContactAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'MesSplatkyHypUvery', 11, N'Customer1Obligation.ObligationMLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'MesSplatkySpotrUvery', 11, N'Customer1Obligation.ObligationCLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'MesSplatkyStavSpor', 11, N'Customer1Obligation.ObligationML2Installment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'NesplJistinaHypUvery', 11, N'Customer1Obligation.ObligationMLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'NesplJistinaSpotrUvery', 11, N'Customer1Obligation.ObligationCLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'NesplJistinaStavSpor', 11, N'Customer1Obligation.ObligationML2LoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'NovyLimitDebet', 11, N'Customer1Obligation.CreditCardCorrectionAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'NovyLimitKreditniKarty', 11, N'Customer1Obligation.CreditCardCorrectionCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'Ostatni', 11, N'Customer1Income.IncomeOther', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'RodneCislo', 11, N'Customer1.BirthNumber', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimDebet', 11, N'Customer1Obligation.ObligationADAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimKreditniKarty', 11, N'Customer1Obligation.ObligationCCAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimPredHypUvery', 11, N'Customer1Obligation.ObligationMLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimPredSpotrUvery', 11, N'Customer1Obligation.ObligationCLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimPredStavSpor', 11, N'Customer1Obligation.ObligationML2SumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimUveremHypUvery', 11, N'Customer1Obligation.ObligationMLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimUveremSpotrUvery', 11, N'Customer1Obligation.ObligationCLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SplatimUveremStavSpor', 11, N'Customer1Obligation.ObligationML2AmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelDatumNarozeni', 11, N'Customer2.DateOfBirth', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelDokladTotoznosti', 11, N'Customer2.IdentificationType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelJmenoPrijmeni', 11, N'Customer2.FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelKontaktniAdresa', 11, N'Customer2.ContactAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelMesSplatkyHypUvery', 11, N'Customer2Obligation.ObligationMLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelMesSplatkySpotrUvery', 11, N'Customer2Obligation.ObligationCLInstallment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelMesSplatkyStavSpor', 11, N'Customer2Obligation.ObligationML2Installment', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelNesplJistinaHypUvery', 11, N'Customer2Obligation.ObligationMLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelNesplJistinaSpotrUvery', 11, N'Customer2Obligation.ObligationCLLoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelNesplJistinaStavSpor', 11, N'Customer2Obligation.ObligationML2LoanPrincipal', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelNovyLimitDebet', 11, N'Customer2Obligation.CreditCardCorrectionAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelNovyLimitKreditniKarty', 11, N'Customer2Obligation.CreditCardCorrectionCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelOstatni', 11, N'Customer2Income.IncomeOther', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelRodneCislo', 11, N'Customer2.BirthNumber', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimDebet', 11, N'Customer2Obligation.ObligationADAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimKreditniKarty', 11, N'Customer2Obligation.ObligationCCAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimPredHypUvery', 11, N'Customer2Obligation.ObligationMLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimPredSpotrUvery', 11, N'Customer2Obligation.ObligationCLSumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimPredStavSpor', 11, N'Customer2Obligation.ObligationML2SumWithCorrection', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimUveremHypUvery', 11, N'Customer2Obligation.ObligationMLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimUveremSpotrUvery', 11, N'Customer2Obligation.ObligationCLAmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelSplatimUveremStavSpor', 11, N'Customer2Obligation.ObligationML2AmountConsolidated', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelStav', 11, N'Customer2MaritalStatus', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelTelEmail', 11, N'Customer2.Contacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelTrvalyPobyt', 11, N'Customer2.PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelUpravaSpolecnehoJmeni', 11, N'PropertySettlement', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelVyseLimituDebet', 11, N'Customer2Obligation.CreditCardLimitAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelVyseLimituKreditniKarty', 11, N'Customer2Obligation.CreditCardLimitCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelZeZamestnani', 11, N'Customer2Income.IncomeEmployment', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelZPodnikani', 11, N'Customer2Income.IncomeEnterprise', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelZPronajmu', 11, N'Customer2Income.IncomeRent', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelZrusimDebet', 11, N'Customer2Obligation.CreditCardCorrectionConsolidatedAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'SpoluzadatelZrusimKreditniKarty', 11, N'Customer2Obligation.CreditCardCorrectionConsolidatedCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'Stav', 11, N'Customer1MaritalStatus', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'TelEmail', 11, N'Customer1.Contacts', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'TrvalyPobyt', 11, N'Customer1.PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'TypUveru', 3, N'LoanType', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'UcelUveru', 3, N'LoanPurposes', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'UpravaSpolecnehoJmeni', 11, N'PropertySettlement', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'VyseLimituDebet', 11, N'Customer1Obligation.CreditCardLimitAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'VyseLimituKreditniKarty', 11, N'Customer1Obligation.CreditCardLimitCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'Zahlavi1Radek', 4, N'LoanApplicationHeader1', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'Zahlavi2Radky', 4, N'LoanApplicationHeader2', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'ZeZamestnani', 11, N'Customer1Income.IncomeEmployment', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'ZPodnikani', 11, N'Customer1Income.IncomeEnterprise', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'ZPronajmu', 11, N'Customer1Income.IncomeRent', N'{0} Kč/měsíčně')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'ZrusimDebet', 11, N'Customer1Obligation.CreditCardCorrectionConsolidatedAD', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (5, N'ZrusimKreditniKarty', 11, N'Customer1Obligation.CreditCardCorrectionConsolidatedCC', N'{0:C}')
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'CisloUverovahoUctu', 6, N'PaymentAccount', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'JmenoPrijmeni', 5, N'PersonName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'PodpisJmenoKlienta', 1, N'SignPersonName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'PodpisJmenoZmocnence', 1, N'SignAgentName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'TrvalyPobyt', 5, N'PersonAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (6, N'UcetKeSplaceni', 1, N'RepaymentAccount', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'CisloUverovehoUctu', 6, N'PaymentAccount', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'JmenoPrijmeni', 5, N'FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'MajitelUctu', 1, N'RepaymentAccountOwner', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'NoveCisloUctu', 1, N'RepaymentAccount', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'TrvalyPobyt', 5, N'PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'TypNemovitosti', 1, N'RealEstateTypes', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (8, N'UcelPorizeni', 1, N'RealEstatePurchaseTypes', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (10, N'CisloUverUctu', 6, N'PaymentAccount', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (10, N'JmenoPrijmeni', 5, N'FullName', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (10, N'ObjektUveru', 1, N'LoanRealEstates[]', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (10, N'TrvalyPobyt', 5, N'PermanentAddress', NULL)
GO
INSERT [dbo].[DocumentSpecialDataField] ([DocumentId], [AcroFieldName], [DataServiceId], [FieldPath], [StringFormat]) VALUES (10, N'Ucel', 1, N'LoanPurposes[]', NULL)
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'business_id_formulare', 0, 1, N'DynamicFormValues.FormId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'cislo_dokumentu', 0, 1, N'MockValues.MockDocumentId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'podpis_zadatele', 0, 1, N'MockValues.DefaultOneValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'sjednal_CPM', 0, 1, N'MockValues.UserCPM')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'sjednal_ICP', 0, 1, N'MockValues.UserICP')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'ucet_splaceni.zpusob_splaceni', 0, 1, N'MockValues.DefaultOneValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'zadatel_o_cerpani.kb_id', 5, 1, N'IdentityKb')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'zadatel_o_cerpani.titul_pred', 5, 1, N'DegreeBefore')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'zmocnena_osoba', 1, 1, N'IsAgent')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (1, N'zpusob_podpisu_zadosti', 0, 1, N'MockValues.DefaultOneValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'business_id_formulare', 0, 2, N'DynamicFormValues.FormId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'business_id_formulare', 0, 3, N'DynamicFormValues.FormId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'cislo_dokumentu', 0, 2, N'MockValues.MockDocumentId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'cislo_dokumentu', 0, 3, N'MockValues.MockDocumentId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'datum_prvniho_podpisu', 1, 2, N'FirstSignedDate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'developer_id', 3, 2, N'ConditionalFormValues.DeveloperId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'developer_popis', 3, 2, N'ConditionalFormValues.DeveloperDescription')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'developer_projekt_id', 3, 2, N'ConditionalFormValues.DeveloperProjectId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'ea_kod', 0, 2, N'DefaultValues3601.PasswordCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'ea_kod', 0, 3, N'DefaultValues3602.PasswordCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'fin_kryti_celkem', 3, 2, N'ConditionalFormValues.FinancialResourcesTotal')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'fin_kryti_cizi_zdroje', 3, 2, N'ConditionalFormValues.FinancialResourcesOther')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'fin_kryti_vlastni_zdroje', 3, 2, N'ConditionalFormValues.FinancialResourcesOwn')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'forma_splaceni', 0, 2, N'MockValues.DefaultOneValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'individualni_cenotvorba_odchylka', 1, 2, N'InterestRateDiscount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'lhuta_ukonceni_cerpani', 3, 2, N'DrawingDurationId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'pojisteni_nem_suma', 3, 2, N'ConditionalFormValues.InsuranceSumRealEstate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].cislo_domacnosti', 0, 2, N'HouseholdList[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].cislo_domacnosti', 0, 3, N'HouseholdList[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].naklady_na_bydleni', 0, 2, N'HouseholdList[].HousingExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].naklady_na_bydleni', 0, 3, N'HouseholdList[].HousingExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].ostatni_vydaje', 0, 2, N'HouseholdList[].OtherExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].ostatni_vydaje', 0, 3, N'HouseholdList[].OtherExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pocet_deti_0_10let', 0, 2, N'HouseholdList[].ChildrenUpToTenYearsCount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pocet_deti_0_10let', 0, 3, N'HouseholdList[].ChildrenUpToTenYearsCount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pocet_deti_nad_10let', 0, 2, N'HouseholdList[].ChildrenOverTenYearsCount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pocet_deti_nad_10let', 0, 3, N'HouseholdList[].ChildrenOverTenYearsCount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pojisteni', 0, 2, N'HouseholdList[].InsuranceExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].pojisteni', 0, 3, N'HouseholdList[].InsuranceExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].sporeni', 0, 2, N'HouseholdList[].SavingExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].sporeni', 0, 3, N'HouseholdList[].SavingExpenseAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].vyporadani_majetku', 0, 2, N'HouseholdList[].PropertySettlementId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_domacnosti[].vyporadani_majetku', 0, 3, N'HouseholdList[].PropertySettlementId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_id_formulare[].id_formulare', 0, 2, N'MockValues.ListIdForm[].Id')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_id_formulare[].id_formulare', 0, 3, N'MockValues.ListIdForm[].Id')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_objektu[].cislo_objektu_uveru', 1, 2, N'ConditionalFormValues.LoanRealEstates[].RowNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_objektu[].objekt_uv_je_zajisteni', 1, 2, N'ConditionalFormValues.LoanRealEstates[].LoanRealEstateData.IsCollateral')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_objektu[].typ_nemovitosti', 1, 2, N'ConditionalFormValues.LoanRealEstates[].LoanRealEstateData.RealEstateTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_objektu[].ucel_porizeni', 1, 2, N'ConditionalFormValues.LoanRealEstates[].LoanRealEstateData.RealEstatePurchaseTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].cislo_domacnosti', 1, 2, N'HouseholdData.Customers[].HouseholdNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].cislo_domacnosti', 1, 3, N'HouseholdData.Customers[].HouseholdNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.datum_narozeni', 1, 2, N'HouseholdData.Customers[].NaturalPerson.DateOfBirth')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.datum_narozeni', 1, 3, N'HouseholdData.Customers[].NaturalPerson.DateOfBirth')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.datum_posledniho_uzam_prijmu', 1, 2, N'HouseholdData.Customers[].LockedIncomeDateTime')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.datum_posledniho_uzam_prijmu', 1, 3, N'HouseholdData.Customers[].LockedIncomeDateTime')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.druh_druzka', 1, 2, N'HouseholdData.Customers[].IsPartner')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.druh_druzka', 1, 3, N'HouseholdData.Customers[].IsPartner')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_blizka_osoba_banka', 1, 2, N'HouseholdData.Customers[].HasRelationshipWithKBEmployee')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_blizka_osoba_banka', 1, 3, N'HouseholdData.Customers[].HasRelationshipWithKBEmployee')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_fatca', 1, 2, N'HouseholdData.Customers[].IsUSPerson')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_fatca', 1, 3, N'HouseholdData.Customers[].IsUSPerson')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_propojeni_korporace', 1, 2, N'HouseholdData.Customers[].HasRelationshipWithCorporate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_propojeni_korporace', 1, 3, N'HouseholdData.Customers[].HasRelationshipWithCorporate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_zvlastni_vztah_banka', 1, 2, N'HouseholdData.Customers[].HasRelationshipWithKB')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.je_zvlastni_vztah_banka', 1, 3, N'HouseholdData.Customers[].HasRelationshipWithKB')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.jmeno', 1, 2, N'HouseholdData.Customers[].NaturalPerson.FirstName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.jmeno', 1, 3, N'HouseholdData.Customers[].NaturalPerson.FirstName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.kb_id', 1, 2, N'HouseholdData.Customers[].IdentityKb')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.kb_id', 1, 3, N'HouseholdData.Customers[].IdentityKb')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.misto_narozeni_obec', 1, 2, N'HouseholdData.Customers[].NaturalPerson.PlaceOfBirth')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.misto_narozeni_obec', 1, 3, N'HouseholdData.Customers[].NaturalPerson.PlaceOfBirth')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.misto_narozeni_stat', 1, 2, N'HouseholdData.Customers[].NaturalPerson.BirthCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.misto_narozeni_stat', 1, 3, N'HouseholdData.Customers[].NaturalPerson.BirthCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.mp_id', 1, 2, N'HouseholdData.Customers[].IdentityMp')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.mp_id', 1, 3, N'HouseholdData.Customers[].IdentityMp')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.PEP', 1, 2, N'HouseholdData.Customers[].IsPoliticallyExposed')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.PEP', 1, 3, N'HouseholdData.Customers[].IsPoliticallyExposed')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pohlavi', 1, 2, N'HouseholdData.Customers[].GenderCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pohlavi', 1, 3, N'HouseholdData.Customers[].GenderCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pravni_omezeni_do', 1, 2, N'HouseholdData.Customers[].CustomerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionUntil')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pravni_omezeni_do', 1, 3, N'HouseholdData.Customers[].CustomerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionUntil')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pravni_omezeni_typ', 1, 2, N'HouseholdData.Customers[].RestrictionType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.pravni_omezeni_typ', 1, 3, N'HouseholdData.Customers[].RestrictionType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.poradi_prijmu', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.poradi_prijmu', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.prijem_mena', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.prijem_mena', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.prijem_vyse', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.prijem_vyse', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.rc_ico', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.IdentificationNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.rc_ico', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.IdentificationNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.sidlo_stat', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.Entrepreneur.CountryOfResidenceId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.sidlo_stat', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.Entrepreneur.CountryOfResidenceId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.typ_dokumentu', 1, 2, N'HouseholdData.Customers[].IncomeEntrepreneur.DocumentType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_dp.typ_dokumentu', 1, 3, N'HouseholdData.Customers[].IncomeEntrepreneur.DocumentType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.poradi_prijmu', 1, 2, N'HouseholdData.Customers[].IncomeRent.Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.poradi_prijmu', 1, 3, N'HouseholdData.Customers[].IncomeRent.Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.prijem_mena', 1, 2, N'HouseholdData.Customers[].IncomeRent.CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.prijem_mena', 1, 3, N'HouseholdData.Customers[].IncomeRent.CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.prijem_vyse', 1, 2, N'HouseholdData.Customers[].IncomeRent.IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijem_naj.prijem_vyse', 1, 3, N'HouseholdData.Customers[].IncomeRent.IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijmeni_nazev', 1, 2, N'HouseholdData.Customers[].NaturalPerson.LastName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijmeni_nazev', 1, 3, N'HouseholdData.Customers[].NaturalPerson.LastName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijmeni_rodne', 1, 2, N'HouseholdData.Customers[].NaturalPerson.BirthName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.prijmeni_rodne', 1, 3, N'HouseholdData.Customers[].NaturalPerson.BirthName')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rezident', 1, 2, N'HouseholdData.Customers[].IsResident')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rezident', 1, 3, N'HouseholdData.Customers[].IsResident')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rodinny_stav', 1, 2, N'HouseholdData.Customers[].NaturalPerson.MaritalStatusStateId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rodinny_stav', 1, 3, N'HouseholdData.Customers[].NaturalPerson.MaritalStatusStateId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rodne_cislo', 1, 2, N'HouseholdData.Customers[].NaturalPerson.BirthNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.rodne_cislo', 1, 3, N'HouseholdData.Customers[].NaturalPerson.BirthNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.segment', 1, 2, N'HouseholdData.Customers[].NaturalPerson.Segment')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.segment', 1, 3, N'HouseholdData.Customers[].NaturalPerson.Segment')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].adresni_bod_id', 1, 2, N'HouseholdData.Customers[].Addresses[].AddressPointId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].adresni_bod_id', 1, 3, N'HouseholdData.Customers[].Addresses[].AddressPointId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cast_obce', 1, 2, N'HouseholdData.Customers[].Addresses[].CityDistrict')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cast_obce', 1, 3, N'HouseholdData.Customers[].Addresses[].CityDistrict')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_evidencni', 1, 2, N'HouseholdData.Customers[].Addresses[].EvidenceNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_evidencni', 1, 3, N'HouseholdData.Customers[].Addresses[].EvidenceNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_orientacni', 1, 2, N'HouseholdData.Customers[].Addresses[].StreetNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_orientacni', 1, 3, N'HouseholdData.Customers[].Addresses[].StreetNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_popisne', 1, 2, N'HouseholdData.Customers[].Addresses[].HouseNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].cislo_popisne', 1, 3, N'HouseholdData.Customers[].Addresses[].HouseNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].misto', 1, 2, N'HouseholdData.Customers[].Addresses[].City')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].misto', 1, 3, N'HouseholdData.Customers[].Addresses[].City')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].obvod_praha', 1, 2, N'HouseholdData.Customers[].Addresses[].PragueDistrict')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].obvod_praha', 1, 3, N'HouseholdData.Customers[].Addresses[].PragueDistrict')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].psc', 1, 2, N'HouseholdData.Customers[].Addresses[].Postcode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].psc', 1, 3, N'HouseholdData.Customers[].Addresses[].Postcode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].stat', 1, 2, N'HouseholdData.Customers[].Addresses[].CountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].stat', 1, 3, N'HouseholdData.Customers[].Addresses[].CountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].typ_adresy', 1, 2, N'HouseholdData.Customers[].Addresses[].AddressTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].typ_adresy', 1, 3, N'HouseholdData.Customers[].Addresses[].AddressTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].ulice', 1, 2, N'HouseholdData.Customers[].Addresses[].Street')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].ulice', 1, 3, N'HouseholdData.Customers[].Addresses[].Street')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].ulice_dodatek', 1, 2, N'HouseholdData.Customers[].Addresses[].DeliveryDetails')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].ulice_dodatek', 1, 3, N'HouseholdData.Customers[].Addresses[].DeliveryDetails')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].uzemni_celek', 1, 2, N'HouseholdData.Customers[].Addresses[].CountrySubdivision')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_adres[].uzemni_celek', 1, 3, N'HouseholdData.Customers[].Addresses[].CountrySubdivision')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].cislo_dokladu', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].cislo_dokladu', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].platnost_do', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].ValidTo')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].platnost_do', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].ValidTo')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].typ_dokladu', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].IdentificationDocumentTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].typ_dokladu', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].IdentificationDocumentTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].IssuedBy')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].IssuedBy')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal_datum', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].IssuedOn')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal_datum', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].IssuedOn')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal_stat', 1, 2, N'HouseholdData.Customers[].IdentificationDocuments[].IssuingCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_dokladu[].vydal_stat', 1, 3, N'HouseholdData.Customers[].IdentificationDocuments[].IssuingCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_kontaktu[].hodnota_kontaktu', 1, 2, N'HouseholdData.Customers[].Contacts[].Value')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_kontaktu[].hodnota_kontaktu', 1, 3, N'HouseholdData.Customers[].Contacts[].Value')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_kontaktu[].typ_kontaktu', 1, 2, N'HouseholdData.Customers[].Contacts[].ContactTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_kontaktu[].typ_kontaktu', 1, 3, N'HouseholdData.Customers[].Contacts[].ContactTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].poradi_prijmu', 1, 2, N'HouseholdData.Customers[].IncomesOther[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].poradi_prijmu', 1, 3, N'HouseholdData.Customers[].IncomesOther[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].prijem_mena', 1, 2, N'HouseholdData.Customers[].IncomesOther[].CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].prijem_mena', 1, 3, N'HouseholdData.Customers[].IncomesOther[].CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].prijem_vyse', 1, 2, N'HouseholdData.Customers[].IncomesOther[].IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].prijem_vyse', 1, 3, N'HouseholdData.Customers[].IncomesOther[].IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].zdroj_prijmu_ostatni', 1, 2, N'HouseholdData.Customers[].IncomesOther[].IncomeOtherTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_ost[].zdroj_prijmu_ostatni', 1, 3, N'HouseholdData.Customers[].IncomesOther[].IncomeOtherTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].hruby_rocny_prijem', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.GrossAnnualIncome')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].hruby_rocny_prijem', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.GrossAnnualIncome')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].klient_ve_vypovedni_lhute', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.IsInProbationaryPeriod')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].klient_ve_vypovedni_lhute', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.IsInProbationaryPeriod')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].klient_ve_zkusebni_lhute', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.IsInTrialPeriod')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].klient_ve_zkusebni_lhute', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.IsInTrialPeriod')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].poradi_prijmu', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].poradi_prijmu', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].pracovni_smlouva_aktualni_do', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.CurrentWorkContractTo')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].pracovni_smlouva_aktualni_do', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.CurrentWorkContractTo')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].pracovni_smlouva_aktualni_od', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.CurrentWorkContractSince')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].pracovni_smlouva_aktualni_od', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.CurrentWorkContractSince')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_mena', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_mena', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].CurrencyCode')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_datum', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationDate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_datum', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationDate')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_kontakt', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationContact')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_kontakt', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationContact')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_osoba', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationPerson')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_osoba', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.ConfirmationPerson')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_vystavila_ext_firma', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.IsIssuedByExternalAccountant')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_potvrzeni_vystavila_ext_firma', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.IncomeConfirmation.IsIssuedByExternalAccountant')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_vyse', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_vyse', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].IncomeSum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_ze_zahranici_zpusob_vykonu', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.ForeignIncomeTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prijem_ze_zahranici_zpusob_vykonu', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.ForeignIncomeTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prvni_pracovni_sml_od', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.FirstWorkContractSince')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].prvni_pracovni_sml_od', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.FirstWorkContractSince')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_ostatni', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionOther')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_ostatni', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionOther')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_rozhodnuti', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionDecision')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_rozhodnuti', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionDecision')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_splatky', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionPayments')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].srazky_ze_mzdy_splatky', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.WageDeduction.DeductionPayments')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].typ_dokumentu', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].DocumentType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].typ_dokumentu', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].DocumentType')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].typ_pracovniho_pomeru', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].EmploymentTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].typ_pracovniho_pomeru', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].EmploymentTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnan_jako', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.JobDescription')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnan_jako', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Job.JobDescription')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_nazov', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Employer.Name')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_nazov', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Employer.Name')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_rc_ico', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].EmployerIdentificationNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_rc_ico', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].EmployerIdentificationNumber')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_sidlo_stat', 1, 2, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Employer.CountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_prijmu_zam[].zamestnavatel_sidlo_stat', 1, 3, N'HouseholdData.Customers[].IncomesEmployment[].Employment.Employer.CountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].castecna_konsolidace', 1, 2, N'HouseholdData.Customers[].Obligations[].PartialCorrection')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].castecna_konsolidace', 1, 3, N'HouseholdData.Customers[].Obligations[].PartialCorrection')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].cislo_zavazku', 1, 2, N'HouseholdData.Customers[].Obligations[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].cislo_zavazku', 1, 3, N'HouseholdData.Customers[].Obligations[].Number')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].druh_zavazku', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.ObligationTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].druh_zavazku', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.ObligationTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].mimo_entitu_mandanta', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.IsExternal')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].mimo_entitu_mandanta', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.IsExternal')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].veritel_kod_banky', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.CreditorId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].veritel_kod_banky', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.CreditorId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].veritel_nazev', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.Name')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].veritel_nazev', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.Creditor.Name')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_konsolid_jistiny', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.AmountConsolidated')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_konsolid_jistiny', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.AmountConsolidated')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_korekce_zavazku_o_jistina', 1, 2, N'HouseholdData.Customers[].Obligations[].Correction')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_korekce_zavazku_o_jistina', 1, 3, N'HouseholdData.Customers[].Obligations[].Correction')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_korekce_zavazku_o_spl', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.Correction.InstallmentAmountCorrection')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_korekce_zavazku_o_spl', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.Correction.InstallmentAmountCorrection')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_limitu', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.CreditCardLimit')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_limitu', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.CreditCardLimit')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_nesplacene_jistiny', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.LoanPrincipalAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_nesplacene_jistiny', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.LoanPrincipalAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_splatky', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.InstallmentAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].vyse_splatky', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.InstallmentAmount')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].zpusob_korekce_zavazku', 1, 2, N'HouseholdData.Customers[].Obligations[].ObligationData.Correction.CorrectionTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.seznam_zavazku[].zpusob_korekce_zavazku', 1, 3, N'HouseholdData.Customers[].Obligations[].ObligationData.Correction.CorrectionTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.statni_prislusnost', 1, 2, N'HouseholdData.Customers[].CitizenshipCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.statni_prislusnost', 1, 3, N'HouseholdData.Customers[].CitizenshipCountryId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.titul_pred', 1, 2, N'HouseholdData.Customers[].DegreeBefore')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.titul_pred', 1, 3, N'HouseholdData.Customers[].DegreeBefore')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.uzamcene_prijmy', 1, 2, N'HouseholdData.Customers[].HasLockedIncomeDateTime')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.uzamcene_prijmy', 1, 3, N'HouseholdData.Customers[].HasLockedIncomeDateTime')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.vzdelani', 1, 2, N'HouseholdData.Customers[].NaturalPerson.EducationLevelId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].klient.vzdelani', 1, 3, N'HouseholdData.Customers[].NaturalPerson.EducationLevelId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].role', 1, 2, N'HouseholdData.Customers[].CustomerOnSA.CustomerRoleId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucastniku[].role', 1, 3, N'HouseholdData.Customers[].CustomerOnSA.CustomerRoleId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucelu[].uv_ucel', 3, 2, N'ConditionalFormValues.LoanPurposes[].LoanPurposeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'seznam_ucelu[].uv_ucel_suma', 3, 2, N'ConditionalFormValues.LoanPurposes[].Sum')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'sjednal_CPM', 0, 2, N'MockValues.UserCPM')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'sjednal_CPM', 0, 3, N'MockValues.UserCPM')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'sjednal_ICP', 0, 2, N'MockValues.UserICP')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'sjednal_ICP', 0, 3, N'MockValues.UserICP')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'stav_zadosti', 1, 2, N'SalesArrangementStateId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'typ_cerpani', 3, 2, N'DrawingTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'uv_produkt', 1, 2, N'ProductTypeId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'zadaZvyhodneni', 3, 2, N'IsEmployeeBonusRequested')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'zmenovy_navrh', 0, 3, N'MockValues.DefaultZeroValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'zmocnenec_mp_id', 1, 2, N'MpIdentityId')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'zprostredkovano_3_stranou', 0, 2, N'MockValues.DefaultFalseValue')
GO
INSERT [dbo].[EasFormSpecialDataField] ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath]) VALUES (2, N'zpusob_podpisu_zadosti', 0, 2, N'MockValues.DefaultOneValue')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'Amount', 18, 3, NULL, N'Výše splátky (Kč)')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'Date', 14, 2, NULL, N'Datum splátky')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'Interest', 16, 5, NULL, N'Z toho úrok (Kč)')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'PaymentNumber', 13, 1, NULL, N'Číslo splátky')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'Principal', 18, 4, NULL, N'Z toho jistina (Kč)')
GO
INSERT [dbo].[DocumentTableColumn] ([DocumentTableId], [FieldPath], [WidthPercentage], [Order], [StringFormat], [Header]) VALUES (2, N'RemainingPrincipal', 21, 6, NULL, N'Zůstatek jistiny (Kč)')
GO
