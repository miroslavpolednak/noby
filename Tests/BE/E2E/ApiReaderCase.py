from typing import List

from .Processing import Processing

from business.case import Case

from fe_api import FeAPI
from common import Log

class ApiReaderCase():

    def __init__(self):
        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')
   

    def load(self, case_id: int) -> Case | Exception:

        result: Case | Exception = None

        try:
            result = self.__load_case(case_id)
        except Exception as e:
            result = e

        return result


    def __load_case(self, case_id: int) -> Case:

        # CASE
        res_case = FeAPI.Case.get_case(case_id)

        # SA
        # https://wiki.kb.cz/display/HT/getSalesArrangementList # proč collection? mohou být různá salesArrangementId v jednotlivých položkách ???
        sales_arrangement_list = FeAPI.SalesArrangement.get_sales_arrangement_list(case_id)
        sales_arrangement_id: int = sales_arrangement_list[0]['salesArrangementId']

        print(f'sales_arrangement_id: {sales_arrangement_id}')

        households: dict = self.__load_households(sales_arrangement_id)
        parameters: dict = self.__load_parameters(sales_arrangement_id)

        # ['phoneNumberForOffer','emailForOffer','households','parameters']
        json_dict = dict(
            #offer = dict(),
            phoneNumberForOffer = Processing.get_key(res_case, 'phoneNumberForOffer'),
            emailForOffer = Processing.get_key(res_case, 'emailForOffer'),
            households = households,
            parameters = parameters,
        )

        return Case(json_dict)


    def __load_parameters(self, sales_arrangement_id: int) -> dict:

        # call FE API endpoint
        res = FeAPI.SalesArrangement.get_sales_arrangement(sales_arrangement_id)
        res_parameters = Processing.get_key(res, 'parameters')
        
        def parse_loan_real_estate(loan_real_estate_item: dict) -> dict:
            # ['realEstateTypeId', 'isCollateral','realEstatePurchaseTypeId']
            return dict(
                realEstateTypeId = Processing.get_key(loan_real_estate_item, 'realEstateTypeId'),
                isCollateral = Processing.get_key(loan_real_estate_item, 'isCollateral'),
                realEstatePurchaseTypeId = Processing.get_key(loan_real_estate_item, 'realEstatePurchaseTypeId'),
            )

        parameters = None
        if res_parameters is not None:

            res_loan_real_estates = Processing.get_key(res_parameters, 'loanRealEstates')
            loan_real_estates: List[dict] = None
            
            if res_loan_real_estates is not None:
                loan_real_estates = list(map(lambda loan_real_estate_item: parse_loan_real_estate(loan_real_estate_item), res_loan_real_estates))

            # ['expectedDateOfDrawing','incomeCurrencyCode','residencyCurrencyCode','contractSignatureTypeId','agent','agentConsentWithElCom','loanRealEstates']
            parameters = dict(
                expectedDateOfDrawing = Processing.get_key(res_parameters, 'expectedDateOfDrawing'),
                incomeCurrencyCode = Processing.get_key(res_parameters, 'incomeCurrencyCode'),
                residencyCurrencyCode = Processing.get_key(res_parameters, 'residencyCurrencyCode'),
                contractSignatureTypeId = Processing.get_key(res_parameters, 'contractSignatureTypeId'),
                agent = Processing.get_key(res_parameters, 'agent'),
                agentConsentWithElCom = Processing.get_key(res_parameters, 'agentConsentWithElCom'),
                loanRealEstates = loan_real_estates, 
            )

        return parameters


    def __load_households(self, sales_arrangement_id: int) -> List[dict]:

        # call FE API endpoint
        customer_list = FeAPI.SalesArrangement.get_customers(sales_arrangement_id)
        household_list = FeAPI.Household.get_household_list(sales_arrangement_id)

        households: List[dict] = list(map(lambda household_list_item: self.__load_household(household_list_item), household_list))

        return households


    def __load_household(self, household_list_item: dict) -> dict:
       
        household_id: int = Processing.get_key(household_list_item, 'householdId')
        household_type_id: int = Processing.get_key(household_list_item, 'householdTypeId')
        household_type_name: int = Processing.get_key(household_list_item, 'householdTypeName')

        # call FE API endpoint
        res = FeAPI.Household.get_household(household_id)

        res_data = Processing.get_key(res, 'data')
        res_customer1 = Processing.get_key(res, 'customer1')
        res_customer2 = Processing.get_key(res, 'customer2')
        res_expenses = Processing.get_key(res, 'expenses')

        # customer1
        customer1: dict = None if res_customer1 is None else self.__load_customer(res_customer1)

        # customer2
        customer2: dict = None if res_customer2 is None else self.__load_customer(res_customer2)

        # expenses
        expenses: dict = None
        if res_expenses is not None:
            expenses = dict(
                savingExpenseAmount = Processing.get_key(res_expenses, 'savingExpenseAmount'),
                insuranceExpenseAmount = Processing.get_key(res_expenses, 'insuranceExpenseAmount'),
                housingExpenseAmount = Processing.get_key(res_expenses, 'housingExpenseAmount'),
                otherExpenseAmount = Processing.get_key(res_expenses, 'otherExpenseAmount'),
            )

        household = dict(
            householdId = Processing.get_key(res, 'householdId'),
            householdTypeId = household_type_id,
            childrenUpToTenYearsCount = Processing.get_key(res_data, 'childrenUpToTenYearsCount'),
            childrenOverTenYearsCount = Processing.get_key(res_data, 'childrenOverTenYearsCount'),
            areCustomersPartners = Processing.get_key(res, 'areCustomersPartners'),
            customer1 = customer1,
            customer2 = customer2,
            expenses = expenses
        )

        return household


    def __load_customer(self, household_list_item_customer: dict) -> dict:

        if household_list_item_customer is None:
            return None

        res = household_list_item_customer

        res_identity = Processing.get_key(res, 'identity')
        res_incomes = Processing.get_key(res, 'incomes')
        res_obligations = Processing.get_key(res, 'obligations')

        customer_on_sa_id = Processing.get_key(res, 'customerOnSAId')

        # identity
        identity = None
        if res_identity is not None:
            identity = dict(
                id = Processing.get_key(res_identity, 'id'),
                scheme = Processing.get_key(res_identity, 'scheme'),
            )

        # incomes
        incomes = None if res_incomes is None else list(map(lambda res_income_item: self.__load_income(customer_on_sa_id, res_income_item), res_incomes))

        # obligations
        obligations = None if res_obligations is None else list(map(lambda res_obligation_item: self.__load_obligation(customer_on_sa_id, res_obligation_item), res_obligations))

        # customer
        customer = dict(
            customerOnSAId = customer_on_sa_id,
            roleId = Processing.get_key(res, 'roleId'),
            firstName = Processing.get_key(res, 'firstName'),
            lastName = Processing.get_key(res, 'lastName'),
            dateOfBirth = Processing.get_key(res, 'dateOfBirth'),

            identity = identity,
            incomes = incomes,
            obligations = obligations,
        )

        return customer


    def __load_income(self, customer_on_sa_id: int, household_list_item_customer_income: dict) -> dict:

        if customer_on_sa_id is None or household_list_item_customer_income is None:
            return None

        income_id = Processing.get_key(household_list_item_customer_income, 'incomeId')

        # call FE API endpoint
        # res = household_list_item_customer_income
        res = FeAPI.CustomerOnSa.get_income(customer_on_sa_id, income_id)
        res_data = Processing.get_key(res, 'data')

        # data
        data = None
        if res_data is not None:

            res_employer = Processing.get_key(res_data, 'employer')
            res_job = Processing.get_key(res_data, 'job')
            res_wage_deduction = Processing.get_key(res_data, 'wageDeduction')
            res_income_confirmation = Processing.get_key(res_data, 'incomeConfirmation')

            # employer
            employer = None
            if res_employer is not None:
                employer = dict(
                    name = Processing.get_key(res_employer, 'name'),
                    cin = Processing.get_key(res_employer, 'cin'),
                    birthNumber = Processing.get_key(res_employer, 'birthNumber'),
                    countryId = Processing.get_key(res_employer, 'countryId'),
                )

            # job
            job = None
            if res_job is not None:
                job = dict(
                    jobDescription = Processing.get_key(res_job, 'jobDescription'),
                    firstWorkContractSince = Processing.get_key(res_job, 'firstWorkContractSince'),
                    employmentTypeId = Processing.get_key(res_job, 'employmentTypeId'),
                    currentWorkContractSince = Processing.get_key(res_job, 'currentWorkContractSince'),
                    currentWorkContractTo = Processing.get_key(res_job, 'currentWorkContractTo'),
                    grossAnnualIncome = Processing.get_key(res_job, 'grossAnnualIncome'),
                    isInTrialPeriod = Processing.get_key(res_job, 'isInTrialPeriod'),
                    isInProbationaryPeriod = Processing.get_key(res_job, 'isInProbationaryPeriod'),
                )

            # wageDeduction
            wage_deduction = None
            if res_wage_deduction is not None:
                wage_deduction = dict(
                    deductionDecision = Processing.get_key(res_wage_deduction, 'deductionDecision'),
                    deductionPayments = Processing.get_key(res_wage_deduction, 'deductionPayments'),
                    deductionOther = Processing.get_key(res_wage_deduction, 'deductionOther'),
                )

            # incomeConfirmation
            income_confirmation = None
            if res_income_confirmation is not None:
                income_confirmation = dict(
                    confirmationDate = Processing.get_key(res_income_confirmation, 'confirmationDate'),
                    confirmationPerson = Processing.get_key(res_income_confirmation, 'confirmationPerson'),
                    confirmationContact = Processing.get_key(res_income_confirmation, 'confirmationContact'),
                    isIssuedByExternalAccountant = Processing.get_key(res_income_confirmation, 'isIssuedByExternalAccountant'),   
                )

            # data
            data = dict(
                employer = employer,
                job = job,
                wageDeduction = wage_deduction,
                incomeConfirmation = income_confirmation,

                incomeSource = Processing.get_key(res_data, 'incomeSource'),
                hasProofOfIncome = Processing.get_key(res_data, 'hasProofOfIncome'),
                foreignIncomeTypeId = Processing.get_key(res_data, 'foreignIncomeTypeId'),
                hasWageDeduction = Processing.get_key(res_data, 'hasWageDeduction'),
                cin = Processing.get_key(res_data, 'cin'),
                birthNumber = Processing.get_key(res_data, 'birthNumber'),
                countryOfResidenceId = Processing.get_key(res_data, 'countryOfResidenceId'),
                incomeOtherTypeId = Processing.get_key(res_data, 'incomeOtherTypeId'),
            )

        # income
        income = dict(
            incomeId = Processing.get_key(res, 'incomeId'),
            incomeTypeId = Processing.get_key(res, 'incomeTypeId'),            
            sum = Processing.get_key(res, 'sum'),
            currencyCode = Processing.get_key(res, 'currencyCode'),

            data = data
        )

        return income


    def __load_obligation(self, customer_on_sa_id: int, household_list_item_customer_obligation: dict) -> dict:

        if customer_on_sa_id is None or household_list_item_customer_obligation is None:
            return None

        obligation_id = Processing.get_key(household_list_item_customer_obligation, 'obligationId')

        # call FE API endpoint
        # res = household_list_item_customer_obligation
        res = FeAPI.CustomerOnSa.get_obligation(customer_on_sa_id, obligation_id)

        res_creditor = Processing.get_key(res, 'creditor')
        res_correction = Processing.get_key(res, 'correction')

        # creditor
        creditor = None
        if res_creditor is not None:
            # ['name','isExternal', 'creditorId']
            creditor = dict(
                name = Processing.get_key(res_creditor, 'name'),
                isExternal = Processing.get_key(res_creditor, 'isExternal'),
                creditorId = Processing.get_key(res_creditor, 'creditorId'),
            )

        # correction
        correction = None
        if res_correction is not None:
            correction = dict(
                correctionTypeId = Processing.get_key(res_correction, 'correctionTypeId'),
                installmentAmountCorrection = Processing.get_key(res_correction, 'installmentAmountCorrection'),
                loanPrincipalAmountCorrection = Processing.get_key(res_correction, 'loanPrincipalAmountCorrection'),
                creditCardLimitCorrection = Processing.get_key(res_correction, 'creditCardLimitCorrection'),
            )

        # obligation
        obligation = dict(
            obligationId = Processing.get_key(res, 'obligationId'),
            obligationTypeId = Processing.get_key(res, 'obligationTypeId'),  
            obligationState = Processing.get_key(res, 'obligationState'),
            installmentAmount = Processing.get_key(res, 'installmentAmount'),
            loanPrincipalAmount = Processing.get_key(res, 'loanPrincipalAmount'),
            creditCardLimit = Processing.get_key(res, 'creditCardLimit'),
            amountConsolidated = Processing.get_key(res, 'amountConsolidated'),

            creditor = creditor,
            correction = correction,
        )

        return obligation
