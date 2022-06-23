using Mpss.Rip.Infrastructure.Services.PersonDealer;
using C4M = Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RipService.CreditWorthiness;

/// <summary>
/// Request transformation (RIP->C4M)
/// </summary>
[CIS.Infrastructure.Attributes.ScopedService]
public class CreditWorthinessComputeRequestTransformation 
    : ICreditWorthinessComputeRequestTransformation
{
    private struct Constants
    {
        public const string ResourceProcessIdDomain = "OM";
        public const string ResourceProcessIdInstance = "MPSS";
        public const string ResourceProcessIdResource = "OfferInstance";
        public const string ResourceProcessIdVariant = "NOBY";

        public const string PersonKbResourceIdentifierDomain = "PM";
        public const string PersonKbResourceIdentifierMpssInstance = "MPSS";
        public const string PersonKbResourceIdentifierKbInstance = "KBCZ";
        public const string PersonKbResourceIdentifierResource = "KBGroupPerson";

        public const string DealerResourceIdentifierDomain = "BM";
        public const string DealerResourceIdentifierMpssInstance = "MPSS";
        public const string DealerResourceIdentifierKbInstance = "KBCZ";
        public const string DealerResourceIdentifierResource = "Broker";

        public const string CompanyResourceIdentifierDomain = "BM";
        public const string CompanyResourceIdentifierMpssInstance = "MPSS";
        public const string CompanyResourceIdentifierKbInstance = "KBCZ";
        public const string CompanyResourceIdentifierResource = "Broker";

        public const string ExpensesCategoryRent = "RENT";
        public const string ExpensesCategorySaving = "SAVING";
        public const string ExpensesCategoryInsurance = "INSURANCE";
        public const string ExpensesCategoryOther = "OTHER";
        public const string ExpensesCategoryAlimony = "ALIMONY";

        public const string LoanApplicationCounterPartyDomain = "CM";
        public const string LoanApplicationCounterPartyResource = "Customer";

        public const string PersonKbIdentityScheme = "KBAD";
        public const string PersonMpIdentityScheme = "MPAD";

        public const string CounterpartyMaritalStatusDefault = "M";

        public const string LiabilityTypeInstallments = "1;2";
        public const string LiabilityTypeCredits = "3;4";

        public const string IncomeTransformation = "1,3,4,1|2,12";  // "RENT,OTHER,SALARY,Momth=1|ENTERPRISE,Month=12";
    }

    private enum Counterparty
    {
        IsNotPartner = 0,
        IsPartner = 1
    }

    private enum Mandant
    {
        MPSS = 1,
        KBCZ = 2
    }

    private enum PersonType : sbyte
    {
        Person,
        Dealer
    }

    /// <summary>
    /// Household (CreditLiabilitiesSummary, InstallmentsSummary)
    /// </summary>
    private class HouseholdLiabilitySummaryTableItem
    {
        public int LiablityType { get; set; }
        public bool OutHomeCompnyFlag { get; set; }
        public string ProductGroup { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountConsolidated { get; set; }
        public HouseholdLiabilitySummaryTableItem()
        {
            ProductGroup = string.Empty;
            Amount = 0;
            AmountConsolidated = 0;
        }
    }

    private class CounterpartyIncomeTableItem
    {
        /// <summary>
        /// NOBY code
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// C4M code
        /// </summary>
        public string Code { get; set; }
        public int Month { get; set; }
        public double Amount { get; set; }

        public CounterpartyIncomeTableItem()
        {
            Month = 0;
            Amount = 0;
        }
    }

    private readonly ILogger<CreditWorthinessComputeRequestTransformation> _logger;
    private readonly IPersonDealerService _personDealerService;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
 
    private List<HouseholdLiabilitySummaryTableItem> HouseholdLiabilitySummaryTable { get; set; }
    private List<CounterpartyIncomeTableItem> CounterpartyIncomeTable { get; set; }

    private Contracts.CreditWorthinessRequest RipRequest { get; set; }
    private C4M.CreditWorthinessCalculationArguments C4mRequest { get; set; } = new C4M.CreditWorthinessCalculationArguments();

    public CreditWorthinessComputeRequestTransformation(
        ILogger<CreditWorthinessComputeRequestTransformation> logger,
        IPersonDealerService personDealerService,
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _logger = logger;
        _codebookService = codebookService;
        _personDealerService = personDealerService;
    }

    /// <summary>
    /// Create C4M request (transformation, validation)
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public async Task<C4M.CreditWorthinessCalculationArguments> Transform(Contracts.CreditWorthinessRequest ripRequest)
    {
        RipRequest = ripRequest;

        if (ripRequest is null)
            throw new CisValidationException("Chybná vstupní struktura!");

        //Transformace
        C4mRequest.ItChannel = RipRequest.ItChannel;
        AddRootRiskBusinessCaseId();
        AddRootResourceProcessId();
        await AddRootDealerOrPerson();
        await AddRootLoanApplicationProduct();
        await AddRootHouseholds();

        return C4mRequest;
    }

    /// <summary>
    /// Add RiskBusinessaseId
    /// </summary>
    private void AddRootRiskBusinessCaseId()
    {
        bool isNumber = long.TryParse(RipRequest.RiskBusinessCaseIdMp, out long Id);
        if (!isNumber)
        {
            Id = 0;
        }
        C4mRequest.RiskBusinessCaseId = Id;
    }

    #region Add ResourceProcessId
    /// <summary>
    /// Add ResourceProcessId
    /// </summary>
    private void AddRootResourceProcessId()
    {
        C4mRequest.ResourceProcessId = new C4M.ResourceIdentifier
        {
            Domain = Constants.ResourceProcessIdDomain,
            Instance = Constants.ResourceProcessIdInstance,
            Resource = Constants.ResourceProcessIdResource,
            //Variant = Constants.ResourceProcessIdVariant,
            Variant = RipRequest.ItChannel,
            Id = RipRequest.ResourceProcessIdMp
        };
    }
    #endregion

    #region Add DealerOrPerson
    private async Task AddRootDealerOrPerson()
    {
        // call DB function to get data about Person/Dealer
        var user = await _personDealerService.GetUserData(RipRequest.HumanUser.Identity, RipRequest.HumanUser.IdentityScheme);

        if (!user.PersonDealerExists)
        {
            _logger.LogError(null, $"Nenalezen uživatel pro Identity={RipRequest.HumanUser.Identity}, IdentityScheme={RipRequest.HumanUser.IdentityScheme}");
            throw new CisValidationException($"Nenalezen uživatel pro Identity={RipRequest.HumanUser.Identity}, IdentityScheme={RipRequest.HumanUser.IdentityScheme}");
        }
        if (user.IsDealer)
        {
            C4M.Dealer dealer = new C4M.Dealer
            {
                Id = GetDealerOrPersonResourceIdentifier((sbyte)PersonType.Dealer),
                CompanyId = GetCompanyResourceIdentifier()
            };
            C4mRequest.LoanApplicationDealer = dealer;
        }
        else
        {
            C4M.Person person = new C4M.Person
            {
                Surname = user.PersonSurname,
                Id = GetDealerOrPersonResourceIdentifier((sbyte)PersonType.Person),
                //+OrgUnit
            };
            C4M.OrganizationUnit orgUnit = new C4M.OrganizationUnit
            {
                Id = user.PersonOrgUnitId.ToString(),
                Name = user.PersonOrgUnitName,
                //+JobPost
            };
            C4M.JobPost jobPost = new C4M.JobPost
            {
                Id = user.PersonJobPostId
            };
            orgUnit.JobPost = jobPost;
            person.OrgUnit = orgUnit;

            C4mRequest.KbGroupPerson = person;
        }
    }
    /// <summary>
    /// Return ResourceIdentifier for Dealer or Person
    /// </summary>
    /// <returns></returns>
    private C4M.ResourceIdentifier GetDealerOrPersonResourceIdentifier(sbyte personType)
    {
        var resourceIdentifier = new C4M.ResourceIdentifier
        {
            Id = RipRequest.HumanUser.Identity,
            Variant = RipRequest.ItChannel
        };

        resourceIdentifier.Instance = RipRequest.HumanUser.IdentityScheme.Equals(Constants.PersonKbIdentityScheme)
            ? Constants.PersonKbResourceIdentifierKbInstance
            : Constants.PersonKbResourceIdentifierMpssInstance;

        switch (personType)
        {
            case (sbyte)PersonType.Person:
                resourceIdentifier.Domain = Constants.PersonKbResourceIdentifierDomain;
                resourceIdentifier.Resource = Constants.PersonKbResourceIdentifierResource;

                break;
            default:
                resourceIdentifier.Domain = Constants.DealerResourceIdentifierDomain;
                resourceIdentifier.Resource = Constants.DealerResourceIdentifierResource;
                break;
        }
        return resourceIdentifier;
    }

    /// <summary>
    /// Company resource identifier
    /// </summary>
    /// <returns></returns>
    private C4M.ResourceIdentifier GetCompanyResourceIdentifier()
    {
        C4M.ResourceIdentifier resourceIdentifier = new C4M.ResourceIdentifier
        {
            Domain = Constants.CompanyResourceIdentifierDomain,
            Resource = Constants.CompanyResourceIdentifierResource,
            Id = RipRequest.HumanUser.Identity,
            Variant = RipRequest.ItChannel
        };

        resourceIdentifier.Instance = RipRequest.HumanUser.IdentityScheme.Equals(Constants.PersonKbIdentityScheme)
            ? Constants.PersonKbResourceIdentifierKbInstance
            : Constants.PersonKbResourceIdentifierMpssInstance;

        return resourceIdentifier;
    }

    #endregion

    #region Add LoanApplicationProduct
    /// <summary>
    /// Loan Application Product
    /// </summary>
    private async Task AddRootLoanApplicationProduct()
    {
        C4M.LoanApplicationProduct c4mProduct = new C4M.LoanApplicationProduct
        {
            ProductClusterCode = (await GetRiskApplicationTypeItem(RipRequest.LoanApplicationProduct.Product))?.C4mAplCode,
            Maturity = RipRequest.LoanApplicationProduct.Maturity,
            InterestRate = (decimal)RipRequest.LoanApplicationProduct.InterestRate,
            AmountRequired = RipRequest.LoanApplicationProduct.AmountRequired,
            Annuity = RipRequest.LoanApplicationProduct.Annuity,
            FixationPeriod = RipRequest.LoanApplicationProduct.FixationPeriod
        };
        C4mRequest.LoanApplicationProduct = c4mProduct;
    }
    #endregion

    /// <summary>
    /// Překlady pomocí číselníku RiskApplicationType
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> GetRiskApplicationTypeItem(int product)
    {
        
        var item = (await _codebookService.RiskApplicationTypes()).FirstOrDefault(m => m.ProductTypeId != null && m.ProductTypeId.Contains(product));

        _logger.LogInformation($"Get item z číselníku RiskAppType: Product = {product} --> AppTypeId = {item.C4mAplTypeId}, MandantId = {item.MandantId}");

        if (item == null)
        {
            _logger.LogError(null, $"Product {product} nenalezen v číselníku RiskApplicationType!");
            throw new CisValidationException($"Product {product} nenalezen v číselníku RiskApplicationType!");
        }
        return item;
    }

    #region Add Households
    /// <summary>
    /// Households
    /// </summary>
    private async Task AddRootHouseholds()
    {
        C4mRequest.Households = new List<C4M.LoanApplicationHousehold>();
        foreach (Contracts.LoanApplicationHousehold ripHousehold in RipRequest.Households)
        {
            C4mRequest.Households.Add(await GetHousehold(ripHousehold));
        }
    }
    /// <summary>
    /// Household
    /// </summary>
    /// <param name="ripHousehold"></param>
    /// <returns></returns>
    private async Task<C4M.LoanApplicationHousehold> GetHousehold(Contracts.LoanApplicationHousehold ripHousehold)
    {
        C4M.LoanApplicationHousehold c4mHousehold = new C4M.LoanApplicationHousehold
        {
            ChildrenUnderAnd10 = ripHousehold.ChildrenUnderAnd10,
            ChildrenOver10 = ripHousehold.ChildrenOver10
        };

        AddHouseholdExpensesSummary(ripHousehold, c4mHousehold);
        await AddHouseholdClients(ripHousehold, c4mHousehold);

        return c4mHousehold;
    }
    /// <summary>
    /// Household expenses
    /// </summary>
    /// <param name="ripHousehold"></param>
    /// <param name="c4mHousehold"></param>
    private void AddHouseholdExpensesSummary(Contracts.LoanApplicationHousehold ripHousehold, C4M.LoanApplicationHousehold c4mHousehold)
    {
        List<C4M.ExpensesSummary> c4mExpenses = new List<C4M.ExpensesSummary>
        {
            GetHouseholdExpensesSummary(Constants.ExpensesCategoryAlimony, ripHousehold.ExpensesSummary),
            GetHouseholdExpensesSummary(Constants.ExpensesCategoryInsurance, ripHousehold.ExpensesSummary),
            GetHouseholdExpensesSummary(Constants.ExpensesCategoryOther, ripHousehold.ExpensesSummary),
            GetHouseholdExpensesSummary(Constants.ExpensesCategoryRent, ripHousehold.ExpensesSummary),
            GetHouseholdExpensesSummary(Constants.ExpensesCategorySaving, ripHousehold.ExpensesSummary)
        };

        c4mHousehold.ExpensesSummary = c4mExpenses;
    }
    /// <summary>
    /// Get Household expenses
    /// </summary>
    /// <param name="category"></param>
    /// <param name="ripExpenses"></param>
    /// <returns></returns>
    private C4M.ExpensesSummary GetHouseholdExpensesSummary(string category, Contracts.ExpensesSummary ripExpenses)
    {
        C4M.ExpensesSummary c4mExpenses = new C4M.ExpensesSummary()
        { Category = category };

        switch (category)
        {
            case Constants.ExpensesCategoryAlimony:
                c4mExpenses.Amount = 0;
                break;
            case Constants.ExpensesCategoryInsurance:
                try { c4mExpenses.Amount = (decimal)ripExpenses.Insurance; }
                catch { c4mExpenses.Amount = 0; }
                break;
            case Constants.ExpensesCategoryOther:
                try { c4mExpenses.Amount = (decimal)ripExpenses.Other; }
                catch { c4mExpenses.Amount = 0; }
                break;
            case Constants.ExpensesCategoryRent:
                try { c4mExpenses.Amount = (decimal)ripExpenses.Rent; }
                catch { c4mExpenses.Amount = 0; }
                break;
            case Constants.ExpensesCategorySaving:
                try { c4mExpenses.Amount = (decimal)ripExpenses.Saving; }
                catch { c4mExpenses.Amount = 0; }
                break;
        }
        return c4mExpenses;
    }
    /// <summary>
    /// Init summary table
    /// </summary>
    private async Task InitHouseholdLiabilitySummaryTable()
    {
        var cb = await _codebookService.ObligationTypes();
        HouseholdLiabilitySummaryTable = new List<HouseholdLiabilitySummaryTableItem>();
        foreach (var item in cb)
        {
            if (HouseholdLiabilitySummaryTable.Exists(x => x.LiablityType == item.Id))
            {
                continue;
            }
            else
            {
                HouseholdLiabilitySummaryTable.Add(new HouseholdLiabilitySummaryTableItem { LiablityType = item.Id, ProductGroup = item.Code, OutHomeCompnyFlag = true });
                HouseholdLiabilitySummaryTable.Add(new HouseholdLiabilitySummaryTableItem { LiablityType = item.Id, ProductGroup = item.Code, OutHomeCompnyFlag = false });
            }
        }
        _logger.LogInformation($"HouseholdLiabilitySummaryTable: Počet položek = {HouseholdLiabilitySummaryTable.Count}.");

        foreach (var item in cb)
        {
            _logger.LogInformation($"Číselník");
            _logger.LogInformation($"Id={item.Id} ProductCode/Code={item.Code} Name={item.Name}");
            if (string.IsNullOrEmpty(item.Code))
                _logger.LogInformation("ERROR: ProductCode v čísekníku nezadáno.");
        }
    }

    /// <summary>
    /// Household clients
    /// </summary>
    /// <param name="ripHousehold"></param>
    /// <param name="c4mHousehold"></param>
    private async Task AddHouseholdClients(Contracts.LoanApplicationHousehold ripHousehold, C4M.LoanApplicationHousehold c4mHousehold)
    {
        c4mHousehold.Clients = new List<C4M.LoanApplicationCounterParty>();
        await InitHouseholdLiabilitySummaryTable();
        foreach (Contracts.LoanApplicationCounterParty ripCounterparty in ripHousehold.Clients)
        {
            C4M.LoanApplicationCounterParty c4mCounterParty = new C4M.LoanApplicationCounterParty
            {
                Id = await GetResourceIdentifierClient(ripCounterparty.IdMp),
                IsPartner = ripCounterparty.IsPartnerMp ? (int)Counterparty.IsPartner : (int)Counterparty.IsNotPartner,
                MaritalStatus = await GetCounterpartyMaritalStatus(ripCounterparty)
            };

            SumHouseholdLiabilities(ripCounterparty);
            await AddCounterPartyIncome(ripCounterparty, c4mCounterParty);
            c4mHousehold.Clients.Add(c4mCounterParty);
        }
        AddHouseholdLiabilities(c4mHousehold);
    }

    private async Task<C4M.ResourceIdentifier> GetResourceIdentifierClient(string idMp)
    {
        C4M.ResourceIdentifier resourceIdentifier = new C4M.ResourceIdentifier
        {
            Id = idMp
        };

        var mandant = (await GetRiskApplicationTypeItem(RipRequest.LoanApplicationProduct.Product))?.MandantId;
        if (mandant.HasValue)
        {
            resourceIdentifier.Instance = ((Mandant)(int)mandant).ToString();
        }
        return resourceIdentifier;
    }

    private async Task<string> GetCounterpartyMaritalStatus(Contracts.LoanApplicationCounterParty ripCounterparty)
    {
        string result = Constants.CounterpartyMaritalStatusDefault;
        var maritalStatus = (await _codebookService.MaritalStatuses()).Where(m => m.Id == ripCounterparty.MaritalStatusMp).FirstOrDefault();

        if (maritalStatus != null)
        {
            result = maritalStatus.RdmMaritalStatusCode;
        }
        return result;
    }
    /// <summary>
    /// Counterparty Income
    /// </summary>
    /// <param name="ripCounterparty"></param>
    /// <param name="c4mCounterParty"></param>
    private async Task AddCounterPartyIncome(Contracts.LoanApplicationCounterParty ripCounterparty, C4M.LoanApplicationCounterParty c4mCounterParty)
    {
        _logger.LogInformation("Add CounterPartyIncome start.");
        if (ripCounterparty.LoanApplicationIncome == null)
        {
            return;
        }

        c4mCounterParty.LoanApplicationIncome = new List<C4M.LoanApplicationIncome>();
        await InitCounterpartyIncomeTable();
        foreach (var ripIncome in ripCounterparty.LoanApplicationIncome)
        {
            foreach (var item in CounterpartyIncomeTable.Where(x => x.Id == ripIncome.CategoryMp))
            {
                item.Amount = ripIncome.Amount;
                _logger.LogInformation($"Income připočteno {item.Amount}");
            }
        }

        _logger.LogInformation($"Income načteno.");
        foreach (CounterpartyIncomeTableItem item in CounterpartyIncomeTable)
        {
            C4M.LoanApplicationIncome c4mIncome = new C4M.LoanApplicationIncome
            {
                Amount = (decimal)item.Amount,
                Month = item.Month,
                Category = item.Code
            };

            c4mCounterParty.LoanApplicationIncome.Add(c4mIncome);
        }
    }

    /// <summary>
    /// Init Counterparty Income table
    /// </summary>
    private async Task InitCounterpartyIncomeTable()
    {
        _logger.LogInformation("InitCounterpartyIncomeTable start.");
        CounterpartyIncomeTable = new List<CounterpartyIncomeTableItem>();
        var cb = await _codebookService.IncomeMainTypes();
        foreach (var item in cb)
        {
            CounterpartyIncomeTable.Add(new CounterpartyIncomeTableItem { Id = item.Id, Code = item.Code });

            _logger.LogInformation($"Přidáno Id={item.Id}, Code={item.Code} Name={item.Name}");
            if (string.IsNullOrEmpty(item.Code))
                _logger.LogInformation($"ERROR: Code v číselníku nezadáno.");
        }

        foreach (CounterpartyIncomeTableItem item in CounterpartyIncomeTable)
        {
            item.Month = GetCounterpartyIncomeMonth(item.Id);
            _logger.LogInformation($"IncomeTable přidáno. CodeId = {item.Id} Month = {item.Month}");
        }
        _logger.LogInformation($"IncomeTable připraveno. Počet záznamů={CounterpartyIncomeTable.Count}");
    }
    /// <summary>
    /// Get Counterparty Income Month
    /// </summary>
    /// <param name="categoryMp"></param>
    /// <returns></returns>
    private int GetCounterpartyIncomeMonth(int categoryMp)
    {
        int result = 0;
        var working = Constants.IncomeTransformation
         .Split('|')
         .Select(i => i.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();

        foreach (var item in working)
        {
            if (item.Contains(categoryMp.ToString()))
                result = int.Parse(item.LastOrDefault());
        }

        return result;
    }

    /// <summary>
    /// Sum Household Liabilities
    /// </summary>
    /// <param name="ripCounterparty"></param>
    private void SumHouseholdLiabilities(Contracts.LoanApplicationCounterParty ripCounterparty)
    {
        if (ripCounterparty.CreditLiabilities == null)
        {
            return;
        }

        foreach (Contracts.CreditLiability creditLiability in ripCounterparty.CreditLiabilities)
        {
            _logger.LogInformation($"Zpracovávám liabilitu: Type={creditLiability.LiabilityType} / Limit={creditLiability.Limit} / {creditLiability.AmountConsolidated} / {creditLiability.Installment} /{creditLiability.InstallmentConsolidated}");
            foreach (var item in HouseholdLiabilitySummaryTable.Where(x => x.LiablityType == creditLiability.LiabilityType && x.OutHomeCompnyFlag == creditLiability.OutHomeCompanyFlag))
            {
                //kreditní
                if (Constants.LiabilityTypeCredits.Contains(creditLiability.LiabilityType.ToString()))
                {
                    item.Amount += (decimal)creditLiability.Limit;
                    item.AmountConsolidated += creditLiability.AmountConsolidated;
                }
                //splátkové
                else if (Constants.LiabilityTypeInstallments.Contains(creditLiability.LiabilityType.ToString()))
                {
                    item.Amount += (decimal)creditLiability.Installment;
                    item.AmountConsolidated += (decimal)creditLiability.InstallmentConsolidated;
                }
            }
        }
    }

    /// <summary>
    /// Household Liabilities
    /// </summary>
    /// <param name="c4mHousehold"></param>
    private void AddHouseholdLiabilities(C4M.LoanApplicationHousehold c4mHousehold)
    {
        c4mHousehold.InstallmentsSummary = new List<C4M.InstallmentsSummaryHomeCompany>();
        c4mHousehold.InstallmentsSummaryOut = new List<C4M.InstallmentsSummaryOutHomeCompany>();
        c4mHousehold.CreditLiabilitiesSummary = new List<C4M.CreditLiabilitiesSummaryHomeCompany>();
        c4mHousehold.CreditLiabilitiesSummaryOut = new List<C4M.CreditLiabilitiesSummary>();

        var sumy = HouseholdLiabilitySummaryTable.Where(x => Constants.LiabilityTypeInstallments.Contains(x.LiablityType.ToString()) && x.OutHomeCompnyFlag == false).ToList();
        foreach (HouseholdLiabilitySummaryTableItem item in sumy)
        {
            c4mHousehold.InstallmentsSummary.Add(new C4M.InstallmentsSummaryHomeCompany { ProductGroup = item.ProductGroup, Amount = item.Amount, AmountConsolidated = item.AmountConsolidated });
        }
        sumy = HouseholdLiabilitySummaryTable.Where(x => Constants.LiabilityTypeInstallments.Contains(x.LiablityType.ToString()) && x.OutHomeCompnyFlag == true).ToList();
        foreach (HouseholdLiabilitySummaryTableItem item in sumy)
        {
            c4mHousehold.InstallmentsSummaryOut.Add(new C4M.InstallmentsSummaryOutHomeCompany { ProductGroup = item.ProductGroup, Amount = item.Amount, AmountConsolidated = item.AmountConsolidated });
        }

        sumy = HouseholdLiabilitySummaryTable.Where(x => Constants.LiabilityTypeCredits.Contains(x.LiablityType.ToString()) && x.OutHomeCompnyFlag == false).ToList();
        foreach (HouseholdLiabilitySummaryTableItem item in sumy)
        {
            c4mHousehold.CreditLiabilitiesSummary.Add(new C4M.CreditLiabilitiesSummaryHomeCompany { ProductGroup = item.ProductGroup, Amount = item.Amount, AmountConsolidated = item.AmountConsolidated });
        }
        sumy = HouseholdLiabilitySummaryTable.Where(x => Constants.LiabilityTypeCredits.Contains(x.LiablityType.ToString()) && x.OutHomeCompnyFlag == true).ToList();
        foreach (HouseholdLiabilitySummaryTableItem item in sumy)
        {
            c4mHousehold.CreditLiabilitiesSummaryOut.Add(new C4M.CreditLiabilitiesSummary { ProductGroup = item.ProductGroup, Amount = item.Amount, AmountConsolidated = item.AmountConsolidated });
        }
    }

    #endregion
}
