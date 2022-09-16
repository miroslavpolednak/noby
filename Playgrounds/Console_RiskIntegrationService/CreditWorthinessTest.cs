using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace Console_RiskIntegrationService;

internal static class CreditWorthinessTest
{
    public static CreditWorthinessCalculateRequest _test1 = new CreditWorthinessCalculateRequest
    {
        ResourceProcessId = "E233CFA8-EA4D-4E34-B8E2-6350B7E69EBC",
        Product = new CreditWorthinessProduct
        {
            ProductTypeId = 20001,
            FixedRatePeriod = 5,
            LoanDuration = 180,
            LoanAmount = 1000000,
            LoanPaymentAmount = 8000,
            LoanInterestRate = 3.5M
        },
        Households = new List<CreditWorthinessHousehold>
        {
            new CreditWorthinessHousehold
            {
                ExpensesSummary = new DomainServices.RiskIntegrationService.Contracts.Shared.V1.ExpensesSummary
                {
                    Saving = 1000,
                    Insurance = 1000
                },
                ChildrenOverTenYearsCount = 1,
                Customers = new List<CreditWorthinessCustomer>
                {
                    new CreditWorthinessCustomer
                    {
                        InternalCustomerId = "1111",
                        MaritalStateId = 1,
                        Incomes = new List<CreditWorthinessIncome>
                        {
                            new CreditWorthinessIncome
                            {
                                Amount = 10000M,
                                IncomeTypeId = 1
                            }
                        }
                    }
                }
            }
        }
    };
}
