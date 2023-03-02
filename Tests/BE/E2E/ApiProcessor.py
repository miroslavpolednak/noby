from typing import List

from .EProcessKey import EProcessKey
from .Processing import Processing

from business.offer import Offer
from business.case import Case

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI
from common import Log

class ApiProcessor():

    def __init__(self, case: Case):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')

        self.__case = case
        self.__case_json = case.to_json_value()

    @staticmethod
    def process_list(cases: List[Case]) -> List[dict]:

        results: List[dict] = []

        for case in cases:
            r: dict = ApiProcessor(case).process()
            results.append(r)

        return results

    def process(self) -> dict:

        result: dict = None

        try:
            result = self.__process()
        except Exception as e:
            result = dict(Error = e)

        return result

    def __process(self) -> dict:

        self.__create_offer()
        self.__create_case()

        offer_json: dict = Processing.get_key(self.__case_json, 'offer')

        offer_id = Processing.get_process_key(offer_json, EProcessKey.OFFER_ID)
        case_id = Processing.get_process_key(self.__case_json, EProcessKey.CASE_ID)
        sales_arrangement_id = Processing.get_process_key(self.__case_json, EProcessKey.SALES_ARRAMGEMENT_ID)

        return dict(
            offer_id = offer_id,
            case_id = case_id,
            sales_arrangement_id = sales_arrangement_id,
        )

    def __create_offer(self):

        offer_json: dict = Processing.get_key(self.__case_json, 'offer')
        assert offer_json is not None, 'Offer data not provided!'

        req = offer_json

        # call FE API endpoint 
        self.__log.info(f'create_offer.req [{req}]')
        res = FeAPI.Offer.simulate_mortgage(req)
        self.__log.info(f'create_offer.res [{res}]')

        # set process data
        offer_id: int = res['offerId']
        Processing.set_process_key(offer_json, EProcessKey.OFFER_ID, offer_id)

    def __create_case(self):

        # get process data
        offer_json: dict = Processing.get_key(self.__case_json, 'offer')
        offer_id = Processing.get_process_key(offer_json, EProcessKey.OFFER_ID)
        
        household_json = list(filter(lambda i: int(i['householdTypeId']) == EHouseholdType.Main.value, self.__case_json['households']))[0]
        customer_json = Processing.get_key(household_json,'customer1')

        req = dict(
            offerId = offer_id,
            firstName = Processing.get_key(customer_json, 'firstName', ''),
            lastName = Processing.get_key(customer_json, 'lastName', ''),
            dateOfBirth = Processing.get_key(customer_json, 'dateOfBirth'),
            phoneNumberForOffer = Processing.get_key(customer_json, 'phoneNumberForOffer'),
            emailForOffer = Processing.get_key(customer_json, 'emailForOffer'),
            identity = Processing.get_key(customer_json, 'identity'),
        )

        # call FE API endpoint
        self.__log.info(f'create_case.req [{req}]')
        res = FeAPI.Offer.create_case(req)
        self.__log.info(f'create_case.res [{res}]')

        # set test data
        Processing.set_process_key(self.__case_json, EProcessKey.CASE_ID, res['caseId'])
        Processing.set_process_key(self.__case_json, EProcessKey.SALES_ARRAMGEMENT_ID, res['salesArrangementId'])

        Processing.set_process_key(household_json, EProcessKey.HOUSEHOLD_ID, res['householdId'])
        Processing.set_process_key(customer_json, EProcessKey.CUSTOMER_ON_SA_ID, res['customerOnSAId'])

        # create households
        self.__create_households()

        # create parameters
        self.__create_parameters()

    def __create_households(self):

        # get process data
        sales_arrangement_id = Processing.get_process_key(self.__case_json, EProcessKey.SALES_ARRAMGEMENT_ID)

        household_by_type: dict = dict()

        # create households
        for household_json in self.__case_json['households']:
            household_id: int = Processing.get_process_key(household_json, EProcessKey.HOUSEHOLD_ID)
            household_type_id = household_json['householdTypeId']

            household_by_type[household_type_id] = household_json

            if household_id is None:
                req = dict(
                    salesArrangementId = sales_arrangement_id,
                    householdTypeId = household_type_id
                )

                # call FE API endpoint
                self.__log.info(f'create_households.req [{req}]')
                res = FeAPI.Household.create_household(req)
                self.__log.info(f'create_households.res [{res}]')

                household_id = res['householdId']
                Processing.set_process_key(household_json, EProcessKey.HOUSEHOLD_ID, household_id)

            expenses_json = Processing.get_key(household_json, 'expenses')
            req = dict(
                data = dict(
                    childrenUpToTenYearsCount = Processing.get_key(household_json, 'childrenUpToTenYearsCount', 0),
                    childrenOverTenYearsCount = Processing.get_key(household_json, 'childrenOverTenYearsCount', 0),
                    areCustomersPartners = Processing.get_key(household_json, 'areCustomersPartners', False),
                ),
                expenses = dict(
                    savingExpenseAmount = Processing.get_key(expenses_json, 'savingExpenseAmount', None),
                    insuranceExpenseAmount = Processing.get_key(expenses_json, 'insuranceExpenseAmount', None),
                    housingExpenseAmount = Processing.get_key(expenses_json, 'housingExpenseAmount', None),
                    otherExpenseAmount = Processing.get_key(expenses_json, 'otherExpenseAmount', None),
                )
            )

            # call FE API endpoint
            self.__log.info(f'create_households_parameters.req [{req}]')
            res = FeAPI.Household.set_household_parameters(household_id, req)
            self.__log.info(f'create_households_parameters.res [{res}]')
            
        # set test data (ids of customers)
        customers = FeAPI.SalesArrangement.get_customers(sales_arrangement_id)
        for c in customers:
            customer_on_sa_id = c['id']
            household_type_id = c['customerRoleId']
            household_json = household_by_type[household_type_id]
            household_customer_json = Processing.init_key(household_json,'customer1')
            Processing.set_process_key(household_customer_json, EProcessKey.CUSTOMER_ON_SA_ID, customer_on_sa_id)

        # create customers for households
        for household_json in self.__case_json['households']:
            self.__create_customers(household_json)

    def __create_customers(self, household_json: dict):
        
        # get process data
        household_id = Processing.get_process_key(household_json, EProcessKey.HOUSEHOLD_ID)

        def get_req_customer(customer_json: dict) -> dict:

            if (customer_json is None):
                return None

            customer_on_sa_id: int = Processing.get_process_key(customer_json, EProcessKey.CUSTOMER_ON_SA_ID)
            identity: dict = Processing.get_key(customer_json, 'identity', None)

            customer = dict(
                customerOnSAId = customer_on_sa_id,
                firstName = Processing.get_key(customer_json, 'firstName', ''),
                lastName = Processing.get_key(customer_json, 'lastName', ''),
                incomes = [],
                # roleId = 1, #TODO: remove from source JSON !?
                identities = [] if identity is None else [identity],
                obligations = [],
            )

            return customer

        req = dict(
            householdId = household_id,
            customer1 = get_req_customer(household_json['customer1']),
            customer2 = get_req_customer(household_json['customer2']),
        )

        # call FE API endpoint
        self.__log.info(f'create_customers.req [{req}]')
        res = FeAPI.Household.set_household_customers(household_id, req)
        self.__log.info(f'create_customers.res [{res}]')

        # set process data (ids of customers on SA)
        Processing.set_process_key(household_json['customer1'], EProcessKey.CUSTOMER_ON_SA_ID, res['customerOnSAId1'])
        if household_json['customer2'] is not None:
            Processing.set_process_key(household_json['customer2'], EProcessKey.CUSTOMER_ON_SA_ID, res['customerOnSAId2'])

        # create incomes & obligations
        self.__create_incomes(household_json['customer1'])
        self.__create_obligations(household_json['customer1'])
        if household_json['customer2'] is not None:
            self.__create_incomes(household_json['customer2'])
            self.__create_obligations(household_json['customer2'])    

    def __create_incomes(self, customer_json: dict):

        incomes_json = Processing.get_key(customer_json, 'incomes')

        if incomes_json is None:
            return

        customer_on_sa_id: int = Processing.get_process_key(customer_json, EProcessKey.CUSTOMER_ON_SA_ID)

        def get_req_data(income_json: dict) -> dict:
            
            data_json = Processing.get_key(income_json, 'data')

            if (data_json is None):
                return None

            # employer
            employer: dict = None
            employer_json = Processing.get_key(data_json, 'employer')
            if employer_json is not None:
                employer = dict(
                    name = Processing.get_key(employer_json, 'name'),
                    cin = Processing.get_key(employer_json, 'cin'),
                    birthNumber = Processing.get_key(employer_json, 'birthNumber'),
                    countryId = Processing.get_key(employer_json, 'countryId'),
                )

            # job
            job: dict = None
            job_json = Processing.get_key(data_json, 'job')
            if job_json is not None:
                job = dict(
                    jobDescription = Processing.get_key(job_json, 'jobDescription'),
                    firstWorkContractSince = Processing.get_key(job_json, 'firstWorkContractSince'),
                    employmentTypeId = Processing.get_key(job_json, 'employmentTypeId'),
                    currentWorkContractSince = Processing.get_key(job_json, 'currentWorkContractSince'),
                    currentWorkContractTo = Processing.get_key(job_json, 'currentWorkContractTo'),
                    grossAnnualIncome = Processing.get_key(job_json, 'grossAnnualIncome'),
                    isInTrialPeriod = Processing.get_key(job_json, 'isInTrialPeriod'),
                    isInProbationaryPeriod = Processing.get_key(job_json, 'isInProbationaryPeriod'),
                )
            
            # wageDeduction
            wage_deduction: dict = None
            wage_deduction_json = Processing.get_key(data_json, 'wageDeduction')
            if wage_deduction_json is not None:
                wage_deduction = dict(
                    deductionDecision = Processing.get_key(wage_deduction_json, 'deductionDecision'),
                    deductionPayments = Processing.get_key(wage_deduction_json, 'deductionPayments'),
                    deductionOther = Processing.get_key(wage_deduction_json, 'deductionOther'),
                )

            # incomeConfirmation
            income_confirmation: dict = None
            income_confirmation_json = Processing.get_key(data_json, 'incomeConfirmation')
            if wage_deduction_json is not None:
                income_confirmation = dict(
                    confirmationDate = Processing.get_key(income_confirmation_json, 'confirmationDate'),
                    confirmationPerson = Processing.get_key(income_confirmation_json, 'confirmationPerson'),
                    confirmationContact = Processing.get_key(income_confirmation_json, 'confirmationContact'),
                    isIssuedByExternalAccountant = Processing.get_key(income_confirmation_json, 'isIssuedByExternalAccountant'),
                )

            # data
            return dict(
                hasProofOfIncome = Processing.get_key(data_json, 'hasProofOfIncome'),
                foreignIncomeTypeId = Processing.get_key(data_json, 'foreignIncomeTypeId'),
                hasWageDeduction = Processing.get_key(data_json, 'hasWageDeduction'),
                cin = Processing.get_key(data_json, 'cin'),
                birthNumber = Processing.get_key(data_json, 'birthNumber'),
                countryOfResidenceId = Processing.get_key(data_json, 'countryOfResidenceId'),
                incomeOtherTypeId = Processing.get_key(data_json, 'incomeOtherTypeId'),

                employer = employer,
                job = job,
                wageDeduction = wage_deduction,
                incomeConfirmation = income_confirmation,
            )

        # create incomes
        for income_json in incomes_json:
            req = dict(
                sum = income_json['sum'],
                currencyCode = Processing.get_key(income_json, 'currencyCode'),
                incomeTypeId = Processing.get_key(income_json, 'incomeTypeId'),
                data = get_req_data(income_json)
            )

            # call FE API endpoint
            self.__log.info(f'create_incomes.req [{req}]')
            res = FeAPI.CustomerOnSa.create_income(customer_on_sa_id, req)
            self.__log.info(f'create_incomes.res [{res}]')

            income_id: int = res
            Processing.set_process_key(income_json, EProcessKey.INCOME_ID, income_id)

    def __create_obligations(self, customer_json: dict):

        obligations_json = Processing.get_key(customer_json, 'obligations')

        if obligations_json is None:
            return

        # get process data
        customer_on_sa_id = Processing.get_process_key(customer_json, EProcessKey.CUSTOMER_ON_SA_ID)

        # create obligations
        for obligation_json in obligations_json:

            creditor: dict = None
            if 'creditor' in obligation_json:
                creditor_json = obligation_json['creditor']
                creditor = dict(
                    creditorId = Processing.get_key(creditor_json, 'creditorId'),
                    name = Processing.get_key(creditor_json, 'name'),
                    isExternal = Processing.get_key(creditor_json, 'isExternal'),
                )

            correction: dict = None
            if 'correction' in obligation_json:
                correction_json = obligation_json['correction']
                correction = dict(
                    correctionTypeId = Processing.get_key(correction_json, 'correctionTypeId'),
                    installmentAmountCorrection = Processing.get_key(correction_json, 'installmentAmountCorrection'),
                    loanPrincipalAmountCorrection = Processing.get_key(correction_json, 'loanPrincipalAmountCorrection'),
                    creditCardLimitCorrection = Processing.get_key(correction_json, 'creditCardLimitCorrection'),
                )

            req = dict(
                obligationTypeId = Processing.get_key(obligation_json, 'obligationTypeId'),
                obligationState = Processing.get_key(obligation_json, 'obligationState'),
                installmentAmount = Processing.get_key(obligation_json, 'installmentAmount'),
                loanPrincipalAmount = Processing.get_key(obligation_json, 'loanPrincipalAmount'),
                creditCardLimit = Processing.get_key(obligation_json, 'creditCardLimit'),
                amountConsolidated = Processing.get_key(obligation_json, 'amountConsolidated'),
                
                creditor = creditor,
                correction = correction,
            )

            # call FE API endpoint
            self.__log.info(f'create_obligations.req [{req}]')
            res = FeAPI.CustomerOnSa.create_obligation(customer_on_sa_id, req)
            self.__log.info(f'create_obligations.res [{res}]')

            obligation_id: int = res
            Processing.set_process_key(obligation_json, EProcessKey.OBLIGATION_ID, obligation_id)

    def __create_parameters(self):

        parameters_json = Processing.get_key(self.__case_json, 'parameters')

        if parameters_json is None:
            return

        sales_arrangement_id = Processing.get_process_key(self.__case_json, EProcessKey.SALES_ARRAMGEMENT_ID)

        def map_loan_real_estate(loan_real_estate_json: dict)-> dict:
            return dict(
                realEstateTypeId = Processing.get_key(loan_real_estate_json, 'realEstateTypeId'),
                isCollateral = Processing.get_key(loan_real_estate_json, 'isCollateral'),
                realEstatePurchaseTypeId = Processing.get_key(loan_real_estate_json, 'realEstatePurchaseTypeId'),
            )

        loan_real_estates_json = Processing.get_key(parameters_json, 'loanRealEstates', [])
        loan_real_estates = list(map(lambda i_json: map_loan_real_estate(i_json), loan_real_estates_json))

        req = dict(
            parameters = dict(
                incomeCurrencyCode = Processing.get_key(parameters_json, 'incomeCurrencyCode'),
                residencyCurrencyCode = Processing.get_key(parameters_json, 'residencyCurrencyCode'),
                contractSignatureTypeId = Processing.get_key(parameters_json, 'contractSignatureTypeId'),
                # agent = Processing.get_key(parameters_json, 'agent'), #TODO: modificator !?
                agentConsentWithElCom = Processing.get_key(parameters_json, 'agentConsentWithElCom'),
                loanRealEstates = loan_real_estates,
            )
        )

        # call FE API endpoint
        self.__log.info(f'create_parameters.req [{req}]')
        res = FeAPI.SalesArrangement.set_parameters(sales_arrangement_id, req)
        self.__log.info(f'create_parameters.res [{res}]')

    @staticmethod
    def load_case(case_id: int) -> Case | dict:

        result: Case | dict = None

        try:
            result = ApiProcessor.__load_case(case_id)
        except Exception as e:
            result = dict(Error = e)

        return result

    @staticmethod
    def __load_case(case_id: int) -> Case:

        # https://wiki.kb.cz/display/HT/getSalesArrangementList # proč collection? mohou být různá salesArrangementId v jednotlivých položkách ???
        sales_arrangement_list = FeAPI.SalesArrangement.get_sales_arrangement_list(case_id)
        sales_arrangement_id: int = sales_arrangement_list[0]['salesArrangementId']

        print(f'sales_arrangement_id: {sales_arrangement_id}')

        households: dict = ApiProcessor.__load_households(sales_arrangement_id)

        json_dict = dict(
            offer = dict(),
            households = households,
            parameters = dict(),
        )

        return Case(json_dict)

    @staticmethod
    def __load_households(sales_arrangement_id: int) -> List[dict]:

        # call FE API endpoint
        customer_list = FeAPI.SalesArrangement.get_customers(sales_arrangement_id)
        household_list = FeAPI.Household.get_household_list(sales_arrangement_id)

        households: List[dict] = list(map(lambda household_list_item: ApiProcessor.__load_household(household_list_item), household_list))

        return households

    @staticmethod
    def __load_household(household_list_item: dict) -> dict:
       
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
        customer1: dict = None if res_customer1 is None else ApiProcessor.__load_customer(res_customer1)

        # customer2
        customer2: dict = None if res_customer2 is None else ApiProcessor.__load_customer(res_customer2)

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

    @staticmethod
    def __load_customer(household_list_item_customer: dict) -> dict:

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
        incomes = None if res_incomes is None else list(map(lambda res_income_item: ApiProcessor.__load_income(customer_on_sa_id, res_income_item), res_incomes))

        # obligations
        obligations = None if res_obligations is None else list(map(lambda res_obligation_item: ApiProcessor.__load_obligation(customer_on_sa_id, res_obligation_item), res_obligations))

        # customer
        customer = dict(
            customerOnSAId = customer_on_sa_id,
            roleId = Processing.get_key(res, 'roleId'),
            firstName = Processing.get_key(res, 'firstName'),
            lastName = Processing.get_key(res, 'lastName'),
            dateOfBirth = Processing.get_key(res, 'dateOfBirth'),
            phoneNumberForOffer = Processing.get_key(res, 'phoneNumberForOffer'),
            emailForOffer = Processing.get_key(res, 'emailForOffer'),

            identity = identity,
            incomes = incomes,
            obligations = obligations,
        )

        return customer

    @staticmethod
    def __load_income(customer_on_sa_id: int, household_list_item_customer_income: dict) -> dict:

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

    @staticmethod
    def __load_obligation(customer_on_sa_id: int, household_list_item_customer_obligation: dict) -> dict:

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
    

    # @staticmethod
    # def group_list_by_key(list: List[dict], key: str) -> dict:

    #     if list is None or key is None:
    #         return None

    #     result = {}

    #     for i in list:
    #         key_value = i[key]
    #         if key_value not in result:
    #             result[key_value] = []

    #         result[key_value].append(i)

    #     return result


# ----------------------------------------------------------------------------------------------------------------------------------------------
# PREHLED - click na případ
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET = https://fat.noby.cz/api/case/3013351
# {"caseOwner":{"cpm":"990614w","icp":""},"caseId":3013351,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00","state":1,"stateName":"Příprava žádosti","contractNumber":"","targetAmount":5000000,"productName":"Hypoteční úvěr","createdTime":"2023-02-22T10:59:54.373","createdBy":"Filip Tůma","stateUpdated":"2023-02-22T10:59:54.37","emailForOffer":"novak@testcm.cz","phoneNumberForOffer":"+420 777543234"}

# GET https://fat.noby.cz/api/case/3013351/customers
# [{"roleName":"Dlužník","agent":true,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00"},{"roleName":"Spoludlužník","agent":false,"firstName":"JANA","lastName":"Nováková"}]

# GET https://fat.noby.cz/api/sales-arrangement/list/3013351
# [{"productName":"Hypoteční úvěr","salesArrangementId":337,"offerId":829,"salesArrangementTypeId":1,"salesArrangementTypeText":"Žádost o hypotéční úvěr","state":1,"stateText":"Rozpracováno","createdTime":"2023-02-22T10:59:54.86","createdBy":"Filip Tůma"}]

# GET https://fat.noby.cz/api/sales-arrangement/337
# {"productTypeId":20001,"salesArrangementId":337,"salesArrangementTypeId":1,"loanApplicationAssessmentId":"","createdTime":"2023-02-22T10:59:54.86","createdBy":"Filip Tůma","offerGuaranteeDateFrom":"2023-02-22T00:00:00","offerGuaranteeDateTo":"2023-04-08T00:00:00","parameters":{"incomeCurrencyCode":"CAD","residencyCurrencyCode":"CZK","contractSignatureTypeId":2,"loanRealEstates":[{"realEstateTypeId":1,"isCollateral":true,"realEstatePurchaseTypeId":1},{"realEstateTypeId":5,"isCollateral":false,"realEstatePurchaseTypeId":2}],"agent":534,"agentConsentWithElCom":false}}

# GET https://fat.noby.cz/api/case/3013351/parameters
# {"productType":{"id":20001,"name":"Hypoteční úvěr"},"contractNumber":"","loanAmount":5000000,"loanInterestRate":5.89,"fixedRateValidTo":"0001-01-01T00:00:00","drawingDateTo":"2023-03-14T00:00:00","loanPaymentAmount":55235,"loanKind":{"id":2000,"name":"Standard"},"fixedRatePeriod":60,"loanPurposes":[{"loanPurpose":{"id":202,"name":"koupě"},"sum":3000000},{"loanPurpose":{"id":203,"name":"výstavba"},"sum":2000000}],"loanDueDate":"2033-02-15T00:00:00","paymentDay":15,"cpm":"990614w","icp":""}


# ----------------------------------------------------------------------------------------------------------------------------------------------
# ROZCESTNIK
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/offer/mortgage/sales-arrangement/337
# {"offerId":829,"resourceProcessId":"127532f3-093f-4e28-abb4-ed409bd97d43","simulationInputs":{"productTypeId":20001,"loanKindId":2000,"loanAmount":5000000,"loanDuration":120,"fixedRatePeriod":60,"collateralAmount":1000000,"paymentDay":15,"isEmployeeBonusRequested":false,"expectedDateOfDrawing":"2023-03-01T00:00:00","statementTypeId":1,"interestRateDiscount":0,"drawingTypeId":2,"loanPurposes":[{"id":202,"sum":3000000},{"id":203,"sum":2000000}],"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":false,"incomeLoanRatioDiscount":false,"userVip":false},"developer":{"newDeveloperName":"","newDeveloperProjectName":"","newDeveloperCin":""},"fees":[],"riskLifeInsurance":{"sum":0,"frequency":1},"realEstateInsurance":{"sum":0,"frequency":12}},"simulationResults":{"loanAmount":5000000,"loanDueDate":"2033-02-15T00:00:00","loanDuration":120,"loanPaymentAmount":55235,"employeeBonusLoanCode":201,"loanToValue":100,"aprc":6.09,"loanTotalAmount":6639564.17,"loanInterestRateProvided":5.89,"contractSignedDate":"2023-03-01T00:00:00","drawingDateTo":"2023-03-14T00:00:00","annuityPaymentsDateFrom":"2023-04-15T00:00:00","annuityPaymentsCount":120,"loanInterestRate":5.89,"loanInterestRateAnnounced":5.89,"loanInterestRateAnnouncedType":1,"employeeBonusDeviation":0,"marketingActionsDeviation":0,"paymentDay":15,"loanPurposes":[{"id":202,"sum":3000000},{"id":203,"sum":2000000}],"marketingActions":[{"code":"DOMICILACE","requested":true,"applied":false,"marketingActionId":1,"deviation":0,"name":"Domicilace (přirážka)"},{"code":"RZP","requested":true,"applied":false,"marketingActionId":2,"deviation":0,"name":"Rizikové životní pojištění (přirážka)"},{"code":"POJIST_NEM","requested":false,"applied":false,"marketingActionId":4,"deviation":0,"name":"Pojištění nemovitosti (sleva)"},{"code":"VYSE_PRIJMU_UVERU","requested":false,"applied":false,"marketingActionId":3,"deviation":0,"name":"Výše příjmů nebo úvěru (sleva)"},{"code":"VIP_MAKLER","requested":false,"applied":false,"marketingActionId":20,"deviation":0,"name":"VIP makléř (sleva)"},{"code":"INDIVIDUALNI_SLEVA","requested":false,"applied":false,"marketingActionId":5,"deviation":0,"name":"Individuální sleva poradce"}],"paymentScheduleSimple":[{"paymentNumber":"1","date":"15.3.2023","type":"splátka úroků","amount":"11\u00A0452,78"},{"paymentNumber":"2 - 120","date":"vždy k 15. dni v měsíci","type":"splátka","amount":"55\u00A0235,00"},{"paymentNumber":"121","date":"15.3.2033","type":"splátka","amount":"55\u00A0146,39"}],"fees":[{"feeId":2001,"discountPercentage":0,"tariffSum":4900,"composedSum":0,"finalSum":4900,"marketingActionId":0,"name":"Zpracování žádosti","shortNameForExample":"Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad","tariffName":"Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad","usageText":"VSM","tariffTextWithAmount":"4\u00A0900 Kč","codeKB":"SB2001","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"},{"feeId":2002,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Čerpání úvěru","shortNameForExample":"","tariffName":"Čerpání úvěru","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE034","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2004,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Spravování úvěru (měsíčně)","shortNameForExample":"Spravování úvěru (měsíčně)","tariffName":"Spravování úvěru (měsíčně)","usageText":"VS","tariffTextWithAmount":"zdarma","codeKB":"FEE003","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2006,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Poplatek za papírový výpis (úvěr sazebník)","shortNameForExample":"","tariffName":"Služba zasílání výpisů z úvěrového účtu v papírové formě (měsíčně)","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE045","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2007,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Poplatek za elektronický výpis (úvěr sazebník)","shortNameForExample":"","tariffName":"Služba zasílání výpisů z úvěrového účtu v elektronické formě (měsíčně)","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE035","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2010,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Vedení BÚ ke splácení (do RPSN)","shortNameForExample":"Vedení běžného účtu (měsíčně)","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE009","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2012,"discountPercentage":0,"tariffSum":1000,"composedSum":0,"finalSum":1000,"marketingActionId":0,"name":"Zpráva o stavu výstavby (1-3)","shortNameForExample":"","tariffName":"Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)","usageText":"S","tariffTextWithAmount":"1\u00A0000 Kč první až třetí Zpráva,","codeKB":"FEE031","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2013,"discountPercentage":0,"tariffSum":2900,"composedSum":0,"finalSum":2900,"marketingActionId":0,"name":"Zpráva o stavu výstavby (4 a více)","shortNameForExample":"","tariffName":"Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)","usageText":"S","tariffTextWithAmount":"2\u00A0900 Kč čtvrtá a každá další Zpráva. ","codeKB":"FEE084","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2015,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Výpis z BU ke splácení","shortNameForExample":"Výpis z běžného účtu (měsíčně)","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE036","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2016,"discountPercentage":0,"tariffSum":2000,"composedSum":0,"finalSum":2000,"marketingActionId":0,"name":"Poplatek za vklad do KN","shortNameForExample":"Návrh na vklad zástavního práva k nemovitostem do katastru nemovitostí","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE037","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"},{"feeId":2017,"discountPercentage":0,"tariffSum":2000,"composedSum":0,"finalSum":2000,"marketingActionId":0,"name":"Poplatek za výmaz z KN","shortNameForExample":"Návrh na výmaz zástavního práva k nemovitostem z katastru nemovitostí","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE079","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"C"},{"feeId":2032,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Změna smlouvy GREC","shortNameForExample":"Změna ve smlouvě","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE078","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"}],"warnings":[{"internalMessage":"Warning : CIS_ALT_KANALY_SPECIFIKA::BLOKACIA=0 - Překročena maximální výše úvěru pro kanál.","text":"CIS_WS_ERROR"},{"internalMessage":"Warning : CIS_ALT_KANALY_SPECIFIKA::BLOKACIA=0 - Nepovolené zatížení nemovitosti pro kanál.","text":"CIS_WS_ERROR"},{"internalMessage":"Warning : Mimo produktovy rozsah (CIS_HYPOTEKY_PRODUKTY::MAX_VYSKA_LTV)","text":"CIS_WS_ERROR"}]}}

# GET https://fat.noby.cz/api/sales-arrangement/337
# {"productTypeId":20001,"salesArrangementId":337,"salesArrangementTypeId":1,"loanApplicationAssessmentId":"","createdTime":"2023-02-22T10:59:54.86","createdBy":"Filip Tůma","offerGuaranteeDateFrom":"2023-02-22T00:00:00","offerGuaranteeDateTo":"2023-04-08T00:00:00","parameters":{"incomeCurrencyCode":"CAD","residencyCurrencyCode":"CZK","contractSignatureTypeId":2,"loanRealEstates":[{"realEstateTypeId":1,"isCollateral":true,"realEstatePurchaseTypeId":1},{"realEstateTypeId":5,"isCollateral":false,"realEstatePurchaseTypeId":2}],"agent":534,"agentConsentWithElCom":false}}

# GET https://fat.noby.cz/api/case/3013351
# {"caseOwner":{"cpm":"990614w","icp":""},"caseId":3013351,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00","state":1,"stateName":"Příprava žádosti","contractNumber":"","targetAmount":5000000,"productName":"Hypoteční úvěr","createdTime":"2023-02-22T10:59:54.373","createdBy":"Filip Tůma","stateUpdated":"2023-02-22T10:59:54.37","emailForOffer":"novak@testcm.cz","phoneNumberForOffer":"+420 777543234"}


# ----------------------------------------------------------------------------------------------------------------------------------------------
# DOMACNOSTI
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/household/list/337
# [{"householdId":437,"householdTypeId":1,"householdTypeName":"Hlavní"},{"householdId":438,"householdTypeId":2,"householdTypeName":"Spoludlužnická"},{"householdId":439,"householdTypeId":128,"householdTypeName":"Ručitelská"}]

# GET https://fat.noby.cz/api/sales-arrangement/337/customers
# [{"id":534,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00","customerRoleId":1},{"id":536,"firstName":"","lastName":"","customerRoleId":128},{"id":537,"firstName":"JANA","lastName":"Nováková","customerRoleId":2}]

# GET https://fat.noby.cz/api/household/437
# {"householdId":437,"areCustomersPartners":false,"data":{"childrenUpToTenYearsCount":1,"childrenOverTenYearsCount":2},"expenses":{"savingExpenseAmount":10000,"insuranceExpenseAmount":3000,"housingExpenseAmount":10000,"otherExpenseAmount":7500},"customer1":{"incomes":[{"incomeId":147,"incomeSource":"ABC","hasProofOfIncome":true,"incomeTypeId":1,"sum":25000,"currencyCode":"CZK"},{"incomeId":148,"incomeSource":"-","hasProofOfIncome":false,"incomeTypeId":2,"sum":1000000,"currencyCode":"CZK"},{"incomeId":149,"incomeSource":"-","hasProofOfIncome":false,"incomeTypeId":3,"sum":80000,"currencyCode":"CZK"},{"incomeId":150,"incomeSource":"cestovní náhrady (diety) - měsíční","hasProofOfIncome":false,"incomeTypeId":4,"sum":30000,"currencyCode":"CZK"}],"roleId":1,"identities":[],"obligations":[],"customerOnSAId":534,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00"},"customer2":{"incomes":[],"roleId":2,"identities":[],"obligations":[],"customerOnSAId":537,"firstName":"JANA","lastName":"Nováková"}}

# GET https://fat.noby.cz/api/household/438
# {"householdId":438,"areCustomersPartners":false,"data":{"childrenUpToTenYearsCount":0,"childrenOverTenYearsCount":0},"expenses":{},"customer1":{"incomes":[{"incomeId":161,"incomeSource":"kkkk","hasProofOfIncome":false,"incomeTypeId":1,"sum":45000,"currencyCode":"CZK"}],"roleId":128,"identities":[],"obligations":[],"customerOnSAId":536,"firstName":"","lastName":""}}

# GET https://fat.noby.cz/api/household/439
# {"householdId":439,"areCustomersPartners":false,"data":{"childrenUpToTenYearsCount":0,"childrenOverTenYearsCount":0},"expenses":{},"customer1":{"incomes":[{"incomeId":161,"incomeSource":"kkkk","hasProofOfIncome":false,"incomeTypeId":1,"sum":45000,"currencyCode":"CZK"}],"roleId":128,"identities":[],"obligations":[],"customerOnSAId":536,"firstName":"","lastName":""}}


# ----------------------------------------------------------------------------------------------------------------------------------------------
# PARAMETRY
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/offer/mortgage/sales-arrangement/337
# {"offerId":829,"resourceProcessId":"127532f3-093f-4e28-abb4-ed409bd97d43","simulationInputs":{"productTypeId":20001,"loanKindId":2000,"loanAmount":5000000,"loanDuration":120,"fixedRatePeriod":60,"collateralAmount":1000000,"paymentDay":15,"isEmployeeBonusRequested":false,"expectedDateOfDrawing":"2023-03-01T00:00:00","statementTypeId":1,"interestRateDiscount":0,"drawingTypeId":2,"loanPurposes":[{"id":202,"sum":3000000},{"id":203,"sum":2000000}],"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":false,"incomeLoanRatioDiscount":false,"userVip":false},"developer":{"newDeveloperName":"","newDeveloperProjectName":"","newDeveloperCin":""},"fees":[],"riskLifeInsurance":{"sum":0,"frequency":1},"realEstateInsurance":{"sum":0,"frequency":12}},"simulationResults":{"loanAmount":5000000,"loanDueDate":"2033-02-15T00:00:00","loanDuration":120,"loanPaymentAmount":55235,"employeeBonusLoanCode":201,"loanToValue":100,"aprc":6.09,"loanTotalAmount":6639564.17,"loanInterestRateProvided":5.89,"contractSignedDate":"2023-03-01T00:00:00","drawingDateTo":"2023-03-14T00:00:00","annuityPaymentsDateFrom":"2023-04-15T00:00:00","annuityPaymentsCount":120,"loanInterestRate":5.89,"loanInterestRateAnnounced":5.89,"loanInterestRateAnnouncedType":1,"employeeBonusDeviation":0,"marketingActionsDeviation":0,"paymentDay":15,"loanPurposes":[{"id":202,"sum":3000000},{"id":203,"sum":2000000}],"marketingActions":[{"code":"DOMICILACE","requested":true,"applied":false,"marketingActionId":1,"deviation":0,"name":"Domicilace (přirážka)"},{"code":"RZP","requested":true,"applied":false,"marketingActionId":2,"deviation":0,"name":"Rizikové životní pojištění (přirážka)"},{"code":"POJIST_NEM","requested":false,"applied":false,"marketingActionId":4,"deviation":0,"name":"Pojištění nemovitosti (sleva)"},{"code":"VYSE_PRIJMU_UVERU","requested":false,"applied":false,"marketingActionId":3,"deviation":0,"name":"Výše příjmů nebo úvěru (sleva)"},{"code":"VIP_MAKLER","requested":false,"applied":false,"marketingActionId":20,"deviation":0,"name":"VIP makléř (sleva)"},{"code":"INDIVIDUALNI_SLEVA","requested":false,"applied":false,"marketingActionId":5,"deviation":0,"name":"Individuální sleva poradce"}],"paymentScheduleSimple":[{"paymentNumber":"1","date":"15.3.2023","type":"splátka úroků","amount":"11\u00A0452,78"},{"paymentNumber":"2 - 120","date":"vždy k 15. dni v měsíci","type":"splátka","amount":"55\u00A0235,00"},{"paymentNumber":"121","date":"15.3.2033","type":"splátka","amount":"55\u00A0146,39"}],"fees":[{"feeId":2001,"discountPercentage":0,"tariffSum":4900,"composedSum":0,"finalSum":4900,"marketingActionId":0,"name":"Zpracování žádosti","shortNameForExample":"Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad","tariffName":"Zpracování a vyhodnocení žádosti o úvěr, ocenění, čerpání na návrh na vklad","usageText":"VSM","tariffTextWithAmount":"4\u00A0900 Kč","codeKB":"SB2001","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"},{"feeId":2002,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Čerpání úvěru","shortNameForExample":"","tariffName":"Čerpání úvěru","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE034","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2004,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Spravování úvěru (měsíčně)","shortNameForExample":"Spravování úvěru (měsíčně)","tariffName":"Spravování úvěru (měsíčně)","usageText":"VS","tariffTextWithAmount":"zdarma","codeKB":"FEE003","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2006,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Poplatek za papírový výpis (úvěr sazebník)","shortNameForExample":"","tariffName":"Služba zasílání výpisů z úvěrového účtu v papírové formě (měsíčně)","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE045","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2007,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Poplatek za elektronický výpis (úvěr sazebník)","shortNameForExample":"","tariffName":"Služba zasílání výpisů z úvěrového účtu v elektronické formě (měsíčně)","usageText":"S","tariffTextWithAmount":"zdarma","codeKB":"FEE035","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2010,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Vedení BÚ ke splácení (do RPSN)","shortNameForExample":"Vedení běžného účtu (měsíčně)","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE009","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2012,"discountPercentage":0,"tariffSum":1000,"composedSum":0,"finalSum":1000,"marketingActionId":0,"name":"Zpráva o stavu výstavby (1-3)","shortNameForExample":"","tariffName":"Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)","usageText":"S","tariffTextWithAmount":"1\u00A0000 Kč první až třetí Zpráva,","codeKB":"FEE031","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2013,"discountPercentage":0,"tariffSum":2900,"composedSum":0,"finalSum":2900,"marketingActionId":0,"name":"Zpráva o stavu výstavby (4 a více)","shortNameForExample":"","tariffName":"Vyhodnocení rizik spojených s vypracováním Zprávy o stavu výstavby a rekonstrukce (Zpráva)","usageText":"S","tariffTextWithAmount":"2\u00A0900 Kč čtvrtá a každá další Zpráva. ","codeKB":"FEE084","displayAsFreeOfCharge":true,"includeInRPSN":false,"periodicity":"E"},{"feeId":2015,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Výpis z BU ke splácení","shortNameForExample":"Výpis z běžného účtu (měsíčně)","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE036","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"M"},{"feeId":2016,"discountPercentage":0,"tariffSum":2000,"composedSum":0,"finalSum":2000,"marketingActionId":0,"name":"Poplatek za vklad do KN","shortNameForExample":"Návrh na vklad zástavního práva k nemovitostem do katastru nemovitostí","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE037","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"},{"feeId":2017,"discountPercentage":0,"tariffSum":2000,"composedSum":0,"finalSum":2000,"marketingActionId":0,"name":"Poplatek za výmaz z KN","shortNameForExample":"Návrh na výmaz zástavního práva k nemovitostem z katastru nemovitostí","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE079","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"C"},{"feeId":2032,"discountPercentage":0,"tariffSum":0,"composedSum":0,"finalSum":0,"marketingActionId":0,"name":"Změna smlouvy GREC","shortNameForExample":"Změna ve smlouvě","tariffName":"","usageText":"V","tariffTextWithAmount":"","codeKB":"FEE078","displayAsFreeOfCharge":true,"includeInRPSN":true,"periodicity":"I"}],"warnings":[{"internalMessage":"Warning : CIS_ALT_KANALY_SPECIFIKA::BLOKACIA=0 - Překročena maximální výše úvěru pro kanál.","text":"CIS_WS_ERROR"},{"internalMessage":"Warning : CIS_ALT_KANALY_SPECIFIKA::BLOKACIA=0 - Nepovolené zatížení nemovitosti pro kanál.","text":"CIS_WS_ERROR"},{"internalMessage":"Warning : Mimo produktovy rozsah (CIS_HYPOTEKY_PRODUKTY::MAX_VYSKA_LTV)","text":"CIS_WS_ERROR"}]}}

# GET https://fat.noby.cz/api/sales-arrangement/337
# {"productTypeId":20001,"salesArrangementId":337,"salesArrangementTypeId":1,"loanApplicationAssessmentId":"","createdTime":"2023-02-22T10:59:54.86","createdBy":"Filip Tůma","offerGuaranteeDateFrom":"2023-02-22T00:00:00","offerGuaranteeDateTo":"2023-04-08T00:00:00","parameters":{"incomeCurrencyCode":"CAD","residencyCurrencyCode":"CZK","contractSignatureTypeId":2,"loanRealEstates":[{"realEstateTypeId":1,"isCollateral":true,"realEstatePurchaseTypeId":1},{"realEstateTypeId":5,"isCollateral":false,"realEstatePurchaseTypeId":2}],"agent":534,"agentConsentWithElCom":false}}

# GET https://fat.noby.cz/api/sales-arrangement/337/customers
# [{"id":534,"firstName":"JAN","lastName":"Novák","dateOfBirth":"1980-01-01T00:00:00","customerRoleId":1},{"id":536,"firstName":"","lastName":"","customerRoleId":128},{"id":537,"firstName":"JANA","lastName":"Nováková","customerRoleId":2}]


# ----------------------------------------------------------------------------------------------------------------------------------------------
# SCORING
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/sales-arrangement/337/loan-application-assessment?newAssessmentRequired=true
# {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"SalesArrangement.RiskBusinessCaseId not defined.","status":400}


# ----------------------------------------------------------------------------------------------------------------------------------------------