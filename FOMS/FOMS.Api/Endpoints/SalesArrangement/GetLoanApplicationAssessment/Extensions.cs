using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
 
    public static GetLoanApplicationAssessmentResponse ToApiResponse(this DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse response)
    {
        return new GetLoanApplicationAssessmentResponse
        {
            /*Application = new()
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
                LTFV = response.CollateralRiskCharacteristics?.Ltfv,
                LTV = response.CollateralRiskCharacteristics?.Ltv
            },
            Households = response?.HouseholdsDetails?.Select(h => new Dto.Household
            {
                HouseholdId = h.HouseholdId,
                MonthlyIncome = h.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
                MonthlyCostsWithoutInstallments = h.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
                MonthlyInstallmentsInMPSS = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
                MonthlyInstallmentsInOFI = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
                MonthlyInstallmentsInCBCB = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
                CIR = h.Detail?.Limit?.Cir,
                DTI = h.Detail?.Limit?.Dti,
                DSTI = h.Detail?.Limit?.Dsti
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
                Weight = r.Weight
            }).ToList()*/
        };
    }

    public static cLA.LoanApplicationSaveRequest ToLoanApplicationSaveRequest(this LoanApplicationData data)
    {
        // https://wiki.kb.cz/display/HT/RIP%28v2%29+-+POST+LoanApplication

        cLA.LoanApplicationHousehold MapHousehold(cArrangement.Household h)
        {
            cLA.LoanApplicationCustomer MapCustomer(cArrangement.CustomerOnSA cOnSA)
            {
                var obligationTypeLoanPrincipalIds = new List<int> { 1, 2, 5 }; // nemělo by se brát dle pole Code v číselníku ??

                var MapAddress = (cCis.GrpcAddress a) => new cRS.AddressDetail
                {
                    Street = a.Street,
                    BuildingIdentificationNumber = a.BuildingIdentificationNumber,
                    LandRegistryNumber = a.LandRegistryNumber,
                    EvidenceNumber = a.EvidenceNumber,
                    City = a.City,
                    CountryId = a.CountryId,
                    Postcode = a.Postcode,
                };

                var MapIdentificationDocument = (cCustomer.IdentificationDocument id) => new cRS.V1.IdentificationDocumentDetail
                {
                   DocumentNumber = id.Number,
                   IdentificationDocumentTypeId = id.IdentificationDocumentTypeId,
                   IssuedOn = id.IssuedOn,
                   ValidTo = id.ValidTo,
                };

                var MapObligation = (cArrangement.Obligation o) => new cLA.LoanApplicationObligation
                {
                    ObligationTypeId = o.ObligationId,
                    Amount = obligationTypeLoanPrincipalIds.Contains(o.ObligationTypeId!.Value) ? o.LoanPrincipalAmount : o.CreditCardLimit, // Pro Obligation.ObligationTypeId s hodnotami "1", "2" a "5" poslat hodnotu z: Obligation.LoanPrincipalAmount; Pro Obligation.ObligationTypeId s hodnotami "3" a "4" poslat hodnotu z: Obligation.CreditCardLimit;
                    AmountConsolidated = obligationTypeLoanPrincipalIds.Contains(o.ObligationTypeId!.Value) ? o.Correction.LoanPrincipalAmountCorrection : o.Correction.CreditCardLimitCorrection,  // // Pro Obligation.ObligationTypeId s hodnotami "1", "2" a "5" poslat hodnotu z: Obligation.Correction.LoanPrincipalAmountCorrection; Pro Obligation.ObligationTypeId s hodnotami "3" a "4" poslat hodnotu z: Obligation.Correction.CreditCardLimitCorrection;
                    Installment = o.InstallmentAmount,
                    InstallmentConsolidated = o.Correction.InstallmentAmountCorrection,
                };

                cLA.LoanApplicationIncome MapIncome() {

                    cLA.LoanApplicationEmploymentIncome MapEmploymentIncome(IncomeInList iil) {

                        var i = data.IncomesById[iil.IncomeId];

                        return new cLA.LoanApplicationEmploymentIncome
                        {
                            EmployerIdentificationNumber = new List<string> { i.Employement?.Employer?.Cin, i.Employement?.Employer?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),
                            //WorkSectorId
                            EmployerName = i.Employement?.Employer?.Name,
                            //ClassficationOfEconomicActivityId
                            //JobTypeId
                            Address = new cRS.AddressDetail {
                                //Postcode = i.Employement?.Employer?.Postcode,
                                //City = i.Employement?.Employer?.City,
                                CountryId = i.Employement?.Employer?.CountryId,
                                //Street = i.Employement?.Employer?.Street,
                                //BuildingIdentificationNumber = i.Employement?.Employer?.BuildingIdentificationNumber,
                                //LandRegistryNumber = i.Employement?.Employer?.LandRegistryNumber,
                            },

                            JobDescription = i.Employement?.Job?.JobDescription,

                            //PhoneNumber = i.Employement?.Employer?.PhoneNumber,
                            MonthlyIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                                // CurrencyCode = iil.CurrencyCode,         // Do doby než bude služba na přepočet cizích měn na CZK nemá smysl posílat - RIP při neposlání doplňuje default CZK
                            },
                            // BankAccount = new cRS.BankAccountDetail { },
                            // IsDomicile = i.IsDomicile,
                            ProofTypeId = 1,                                  // Default = 1 (PVP - Potvrzení o příjmu), ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            IncomeForeignTypeId = i.Employement?.ForeignIncomeTypeId,
                            GrossAnnualIncome = i.Employement?.Job?.GrossAnnualIncome,
                            ConfirmationPerson = i.Employement?.IncomeConfirmation?.ConfirmationPerson,
                            ConfirmationContactPhone = i.Employement?.IncomeConfirmation?.ConfirmationContact,
                            ConfirmationDate = i.Employement?.IncomeConfirmation?.ConfirmationDate,
                            JobTrialPeriod = (i.Employement?.Job?.JobTrialPeriod == true),
                            NoticePeriod = (i.Employement?.Job?.JobNoticePeriod == true),
                            EmploymentTypeId = i.Employement?.Job?.EmploymentTypeId,
                            FirstWorkContractSince = i.Employement?.Job?.FirstWorkContractSince,
                            CurrentWorkContractSince = i.Employement?.Job?.CurrentWorkContractSince,
                            CurrentWorkContractTo = i.Employement?.Job?.CurrentWorkContractTo,
                            ConfirmationByCompany = (i.Employement?.IncomeConfirmation?.ConfirmationByCompany == true), // IsIssuedByExternalAccountant ... nemá být ConfirmationByCompany ???
                            IncomeDeduction = i.Employement?.WageDeduction is not null ? new cLA.LoanApplicationEmploymentIncomeDeduction
                            {
                                Execution = i.Employement.WageDeduction.DeductionDecision,
                                Installments = i.Employement.WageDeduction.DeductionPayments,
                                Other = i.Employement.WageDeduction.DeductionOther,
                            } : null,
                        };
                    };

                    cLA.LoanApplicationEntrepreneurIncome? MapEntrepreneurIncome(IncomeInList? iil) {

                        if (iil == null)
                        {
                            return null;
                        }

                        var i = data.IncomesById[iil.IncomeId];
                        
                        return new cLA.LoanApplicationEntrepreneurIncome {
                            EntrepreneurIdentificationNumber = new List<string> { i.Employement?.Employer?.Cin, i.Employement?.Employer?.BirthNumber }.FirstOrDefault(i => !String.IsNullOrEmpty(i)),
                            Address = new cRS.AddressDetail
                            {
                                //Postcode = i.Employement?.Employer?.Postcode,
                                //City = i.Employement?.Employer?.City,
                                //CountryId = i.Employement?.Employer?.CountryOfResidenceId == true,                    //??? ---
                                //Street = i.Employement?.Employer?.Street,
                                //BuildingIdentificationNumber = i.Employement?.Employer?.BuildingIdentificationNumber,
                                //LandRegistryNumber = i.Employement?.Employer?.LandRegistryNumber,
                            },
                            ProofTypeId = 2,            // Default = 2 (DAP - Daňové přiznání), ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            AnnualIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                                //CurrencyCode
                            },
                        };
                    };

                    cLA.LoanApplicationRentIncome? MapRentIncome(IncomeInList? iil)
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
                                //CurrencyCode
                            },
                        };
                    };

                    cLA.LoanApplicationOtherIncome MapOtherIncome(IncomeInList iil)
                    {
                        var i = data.IncomesById[iil.IncomeId];

                        return new cLA.LoanApplicationOtherIncome
                        {
                            //IncomeOtherTypeId = i.IncomeTypeId,                     // IncomeOtherTypeId ... nemá být IncomeTypeId?
                            // ProofTypeId            // Pro tento typ příjmu není v MVP relevantní, ProofType(CIS_TYP_POTVRDENIE_PRIJMU)
                            MonthlyIncomeAmount = new cRS.AmountDetail
                            {
                                Amount = iil.Sum is not null ? (decimal)iil.Sum! : default(decimal),
                                //CurrencyCode
                            },
                        };
                    };

                    // TODO:
                    return new cLA.LoanApplicationIncome
                    {
                        IsIncomeConfirmed = cOnSA.LockedIncomeDateTime is not null,
                        LastConfirmedDate = cOnSA.LockedIncomeDateTime is not null ? (DateTime)cOnSA.LockedIncomeDateTime! : default(DateTime),
                        EmploymentIncomes = cOnSA.Incomes.Where(i => i.IncomeTypeId == 1).Select(i => MapEmploymentIncome(i)).ToList(),     // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 1 //TODO find by codebook MainIncomeType !!! 
                        EntrepreneurIncome = MapEntrepreneurIncome(cOnSA.Incomes.FirstOrDefault(i => i.IncomeTypeId == 2)),                 // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 2 //TODO find by codebook MainIncomeType !!! 
                        RentIncome = MapRentIncome(cOnSA.Incomes.FirstOrDefault(i => i.IncomeTypeId == 3)),                                 // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 3 //TODO find by codebook MainIncomeType !!! 
                        OtherIncomes = cOnSA.Incomes.Where(i => i.IncomeTypeId == 4).Select(i => MapOtherIncome(i)).ToList(),               // Pro zdroj_prijmu_hlavni = ze zaměstnání; WHERE podmínka: Income.IncomeTypeId = 4 //TODO find by codebook MainIncomeType !!! 
                    };
                }

                // do JSON věty jdou pouze Customers s Kb identitou // ???
                var identityKb = cOnSA.CustomerIdentifiers.Single(i => i.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb);
                //var identityMp = cOnSA.CustomerIdentifiers.Single(i => i.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp); // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!  ???
                var c = data.CustomersByIdentityCode[LoanApplicationDataService.IdentityToCode(identityKb)];

                var contactMobilePhone = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 1 && i.IsPrimary);   //TODO find by codebook ???
                var contactEmail = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 5 && i.IsPrimary);   //TODO find by codebook ???

                var addressPermanent = c.Addresses.FirstOrDefault(i => i.AddressTypeId == 1 );  // Pouze trvalá adresa, WHERE podmínka:Customer.Addresses.AddressTypeId = PERMANENT  //TODO find by codebook !!! 

                return new cLA.LoanApplicationCustomer
                {
                    InternalCustomerId = cOnSA.CustomerOnSAId,
                    PrimaryCustomerId = identityKb!.IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    // customer !!!
                    //IsGroupEmployee = c.NaturalPerson?.KbRelationshipCode //TRUE IF: Customer.NaturalPerson.KbRelationshipCode = A nebo Customer.NaturalPerson.KbRelationshipCode = E, V CM customerKbRelationship.code = "A" nebo "E" ??? KbRelationshipCode na žádné entitě neevidujeme
                    //SpecialRelationsWithKB = // TRUE IF: Customer.NaturalPerson.KbRelationshipCode = R nebo Customer.NaturalPerson.KbRelationshipCode = D, V CM customerKbRelationship.code = "R" nebo "D" ???
                    BirthNumber = c.NaturalPerson?.BirthNumber,
                    CustomerRoleId = cOnSA.CustomerRoleId,
                    Firstname = c.NaturalPerson?.FirstName,
                    Surname = c.NaturalPerson?.LastName,
                    BirthName = c.NaturalPerson?.BirthName,
                    BirthDate = c.NaturalPerson?.DateOfBirth,
                    BirthPlace = c.NaturalPerson?.PlaceOfBirth,
                    GenderId = c.NaturalPerson?.GenderId,
                    MaritalStateId = c.NaturalPerson?.MaritalStatusStateId,
                    EducationLevelId = c.NaturalPerson?.EducationLevelId,
                    // AcademicTitlePrefix = c.NaturalPerson?.DegreeBeforeId,   // ??? type
                    MobilePhoneNumber = contactMobilePhone?.Value,
                    HasEmail = !String.IsNullOrEmpty(contactEmail?.Value),
                    IsPartner = (h.Data?.AreCustomersPartners == true),
                    //Taxpayer = //c.NaturalPerson.TaxResidencyCountryId      //Customer.NaturalPerson.TaxResidencyCountryId = "CZ", V CM taxResidence.countryCode = "CZ"  //??? TaxResidencyCountryId na žádné entitě nemáme
                    Address = (addressPermanent is null) ? null : MapAddress(addressPermanent),
                    IdentificationDocument = MapIdentificationDocument(c.IdentificationDocument), // WHERE podmínka: Měli bychom pracovat pouze s jedním dokladem = načítáme z CM pouze hlavní doklad klienta - objekt primaryIdentificationDocument ???
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

            return new cLA.LoanApplicationHousehold
            {
                HouseholdId = h.HouseholdTypeId,        // nemá být HouseholdId ???
                HouseholdTypeId = h.HouseholdTypeId,
                PropertySettlementId = h.Data.PropertySettlementId.HasValue ? h.Data.PropertySettlementId.Value : 0,
                ChildrenUpToTenYearsCount = h.Data.ChildrenUpToTenYearsCount,
                ChildrenOverTenYearsCount = h.Data.ChildrenOverTenYearsCount,
                Expenses = expenses,
                Customers = data.CustomersOnSa.Select(i=>MapCustomer(i)).ToList(),
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
                // CollateralType   // NOBY nebude plnit - na straně RIP dojde k doplnění hodnoty "nemovitost"
                AppraisedValue = new cRS.AmountDetail
                {
                    Amount = data.Offer.SimulationInputs.CollateralAmount,
                    // CurrencyCode //???
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
                // RepaymentScheduleTypeId  // Pro všechny produkty, které aktuálně NOBY bude posílat v MVP je relevantní pouze hodnota anuitní splácení (pro C4M hodnota "A").
                // InstallmentPeriod        // Bude doplněn default na straně RIP, pokud na vstupu nic nepřijde
                InstallmentCount = data.Offer.SimulationResults.AnnuityPaymentsCount,
                DrawingPeriodStart = data.Arrangement.Mortgage.ExpectedDateOfDrawing,
                DrawingPeriodEnd = data.Offer.SimulationResults.DrawingDateTo,
                RepaymentPeriodStart = data.Offer.SimulationResults.AnnuityPaymentsDateFrom,
                RepaymentPeriodEnd = data.Offer.SimulationResults.LoanDueDate,
                HomeCurrencyIncome = data.Arrangement.Mortgage.IncomeCurrencyCode,
                HomeCurrencyResidence = data.Arrangement.Mortgage.ResidencyCurrencyCode,
                DeveloperId = data.Offer.SimulationInputs.Developer.DeveloperId,
                DeveloperProjectId = data.Offer.SimulationInputs.Developer.ProjectId,
                RequiredAmount = data.Offer.SimulationResults.LoanAmount,
                InvestmentAmount = investmentAmount,
                OwnResourcesAmount = financialResourcesOwn,
                ForeignResourcesAmount = financialResourcesOther,
                MarketingActions = data.Offer.AdditionalSimulationResults.MarketingActions?.Where(i => i.MarketingActionId.HasValue).Select(i => i.MarketingActionId!.Value).ToList(),
                Purposes = data.Offer.SimulationInputs.LoanPurposes.Select(i=> MapLoanPurpose(i)).ToList(),
                Collaterals = new List<cLA.LoanApplicationProductCollateral> { productCollateral },
            };
        }

        cRS.Identity? MapUserIdentity()
        {
            // pokud více identit, tak identita se řídí dle produktu (nelze mít dvě MP nebo KB identity) ???
            //var identity = data.User.UserIdentifiers.FirstOrDefault(i => i.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.UserIdentity.Types.UserIdentitySchemes. );
            var identity = data.User.UserIdentifiers.FirstOrDefault();

            if (identity == null)
            {
                return null;
            }

            return new cRS.Identity
            {
                IdentityId = identity.Identity,
                IdentityScheme = identity.IdentityScheme.ToString(),
                //Identity = "10009819",
                //IdentityScheme = "DMID"
            };
        }

        var appendixCode = data.CaseData.Data.ProductTypeId == 20004 ? 25 : 0;      // IF Case.Data.ProductInstanceTypeId = 20004  => poslat "25" ELSE => poslat "0"

        return new cLA.LoanApplicationSaveRequest
        {
            SalesArrangementId = data.Arrangement.SalesArrangementId,
            AppendixCode = appendixCode,
            DistributionChannelId = data.Arrangement.ChannelId,                     // DistributionChannelCode???
            //SignatureType                                                         // pro NOBY nerelevantní
            LoanApplicationDataVersion = data.LoanApplicationDataVersion,
            Households = data.Households.Select(i => MapHousehold(i)).ToList(),
            Product = MapProduct(),
            //ProductRelations                                                      // V MVP neplníme; Objekt plníme pouze pro produkt doprodej neúčelové části ???
            UserIdentity = MapUserIdentity(),
        };
    }

    public static cRB.RiskBusinessCaseCreateAssesmentRequest ToRiskBusinessCaseCreateAssesmentRequest(this LoanApplicationData data)
    {
        // https://wiki.kb.cz/pages/viewpage.action?pageId=472504461

        return new cRB.RiskBusinessCaseCreateAssesmentRequest
        {
            SalesArrangementId = data.Arrangement.SalesArrangementId,
            RiskBusinessCaseId = data.Arrangement.RiskBusinessCaseId,

            // Timestamp, který jsme si uložili pro danou verzi žádosti (dat žádosti), kterou jsme předali v RIP(v2) - POST LoanApplication a tímto danou verzi požadujeme vyhodnotit:
            LoanApplicationDataVersion = data.LoanApplicationDataVersion,

            // Konstanta SC:
            AssessmentMode = cRB.RiskBusinessCaseAssessmentModes.SC,

            // Pokud uložený atribut Offer.SimulationInputs.IsEmployeeBonusRequested = true, pak poslat kód "EMP". Jinak posílat kód "STD".:
            GrantingProcedureCode = data.Offer.SimulationInputs.IsEmployeeBonusRequested == true ? cRB.RiskBusinessCaseGrantingProcedureCodes.EMP : cRB.RiskBusinessCaseGrantingProcedureCodes.STD,
        };
    }

}
