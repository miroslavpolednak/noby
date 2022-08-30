using cArrangement = DomainServices.SalesArrangementService.Contracts;

using ExternalServices.Rip.V1.RipWrapper;
using System.Globalization;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
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

    public static LoanApplicationRequest ToLoanApplicationRequest(this LoanApplicationData data)
    {

        return data.ToLoanApplicationSampleRequest();

        LoanApplicationHousehold2? MapHousehold(cArrangement.Household i)
        {
            if (i == null)
            {
                return null;
            }

            LoanApplicationCounterParty2? MapCustomer(int? customerOnSaId)
            {

                if (!customerOnSaId.HasValue)
                {
                    return null;
                }

                var customer = data.CustomersOnSa.First(i => i.CustomerOnSAId == customerOnSaId);

                return new LoanApplicationCounterParty2
                {
                    Id = customerOnSaId.Value,
                    //CustomerId = customer.CustomerIdentifiers  data.CustomersByIdentityCode[LoanApplicationDataService.IdentityToCode() ]
                    RoleCodeMp = customer.CustomerRoleId,

                    CustomerId = "970182896",
                    // "id": 1,
                    //"customerId": "970182896",
                    //"roleCodeMp": 1


                };

            };

            return new LoanApplicationHousehold2
            {
                Id = i.HouseholdTypeId,
                RoleCodeMp = i.HouseholdTypeId,
                SettlementTypeCodeMp = i.Data?.PropertySettlementId,
                CounterParty = (new List<LoanApplicationCounterParty2?> { MapCustomer(i.CustomerOnSAId1), MapCustomer(i.CustomerOnSAId2) }).Where(i => i is not null).ToList(),
            };

        }


        var id = new SystemId
        {
            Id = data.Arrangement.SalesArrangementId.ToString(CultureInfo.InvariantCulture),
            System = "NOBY",
        };

        var loanApplicationProduct = new LoanApplicationProduct2
        {
            // DrawingPeriodStart = SalesArrangement.Parameters.ExpectedDateOfDrawing
            // HomeCurrencyIncome = SalesArrangement.Parameters.IncomeCurrencyCode
            // HomeCurrencyResidence = SalesArrangement.Parameters.ResidencyCurrencyCode



            //Product = data.Arrangement.SalesArrangementTypeId,      // Case.Data.ProductInstanceTypeId . . . do DV se bere z SA !?
            Product = data.CaseData.Data.ProductTypeId,
            LoanType = data.Offer.SimulationInputs.LoanKindId
        };

        var userIdentity = data.User.UserIdentifiers.First();       // jak vybrat konkrétní identitu?
        var humanUser = new HumanUser
        {
            Identity = userIdentity.Identity,                           //  "10009819",      // ??? User.UserIdentifiers.Identity        (přihlášeného uživatele)
            IdentityScheme = userIdentity.IdentityScheme.ToString()    // "DMID",    // ??? User.UserIdentifiers.IdentityScheme  (přihlášeného uživatele)
        };

        var loanApplicationRequest = new LoanApplicationRequest
        {
            Id = id,
            LoanApplicationDataVersion = DateTime.Now.ToLongTimeString(), // POZOR - pro každé volání nutné upravit (navýšit timestamp) loanApplicationDataVersion
            DistributionChannelCodeMp = data.Arrangement.ChannelId,
            LoanApplicationProduct = loanApplicationProduct,
            LoanApplicationHousehold = data.Households.Select(i => MapHousehold(i)).ToList(),
            HumanUser = humanUser,
        };

        return loanApplicationRequest;
    }

    public static LoanApplicationRequest ToLoanApplicationSampleRequest(this LoanApplicationData data)
    {
        return new LoanApplicationRequest
        {
            Id = new SystemId
            {
                Id = "38000119",
                System = "NOBY"
            },
            LoanApplicationDataVersion = DateTime.Now.ToLongTimeString(),
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