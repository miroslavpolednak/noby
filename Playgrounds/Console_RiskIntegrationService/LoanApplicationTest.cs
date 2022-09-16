using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace Console_RiskIntegrationService;

internal static class LoanApplicationTest
{
    public static LoanApplicationSaveRequest _test1 = new LoanApplicationSaveRequest
    {
        SalesArrangementId = 1,
        LoanApplicationDataVersion = "00003",
        Product = new LoanApplicationProduct
        {
            ProductTypeId = 20001,
            LoanKindId = 20001,
            Ltv = 90,
            RequiredAmount = 1000000,
            LoanDuration = 240,
            LoanPaymentAmount = 5000,
            InstallmentPeriod = "M",
            DrawingPeriodStart = new DateTime(2022, 10, 1),
            RepaymentPeriodStart = new DateTime(2022, 11, 5),
            Collaterals = new List<LoanApplicationProductCollateral>
            {
                new LoanApplicationProductCollateral
                {
                    AppraisedValue = new DomainServices.RiskIntegrationService.Contracts.Shared.AmountDetail
                    {
                        Amount = 10000
                    },
                    CollateralType = 1,
                    Id = "1"
                }
            }
        },
        Households = new List<LoanApplicationHousehold>
        {
            new LoanApplicationHousehold
            {
                HouseholdId = 1,
                Customers = new List<LoanApplicationCustomer>
                {
                    new LoanApplicationCustomer
                    {
                        InternalCustomerId = 144625405,
                        PrimaryCustomerId = "12",
                        CustomerRoleId = 1,
                        Surname = "test",
                        MaritalStateId = 1,
                        BirthDate = new DateTime(1980, 1, 3),
                        Taxpayer = true,
                        Income = new LoanApplicationIncome
                        {
                            EntrepreneurIncome = new LoanApplicationEntrepreneurIncome
                            {   
                                AnnualIncomeAmount = new DomainServices.RiskIntegrationService.Contracts.Shared.AmountDetail
                                {
                                    Amount = 100000
                                },
                                JobTypeId = 1,
                                ProofTypeId = 1,
                                EstablishedOn = new DateTime(2000, 1, 1),
                                EntrepreneurIdentificationNumber = "111111"
                            }
                        }
                    }
                }
            }
        }
    };
}
