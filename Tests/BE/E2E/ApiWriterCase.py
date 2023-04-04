from typing import List, Any
import jsonpath_ng

from .EProcessKey import EProcessKey
from .Processing import Processing

from business.case import Case, Household, Expenses

from business.codebooks import EHouseholdType
from fe_api import FeAPI
from common import Log, DictExtensions
from .workflow.WorkflowStep import WorkflowStep
from .workflow.EWorkflowEntity import EWorkflowEntity
from .workflow.EWorkflowType import EWorkflowType

class ApiWriterCase():

    def __init__(self, case: Case, offer_id: int, handle_create_snapshot = None):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')
        self.__handle_create_snapshot = handle_create_snapshot

        self.__case = case
        self.__case_json = case.to_json_value()
        self.__offer_id = offer_id

        self.__WORKFLOW_DISPATCHES = {
            f'{EWorkflowEntity.CASE_HOUSEHOLDS.name}_{EWorkflowType.ADD.name}': lambda step: self.__add_household(workflow_step=step),
            f'{EWorkflowEntity.CASE_HOUSEHOLDS.name}_{EWorkflowType.EDIT.name}': lambda step: self.__edit_household(workflow_step=step),
            f'{EWorkflowEntity.CASE_HOUSEHOLDS.name}_{EWorkflowType.REMOVE.name}': lambda step: self.__remove_household(workflow_step=step),
        }

    def __create_snapshot(self, order: int, workflow_step: WorkflowStep = None):

        if self.__handle_create_snapshot is None:
            return

        case_id = Processing.get_process_key(self.__case_json, EProcessKey.CASE_ID)

        self.__handle_create_snapshot(case_id, order, workflow_step)


    def build(self) -> int | Exception:
        try:
            self.__create_case()
            self.__create_snapshot(0)

        except Exception as e:
            return e

        return Processing.get_process_key(self.__case_json, EProcessKey.CASE_ID)

    def process_workflow_step(self, order: int, workflow_step: WorkflowStep):
        workflow_key: str = f'{workflow_step.entity.name}_{workflow_step.type.name}'
        assert workflow_key in self.__WORKFLOW_DISPATCHES.keys(), f'Workflow is not supported [entity: {workflow_step.entity.name}, type: {workflow_step.type.name}]'
        workflow_fce = self.__WORKFLOW_DISPATCHES[workflow_key]
        workflow_fce(workflow_step)
        self.__create_snapshot(order, workflow_step)

    def __add_household(self, workflow_step: WorkflowStep):

        # ------------------------------------------------------------------------
        # create_household
        # ------------------------------------------------------------------------

        # get process data
        sales_arrangement_id = Processing.get_process_key(self.__case_json, EProcessKey.SALES_ARRAMGEMENT_ID)
        household_type_id = Processing.get_key(workflow_step.data, 'householdTypeId')

        req = dict(salesArrangementId = sales_arrangement_id, householdTypeId =  household_type_id)
        res = FeAPI.Household.create_household(req)
        
        householdId = Processing.get_key(res, 'householdId')
        householdTypeId = Processing.get_key(res, 'householdTypeId')
        household = Household.from_json(dict(
            householdId = householdId,
            householdTypeId = householdTypeId,
            childrenUpToTenYearsCount = 0,
            childrenOverTenYearsCount = 0,
            areCustomersPartners = False,
            expenses = Expenses.get_default().to_json_value()
        ))

        json_households: list = Processing.get_key(self.__case_json, 'households')
        json_household = household.to_json_value()
        json_households.append(json_household)

        # ------------------------------------------------------------------------
        # set_household_parameters
        # ------------------------------------------------------------------------

        # build data by current state
        data = dict(
            childrenUpToTenYearsCount = Processing.get_key(json_household, 'childrenUpToTenYearsCount'),
            childrenOverTenYearsCount = Processing.get_key(json_household, 'childrenOverTenYearsCount'),
            areCustomersPartners = Processing.get_key(json_household, 'areCustomersPartners'),
            expenses = Processing.get_key(json_household, 'expenses')
        )

        # modify data by workflow data
        data = DictExtensions.modify_json(data, workflow_step.data)

        # build request from data
        req = dict(
                data = dict(
                    childrenUpToTenYearsCount = Processing.get_key(data, 'childrenUpToTenYearsCount'),
                    childrenOverTenYearsCount = Processing.get_key(data, 'childrenOverTenYearsCount'),
                    areCustomersPartners = Processing.get_key(data, 'areCustomersPartners'),
                ),
                expenses = Processing.get_key(data, 'expenses')
            )

        # call FE API endpoint
        res = FeAPI.Household.set_household_parameters(householdId, req)

        # upgrade current state by workflow data
        DictExtensions.modify_json(json_household, data)

    def __edit_household(self, workflow_step: WorkflowStep):
        json_household: dict = self.__find_json_by_path(workflow_step.path)

        householdId: int = Processing.get_process_key(json_household, EProcessKey.HOUSEHOLD_ID)
        householdId =  Processing.get_key(json_household, 'householdId') if householdId is None else householdId     

        # build data by current state
        data = dict(
            childrenUpToTenYearsCount = Processing.get_key(json_household, 'childrenUpToTenYearsCount'),
            childrenOverTenYearsCount = Processing.get_key(json_household, 'childrenOverTenYearsCount'),
            areCustomersPartners = Processing.get_key(json_household, 'areCustomersPartners'),
            expenses = Processing.get_key(json_household, 'expenses')
        )

        # modify data by workflow data
        data = DictExtensions.modify_json(data, workflow_step.data)

        # build request from data
        req = dict(
                data = dict(
                    childrenUpToTenYearsCount = Processing.get_key(data, 'childrenUpToTenYearsCount'),
                    childrenOverTenYearsCount = Processing.get_key(data, 'childrenOverTenYearsCount'),
                    areCustomersPartners = Processing.get_key(data, 'areCustomersPartners'),
                ),
                expenses = Processing.get_key(data, 'expenses')
            )

        # call FE API endpoint
        res = FeAPI.Household.set_household_parameters(householdId, req)

        # upgrade current state by workflow data
        DictExtensions.modify_json(json_household, workflow_step.data)

    def __remove_household(self, workflow_step: WorkflowStep):
        json_household = self.__find_json_by_path(workflow_step.path)
        householdId = Processing.get_key(json_household, 'householdId')
        # call FE API endpoint
        res = FeAPI.Household.delete_household(householdId)
        json_households = self.__find_json_by_path(workflow_step.path, True)
        json_households.remove(json_household)

    def __to_path_without_indexer(self, path: str) -> str:
        if not path.endswith(']'):
            return path
        index_bracket_open = path.rindex('[')
        return path[:index_bracket_open]

    def __find_json_by_path(self, path: str, remove_indexer: bool = False) -> Any:
        if remove_indexer:
            path = self.__to_path_without_indexer(path)

        prefix: str = 'case.' 
        if path.startswith(prefix):
            path = path[len(prefix):]

        jsonpath_expr = jsonpath_ng.parse(path)
        matches = jsonpath_expr.find(self.__case_json)
        matches_count = len(matches)
        assert matches_count == 1
        value_current = matches[0].value
        return value_current
            


    def __create_case(self):

        # get process data
        offer_id = self.__offer_id
        
        household_json = list(filter(lambda i: int(i['householdTypeId']) == EHouseholdType.Main.value, self.__case_json['households']))[0]
        customer_json = Processing.get_key(household_json,'customer1')

        req = dict(
            offerId = offer_id,
            firstName = Processing.get_key(customer_json, 'firstName', ''),
            lastName = Processing.get_key(customer_json, 'lastName', ''),
            dateOfBirth = Processing.get_key(customer_json, 'dateOfBirth'),
            phoneNumberForOffer = Processing.get_key(self.__case_json, 'phoneNumberForOffer'),
            emailForOffer = Processing.get_key(self.__case_json, 'emailForOffer'),
            identity = Processing.get_key(customer_json, 'identity'),
        )

        # call FE API endpoint
        res = FeAPI.Offer.create_case(req)
        assert isinstance(res, dict), f'Invalid response [{res}]'

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
                res = FeAPI.Household.create_household(req)

                assert isinstance(res, dict), f'Invalid response [{res}]'

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
            res = FeAPI.Household.set_household_parameters(household_id, req)
            
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
                # roleId = 1, #TODO: remove from source JSON !?
                firstName = Processing.get_key(customer_json, 'firstName', ''),
                lastName = Processing.get_key(customer_json, 'lastName', ''),
                dateOfBirth = Processing.get_key(customer_json, 'dateOfBirth', ''),
                identities = [] if identity is None else [identity],
                incomes = [],
                obligations = [],
            )

            return customer

        req = dict(
            householdId = household_id,
            customer1 = get_req_customer(household_json['customer1']),
            customer2 = get_req_customer(household_json['customer2']),
        )

        # call FE API endpoint
        res = FeAPI.Household.set_household_customers(household_id, req)
        assert isinstance(res, dict), f'Invalid response [{res}]'

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
            res = FeAPI.CustomerOnSa.create_income(customer_on_sa_id, req)

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
            res = FeAPI.CustomerOnSa.create_obligation(customer_on_sa_id, req)

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
        res = FeAPI.SalesArrangement.set_parameters(sales_arrangement_id, req)
