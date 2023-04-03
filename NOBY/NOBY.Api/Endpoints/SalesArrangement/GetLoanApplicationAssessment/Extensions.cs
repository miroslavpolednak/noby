using cLA = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using cRB = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using cRS = DomainServices.RiskIntegrationService.Contracts.Shared;

using cHousehold = DomainServices.HouseholdService.Contracts;
using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCis = CIS.Infrastructure.gRPC.CisTypes;
using cCustomer = DomainServices.CustomerService.Contracts;
using CIS.Foms.Enums;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{

    public static GetLoanApplicationAssessmentResponse ToApiResponse(this DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse response, cOffer.GetMortgageOfferDetailResponse? offer)
    {
        //https://wiki.kb.cz/pages/viewpage.action?pageId=464683017

        return new GetLoanApplicationAssessmentResponse
        {
            Application = new()
            {
                Limit = response.Detail?.Limit?.Limit?.Amount,
                InstallmentLimit = response.Detail?.Limit?.InstallmentLimit?.Amount,
                RemainingAnnuityLivingAmount = response.Detail?.Limit?.RemainingAnnuityLivingAmount?.Amount,
                MonthlyIncome = response.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
                MonthlyCostsWithoutInstallments = response.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
                MonthlyInstallmentsInKB = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
                MonthlyEntrepreneurInstallmentsInKB = response.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
                MonthlyInstallmentsInMPSS = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
                MonthlyInstallmentsInOFI = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
                MonthlyInstallmentsInCBCB = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
                CIR = response.Detail?.Limit?.Cir,
                DTI = response.Detail?.Limit?.Dti,
                DSTI = response.Detail?.Limit?.Dsti,
                LTCP = response.CollateralRiskCharacteristics?.Ltp,
                LFTV = response.CollateralRiskCharacteristics?.Lftv,
                LTV = response.CollateralRiskCharacteristics?.Ltv,
                LoanAmount = offer?.SimulationResults?.LoanAmount!,
                LoanPaymentAmount = offer?.SimulationResults?.LoanPaymentAmount,
            },
            Households = response?.HouseholdsDetails?.Select(h => new Dto.Household
            {   
                HouseholdId = h.HouseholdId,
                MonthlyIncome = h.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
                MonthlyCostsWithoutInstallments = h.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
                MonthlyInstallmentsInMPSS = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
                MonthlyInstallmentsInOFI = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
                MonthlyInstallmentsInCBCB = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
                MonthlyInstallmentsInKBAmount = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
                MonthlyEntrepreneurInstallmentsInKBAmount = h.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
                CIR = h.Detail?.Limit?.Cir,
                DTI = h.Detail?.Limit?.Dti,
                DSTI = h.Detail?.Limit?.Dsti,
                LoanApplicationLimit = h.Detail?.Limit?.Limit?.Amount,
                LoanApplicationInstallmentLimit = h.Detail?.Limit?.InstallmentLimit?.Amount,
            }).ToList(),
            RiskBusinesscaseExpirationDate = response!.RiskBusinessCaseExpirationDate,
            AssessmentResult = response!.AssessmentResult,
            Reasons = response!.Reasons?.Select(r => new Dto.AssessmentReason
            {
                Code = r.Code,
                Description = r.Description,
                Level = r.Level,
                Result = r.Result,
                Target = r.Target,
                Weight = r.Weight,
            }).ToList()
        };
    }

    private static string? RemoveSpaces(this string value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new string(value.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
    }

    public static cLA.LoanApplicationSaveRequest ToLoanApplicationSaveRequest(this LoanApplicationData data)
    {
        // https://wiki.kb.cz/display/HT/RIP%28v2%29+-+POST+LoanApplication
        // https://wiki.kb.cz/display/HT/RIP%28v2%29+-+POST+LoanApplicationAssessment

        cLA.LoanApplicationHousehold MapHousehold(cHousehold.Household h)
        {
            cLA.LoanApplicationCustomer MapCustomer(cHousehold.CustomerOnSA cOnSA, bool isPartner)
            {
                var obligationTypeAmountIds = data.ObligationTypeIdsByObligationProperty["amount"] ?? new List<int>();

                var MapAddress = (cCis.GrpcAddress a) => new cRS.AddressDetail
                {
                    Street = a.Street,
                    StreetNumber = a.StreetNumber,
                    HouseNumber = a.HouseNumber,
                    EvidenceNumber = a.EvidenceNumber,
                    City = a.City,
                    CountryId = a.CountryId,
                    Postcode = a.Postcode.RemoveSpaces(),
                };

                var MapIdentificationDocument = (cCustomer.IdentificationDocument id) => new cRS.V1.IdentificationDocumentDetail
                {
                    DocumentNumber = id.Number,
                    IdentificationDocumentTypeId = id.IdentificationDocumentTypeId,
                    IssuedOn = id.IssuedOn,
                    ValidTo = id.ValidTo,
                };

                var MapObligation = (cHousehold.Obligation o) => new cLA.LoanApplicationObligation
                {
                    ObligationTypeId = o.ObligationTypeId!.Value,
                    Amount = obligationTypeAmountIds.Contains(o.ObligationTypeId!.Value) ? o.LoanPrincipalAmount : o.CreditCardLimit, // Pro Obligation.ObligationTypeId s hodnotami "1", "2" a "5" poslat hodnotu z: Obligation.LoanPrincipalAmount; Pro Obligation.ObligationTypeId s hodnotami "3" a "4" poslat hodnotu z: Obligation.CreditCardLimit;
                    AmountConsolidated = obligationTypeAmountIds.Contains(o.ObligationTypeId!.Value) ? o.Correction.LoanPrincipalAmountCorrection : o.Correction.CreditCardLimitCorrection,  // Pro Obligation.ObligationTypeId s hodnotami "1", "2" a "5" poslat hodnotu z: Obligation.Correction.LoanPrincipalAmountCorrection; Pro Obligation.ObligationTypeId s hodnotami "3" a "4" poslat hodnotu z: Obligation.Correction.CreditCardLimitCorrection;
                    Installment = o.InstallmentAmount,
                    InstallmentConsolidated = o.Correction.InstallmentAmountCorrection,
                };

                cLA.LoanApplicationIncome MapIncome()
                {

                    cLA.LoanApplicationEmploymentIncome MapEmploymentIncome(cHousehold.IncomeInList iil)
                    {
                        var i = data.IncomesById[iil.IncomeId];

                        return new cLA.LoanApplicationEmploymentIncome
                        {
                            EmployerIdentificationNumber = new List<string?> { i.Employement?.Employer?.Cin, i.Employement?.Employer?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),
                            EmployerName = i.Employement?.Employer?.Name ?? "",
                            Address = new cRS.AddressDetail
                            {
                                CountryId = i.Employement?.Employer?.CountryId,
                            },
                            JobDescription = i.Employement?.Job?.JobDescription,
                            MonthlyIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                            },
                            ProofTypeId = 1, // Default = 1 (PVP - Potvrzení o příjmu), ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            IncomeForeignTypeId = i.Employement?.ForeignIncomeTypeId,
                            GrossAnnualIncome = i.Employement?.Job?.GrossAnnualIncome,
                            ConfirmationPerson = i.Employement?.IncomeConfirmation?.ConfirmationPerson,
                            ConfirmationContactPhone = i.Employement?.IncomeConfirmation?.ConfirmationContact,
                            ConfirmationDate = i.Employement?.IncomeConfirmation?.ConfirmationDate,
                            JobTrialPeriod = (i.Employement?.Job?.IsInTrialPeriod == true),
                            NoticePeriod = (i.Employement?.Job?.IsInProbationaryPeriod == true),
                            EmploymentTypeId = i.Employement?.Job?.EmploymentTypeId,
                            FirstWorkContractSince = i.Employement?.Job?.FirstWorkContractSince,
                            CurrentWorkContractSince = i.Employement?.Job?.CurrentWorkContractSince,
                            CurrentWorkContractTo = i.Employement?.Job?.CurrentWorkContractTo,
                            ConfirmationByCompany = (i.Employement?.IncomeConfirmation?.IsIssuedByExternalAccountant == true),
                            IncomeDeduction = i.Employement?.WageDeduction is not null ? new cLA.LoanApplicationEmploymentIncomeDeduction
                            {
                                Execution = i.Employement.WageDeduction.DeductionDecision,
                                Installments = i.Employement.WageDeduction.DeductionPayments,
                                Other = i.Employement.WageDeduction.DeductionOther,
                            } : null,
                        };
                    };

                    cLA.LoanApplicationEntrepreneurIncome? MapEntrepreneurIncome(cHousehold.IncomeInList? iil)
                    {
                        if (iil == null)
                        {
                            return null;
                        }

                        var i = data.IncomesById[iil.IncomeId];

                        return new cLA.LoanApplicationEntrepreneurIncome
                        {
                            EntrepreneurIdentificationNumber = new List<string?> { i.Entrepreneur?.Cin, i.Entrepreneur?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),
                            Address = new cRS.AddressDetail
                            {
                                CountryId = i.Entrepreneur?.CountryOfResidenceId,
                            },
                            ProofTypeId = 2, // Default = 2 (DAP - Daňové přiznání), ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            AnnualIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                            },
                        };
                    };

                    cLA.LoanApplicationRentIncome? MapRentIncome(cHousehold.IncomeInList? iil)
                    {
                        if (iil == null)
                        {
                            return null;
                        }

                        var i = data.IncomesById[iil.IncomeId];

                        return new cLA.LoanApplicationRentIncome
                        {
                            // ProofTypeId            // Pro tento typ příjmu není v MVP relevantní, ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            MonthlyIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                            },
                        };
                    };

                    cLA.LoanApplicationOtherIncome MapOtherIncome(cHousehold.IncomeInList iil)
                    {
                        var i = data.IncomesById[iil.IncomeId];
                        var id = i.Other?.IncomeOtherTypeId;

                        return new cLA.LoanApplicationOtherIncome
                        {
                            IncomeOtherTypeId = id.HasValue ? id.Value : default(int),
                            // ProofTypeId            // Pro tento typ příjmu není v MVP relevantní, ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            MonthlyIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                            },
                        };
                    };

                    return new cLA.LoanApplicationIncome
                    {
                        IsIncomeConfirmed = cOnSA.LockedIncomeDateTime is not null,
                        LastConfirmedDate = cOnSA.LockedIncomeDateTime is not null ? (DateTime)cOnSA.LockedIncomeDateTime! : default(DateTime),
                        EmploymentIncomes = cOnSA.Incomes.Where(i => i.IncomeTypeId == 1).Select(i => MapEmploymentIncome(i)).ToList(),     // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 1
                        EntrepreneurIncome = MapEntrepreneurIncome(cOnSA.Incomes.FirstOrDefault(i => i.IncomeTypeId == 2)),                 // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 2
                        RentIncome = MapRentIncome(cOnSA.Incomes.FirstOrDefault(i => i.IncomeTypeId == 3)),                                 // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 3
                        OtherIncomes = cOnSA.Incomes.Where(i => i.IncomeTypeId == 4).Select(i => MapOtherIncome(i)).ToList(),               // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 4
                    };
                }


                var identityKb = cOnSA.CustomerIdentifiers.First(i => i.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb);
                var c = data.CustomersByIdentityCode[LoanApplicationDataService.IdentityToCode(identityKb)];

                var contactMobilePhone = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 13 && i.IsPrimary);
                var contactEmail = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 14 && i.IsPrimary);

                var addressPermanent = c.Addresses.FirstOrDefault(i => i.AddressTypeId == 1);  // Pouze trvalá adresa, WHERE podmínka:Customer.Addresses.AddressTypeId = PERMANENT
                var kbRelationshipCodeUpper = c.NaturalPerson?.KbRelationshipCode?.ToUpperInvariant();

                var degreeBeforeId = c.NaturalPerson?.DegreeBeforeId;
                var academicTitlePrefix = degreeBeforeId.HasValue ? (data.AcademicDegreesBeforeById.ContainsKey(degreeBeforeId.Value) ? data.AcademicDegreesBeforeById[degreeBeforeId.Value].Name : null) : null;

                var cGenderId = c.NaturalPerson?.GenderId;
                
                return new cLA.LoanApplicationCustomer
                {
                    InternalCustomerId = cOnSA.CustomerOnSAId,
                    PrimaryCustomerId = identityKb!.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    SpecialRelationsWithKB = cOnSA.CustomerAdditionalData?.HasRelationshipWithKB ?? false,
                    BirthNumber = c.NaturalPerson?.BirthNumber,
                    CustomerRoleId = cOnSA.CustomerRoleId,
                    Firstname = c.NaturalPerson?.FirstName,
                    Surname = c.NaturalPerson?.LastName,
                    BirthName = c.NaturalPerson?.BirthName,
                    BirthDate = c.NaturalPerson?.DateOfBirth,
                    BirthPlace = c.NaturalPerson?.PlaceOfBirth,
                    GenderId = cGenderId == (int)Genders.Unknown ? null : cGenderId,
                    MaritalStateId = c.NaturalPerson?.MaritalStatusStateId,
                    EducationLevelId = c.NaturalPerson?.EducationLevelId > 0 ? c.NaturalPerson?.EducationLevelId : null, // neposílat pokud 0
                    AcademicTitlePrefix = academicTitlePrefix,
                    MobilePhoneNumber = $"{contactMobilePhone?.Mobile?.PhoneIDC}{contactMobilePhone?.Mobile?.PhoneNumber}",
                    HasEmail = !String.IsNullOrEmpty(contactEmail?.Email?.EmailAddress),
                    IsPartner = isPartner,
                    Taxpayer = c.NaturalPerson?.TaxResidence?.ResidenceCountries?.Any(t => t.CountryId == 16) ?? false,
                    Address = (addressPermanent is null) ? null : MapAddress(addressPermanent),
                    IdentificationDocument = MapIdentificationDocument(c.IdentificationDocument),
                    Obligations = cOnSA.Obligations.Where(i => i.Creditor?.IsExternal == true).Select(i => MapObligation(i)).ToList(), //WHERE podmínka - pouze ty závazky, kde: Obligation.Creditor.IsExternal = true
                    Income = MapIncome(),
                };
            };

            var expenses = new cRS.V1.ExpensesSummary
            {
                Rent = h.Expenses.HousingExpenseAmount,
                Saving = h.Expenses.SavingExpenseAmount,
                Insurance = h.Expenses.InsuranceExpenseAmount,
                Other = h.Expenses.OtherExpenseAmount,
            };

            var propertySettlementId = h.Data?.PropertySettlementId;
            var childrenUpToTenYearsCount = h.Data?.ChildrenUpToTenYearsCount;
            var childrenOverTenYearsCount = h.Data?.ChildrenOverTenYearsCount;

            var householdCustomersOnSA = data.CustomersOnSa.Where(c => c.CustomerOnSAId == h.CustomerOnSAId1 || c.CustomerOnSAId == h.CustomerOnSAId2).ToArray();
            bool isPartner = householdCustomersOnSA.Length == 2 ? DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(householdCustomersOnSA[0].MaritalStatusId, householdCustomersOnSA[1].MaritalStatusId) : false;

            return new cLA.LoanApplicationHousehold
            {
                HouseholdId = h.HouseholdId,
                HouseholdTypeId = h.HouseholdTypeId,
                PropertySettlementId = propertySettlementId.HasValue ? propertySettlementId.Value : 0,
                ChildrenUpToTenYearsCount = childrenUpToTenYearsCount.HasValue ? childrenUpToTenYearsCount.Value : 0,
                ChildrenOverTenYearsCount = childrenOverTenYearsCount.HasValue ? childrenOverTenYearsCount.Value : 0,
                Expenses = expenses,
                Customers = householdCustomersOnSA
                    .Where(i => i.CustomerIdentifiers.Any(x => x.IdentityScheme == cCis.Identity.Types.IdentitySchemes.Kb))
                    .Select(i => MapCustomer(i, isPartner)).ToList(),
            };
        }

        cLA.LoanApplicationProduct MapProduct()
        {
            var MapLoanPurpose = (cOffer.LoanPurpose i) => new cLA.LoanApplicationProductPurpose
            {
                LoanPurposeId = i.LoanPurposeId,
                Amount = i.Sum,
            };

            var loanAmount = (decimal)data.Offer.SimulationResults.LoanAmount;
            var financialResourcesOwn = data.Offer.BasicParameters.FinancialResourcesOwn ?? 0;
            var financialResourcesOther = data.Offer.BasicParameters.FinancialResourcesOther ?? 0;
            var investmentAmount = (loanAmount + financialResourcesOwn + financialResourcesOther);  //Jako součet Offer.SimulationResults.LoanAmount +Offer.SimulationInputs.FinancialResourcesOwn + Offer.SimulationInputs.FinancialResourcesOther

            var productCollateral = new cLA.LoanApplicationProductCollateral
            {
                AppraisedValue = new cRS.AmountDetail
                {
                    Amount = data.Offer.SimulationInputs.CollateralAmount,
                }
            };

            return new cLA.LoanApplicationProduct
            {
                ProductTypeId = data.CaseData.Data.ProductTypeId,
                LoanKindId = data.Offer.SimulationInputs.LoanKindId,
                Ltv = data.Offer.SimulationResults.LoanToValue,
                LoanDuration = data.Offer.SimulationResults.LoanDuration,
                LoanPaymentAmount = data.Offer.SimulationResults.LoanPaymentAmount,
                FixedRatePeriod = data.Offer.SimulationInputs.FixedRatePeriod,
                LoanInterestRate = data.Offer.SimulationResults.LoanInterestRate,
                InstallmentCount = data.Offer.SimulationResults.AnnuityPaymentsCount,
                DrawingPeriodStart = data.Arrangement.Mortgage?.ExpectedDateOfDrawing,
                DrawingPeriodEnd = data.Offer.SimulationResults.DrawingDateTo,
                RepaymentPeriodStart = data.Offer.SimulationResults.AnnuityPaymentsDateFrom,
                RepaymentPeriodEnd = data.Offer.SimulationResults.LoanDueDate,
                HomeCurrencyIncome = data.Arrangement.Mortgage?.IncomeCurrencyCode,
                HomeCurrencyResidence = data.Arrangement.Mortgage?.ResidencyCurrencyCode,
                DeveloperId = data.Offer.SimulationInputs.Developer?.DeveloperId,
                DeveloperProjectId = data.Offer.SimulationInputs.Developer?.ProjectId,
                RequiredAmount = data.Offer.SimulationResults.LoanAmount,
                InvestmentAmount = investmentAmount,
                OwnResourcesAmount = financialResourcesOwn,
                ForeignResourcesAmount = financialResourcesOther,
                FinancingTypes = data.Arrangement.Mortgage?.LoanRealEstates?.Select(t => t.RealEstatePurchaseTypeId)?.ToList(),
                MarketingActions = data.Offer.AdditionalSimulationResults?.MarketingActions?.Where(i => i.MarketingActionId.HasValue && i.Applied == 1).Select(i => i.MarketingActionId!.Value).ToList(),
                Purposes = data.Offer.SimulationInputs.LoanPurposes?.Select(i => MapLoanPurpose(i)).ToList(),
                Collaterals = new List<cLA.LoanApplicationProductCollateral> { productCollateral },
            };
        }

        cRS.Identity? MapUserIdentity()
        {
            var identity = data.User.UserIdentifiers.FirstOrDefault();

            if (identity == null)
            {
                return null;
            }

            return new cRS.Identity
            {
                IdentityId = identity.Identity,
                IdentityScheme = identity.IdentityScheme.ToString(),
            };
        }

        var appendixCode = data.CaseData.Data.ProductTypeId == 20004 ? 25 : 0;      // IF Case.Data.ProductInstanceTypeId = 20004  => poslat "25" ELSE => poslat "0"

        return new cLA.LoanApplicationSaveRequest
        {
            SalesArrangementId = data.Arrangement.SalesArrangementId,
            AppendixCode = appendixCode,
            DistributionChannelId = data.Arrangement.ChannelId,
            LoanApplicationDataVersion = data.LoanApplicationDataVersion,
            Households = data.Households.Select(i => MapHousehold(i)).ToList(),
            Product = MapProduct(),
            UserIdentity = MapUserIdentity(),
        };
    }

    public static cRB.RiskBusinessCaseCreateAssessmentRequest ToRiskBusinessCaseCreateAssesmentRequest(this LoanApplicationData data)
    {
        // https://wiki.kb.cz/pages/viewpage.action?pageId=472504461

        return new cRB.RiskBusinessCaseCreateAssessmentRequest
        {
            SalesArrangementId = data.Arrangement.SalesArrangementId,
            RiskBusinessCaseId = data.Arrangement.RiskBusinessCaseId, // hodnota musí být v okamžiku volání známa, bez této hodnoty je request nevalidní

            // Timestamp, který jsme si uložili pro danou verzi žádosti (dat žádosti), kterou jsme předali v RIP(v2) - POST LoanApplication a tímto danou verzi požadujeme vyhodnotit:
            LoanApplicationDataVersion = data.LoanApplicationDataVersion,

            // Konstanta SC:
            AssessmentMode = cRB.RiskBusinessCaseAssessmentModes.SC,

            // Pokud uložený atribut Offer.SimulationInputs.IsEmployeeBonusRequested = true, pak poslat kód "EMP". Jinak posílat kód "STD".:
            GrantingProcedureCode = data.Offer.SimulationInputs.IsEmployeeBonusRequested == true ? cRB.RiskBusinessCaseGrantingProcedureCodes.EMP : cRB.RiskBusinessCaseGrantingProcedureCodes.STD,
        };
    }

}
