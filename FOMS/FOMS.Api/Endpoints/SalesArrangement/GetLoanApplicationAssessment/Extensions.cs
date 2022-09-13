using cLA = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using cRB = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using cRS = DomainServices.RiskIntegrationService.Contracts.Shared;

using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCis = CIS.Infrastructure.gRPC.CisTypes;
using cCustomer = DomainServices.CustomerService.Contracts;

using DomainServices.CaseService.Contracts;
using System;
using DomainServices.CodebookService.Contracts.Endpoints.CollateralTypes;
using Google.Protobuf.WellKnownTypes;
using DomainServices.ProductService.Contracts;
using Microsoft.AspNetCore.Routing;
using StackExchange.Redis;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.JSInterop.Implementation;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
    //https://wiki.kb.cz/display/HT/RIP%28v2%29+-+POST+LoanApplication

    /*
    public static GetLoanApplicationAssessmentResponse ToApiResponse(this LoanApplicationAssessmentResponse response)
    {
        return new GetLoanApplicationAssessmentResponse
        {
            Application = new()
            {
                LoanApplicationLimit = response.AssessmentDetail?.LoanApplicationLimit?._LoanApplicationLimit.Value,
                LoanApplicationInstallmentLimit = response.AssessmentDetail?.LoanApplicationLimit?.LoanApplicationInstallmentLimit?.Value,
                RemainingAnnuityLivingAmount = response.AssessmentDetail?.LoanApplicationLimit?.RemainingAnnuityLivingAmount?.Value,
                MonthlyIncomeAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInKBAmount.Value,
                MonthlyEntrepreneurInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKBAmount.Value,
                MonthlyInstallmentsInMPSSAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInCBCBAmount.Value,
                CIR = response.AssessmentDetail?.LoanApplicationLimit?.Cir,
                DTI = response.AssessmentDetail?.LoanApplicationLimit?.Dti,
                DSTI = response.AssessmentDetail?.LoanApplicationLimit?.Dsti,
                LTC = response.CollateralRiskCharacteristics?.Ltp,
                LFTV = response.CollateralRiskCharacteristics?.Ltfv,
                LTV = response.CollateralRiskCharacteristics?.Ltv
            },
            Households = response.HouseholdAssessmentDetail?.Select(h => new Dto.Household
            {
                HouseholdId = h.HouseholdId,
                MonthlyIncomeAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInMPSSAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInCBCBAmount.Value,
                CIR = h.AssessmentDetail?.LoanApplicationLimit?.Cir,
                DTI = h.AssessmentDetail?.LoanApplicationLimit?.Dti,
                DSTI = h.AssessmentDetail?.LoanApplicationLimit?.Dsti
            }).ToList(),
            RiskBusinesscaseExpirationDate = response.RiskBusinesscaseExpirationDate,
            AssessmentResult = response.AssessmentResult,
            Reasons = response.LoanApplicationAssessmentReason?.Select(r => new Dto.AssessmentReason
            {
                Code = r.Code,
                Desc = r.Detail?.Desc,
                Level = r.Level,
                Result = r.Detail?.Result,
                Target = r.Detail?.Target,
                Weight = r.Weight
            }).ToList()
        };
    }
    */

    public static GetLoanApplicationAssessmentResponse ToApiResponse(this DomainServices.RiskIntegrationService.Contracts.Shared.V1.LoanApplicationAssessmentResponse response)
    {
        //TODO:
        return new GetLoanApplicationAssessmentResponse
        {

        };
    }

    public static cLA.LoanApplicationSaveRequest ToLoanApplicationSaveRequest(this LoanApplicationData data)
    {
        //TODO:
        /*
         "'Sales Arrangement Id' must be greater than '0'.",
            "'Loan Application Data Version' must not be empty.",
            "'Households' must not be empty.",
            "'Product' must not be empty.",
            "'Households' must not be empty." 
         */

        cLA.LoanApplicationHousehold MapHousehold(cArrangement.Household h)
        {
            cLA.LoanApplicationCustomer MapCustomer(cArrangement.CustomerOnSA cOnSA)
            {
                var obligationTypeLoanPrincipalIds = new List<int> { 1, 2, 5 };

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

                            //JobTitle =                // nemá to být JobDescription ???
                            //JobDescription = i.Employement?.Job?.JobDescription,

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
                            IncomeDeduction = new cLA.LoanApplicationEmploymentIncomeDeduction
                            {
                                Execution = i.Employement?.WageDeduction?.DeductionDecision,
                                Installments = i.Employement?.WageDeduction?.DeductionPayments,
                                Other = i.Employement?.WageDeduction?.DeductionOther,
                            },
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
                                //CountryId = i.Employement?.Employer?.CountryOfResidenceId == true,                    //???
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
                var identityMp = cOnSA.CustomerIdentifiers.Single(i => i.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp); // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!  ???
                var c = data.CustomersByIdentityCode[LoanApplicationDataService.IdentityToCode(identityKb)];

                var contactMobilePhone = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 1 && i.IsPrimary);   //TODO find by codebook ???
                var contactEmail = c.Contacts.FirstOrDefault(i => i.ContactTypeId == 5 && i.IsPrimary);   //TODO find by codebook ???

                var addressPermanent = c.Addresses.FirstOrDefault(i => i.AddressTypeId == 1 );  // Pouze trvalá adresa, WHERE podmínka:Customer.Addresses.AddressTypeId = PERMANENT  //TODO find by codebook !!! 

                return new cLA.LoanApplicationCustomer
                {
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
                    // Taxpayer =       // Customer.NaturalPerson.TaxResidencyCountryId = "CZ", V CM taxResidence.countryCode = "CZ"
                    Address = (addressPermanent is null) ? null : MapAddress(addressPermanent),
                    IdentificationDocument = MapIdentificationDocument(c.IdentificationDocument),
                    Obligations = cOnSA.Obligations.Where(i => i.Creditor?.IsExternal == true).Select(i => MapObligation(i)).ToList(),
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
                PropertySettlementId = h.Data.PropertySettlementId,
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
            LoanApplicationDataVersion = Guid.NewGuid().ToString(),
            Households = data.Households.Select(i => MapHousehold(i)).ToList(),
            Product = MapProduct(),
            //ProductRelations                                                      // V MVP neplníme; Objekt plníme pouze pro produkt doprodej neúčelové části
            UserIdentity = MapUserIdentity(),
        };
    }

    public static cRB.RiskBusinessCaseCreateAssesmentRequest ToRiskBusinessCaseCreateAssesmentRequest(this LoanApplicationData data)
    {
        //TODO:
        return new cRB.RiskBusinessCaseCreateAssesmentRequest
        {
            SalesArrangementId = data.Arrangement.SalesArrangementId,
        };
    }

    /*
    public static LoanApplicationRequest ToLoanApplicationSampleRequest(this LoanApplicationData data)
    {
        return new LoanApplicationRequest
        {
            Id = new SystemId
            {
                Id = "38000119",
                System = "NOBY"
            },
            LoanApplicationDataVersion = "2022-05-31T12:16:00.7777", //  Guid.NewGuid().ToString(),
            LoanApplicationHousehold = new List<LoanApplicationHousehold2> {
                new LoanApplicationHousehold2 {
                     Id = 1,
                     CounterParty = new List<LoanApplicationCounterParty2>{
                         new LoanApplicationCounterParty2
                         {
                             Id = 1,
                             CustomerId = "970182896",
                             RoleCodeMp = 1
                         }
                     },
            }
            },
            LoanApplicationProduct = new LoanApplicationProduct2
            {
                Product = 20001,
                LoanType = 2000
            },
            HumanUser = new HumanUser
            {
                Identity = "10009819",
                IdentityScheme = "DMID"
            }
        };
    }
    */
}


/* MIN REQ
{
    "id": {
        "id": "38000119",
        "system": "NOBY"
    },
    "loanApplicationDataVersion": "2022-05-31T12:16:00.626Z",
    "loanApplicationHousehold": [
        {
            "id": 1,
            "counterParty": [
                {
                    "id": 1,
                    "customerId": "970182896",
                    "roleCodeMp": 1
                }
            ]
        }
    ],
    "loanApplicationProduct": {
        "product": 20001,
        "loanType": 2000
    },
    "humanUser": {
        "identity": "10009819",
        "identityScheme": "DMID"
    }
}
 * */