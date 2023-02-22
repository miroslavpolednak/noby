# ----------------------------
import _setup
# ----------------------------

# https://fat.noby.cz/undefined#/

from typing import List

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI

from business.offer import Offer
from business.case import Case

from tests.tuning.OptionsResolver import OptionsResolver
from tests.tuning.data import load_file_json


def run_via_processor():
    from E2E import ApiProcessor
    case_inputs_json: dict = load_file_json()
    case = Case.from_json(case_inputs_json)
    
    results = ApiProcessor.process_list([case])
    for r in results:
        print(r)


run_via_processor()

quit()



# --------------------------------------------------------------------------------------------------------------
# Processing data helpers
# --------------------------------------------------------------------------------------------------------------

KEY_TEST_DATA = 'TEST_DATA'
KEY_OFFER_ID = 'KEY_OFFER_ID'
KEY_CASE_ID = 'CASE_ID'
KEY_SALES_ARRAMGEMENT_ID = 'SALES_ARRAMGEMENT_ID'
KEY_HOUSEHOLD_ID = 'HOUSEHOLD_ID'
KEY_CUSTOMER_ON_SA_ID = 'CUSTOMER_ON_SA_ID'
KEY_INCOME_ID = 'INCOME_ID'
KEY_OBLIGATION_ID = 'OBLIGATION_ID'

def initKey(entity_json: dict, key: str, value = dict()):
    if entity_json is None:
        return entity_json

    if key not in entity_json:
        entity_json[key] = value

    if entity_json[key] is None:
        entity_json[key] = value

    return entity_json[key]

def getKey(entity_json: dict, key: str, default = None):
    if entity_json is None:
        return None

    if key not in entity_json:
        return default

    if entity_json[key] is None:
        return default

    return entity_json[key]

def setTestKey(entity_json: dict, key: str, value: object ) -> dict:
    if entity_json is None:
        return entity_json

    if KEY_TEST_DATA not in entity_json:
        entity_json[KEY_TEST_DATA] = dict()
    
    entity_json[KEY_TEST_DATA][key] = value

    return entity_json

def getTestKey(entity_json: dict, key: str) -> object:
    if entity_json is None:
        return entity_json

    if KEY_TEST_DATA not in entity_json:
        return None

    if key not in entity_json[KEY_TEST_DATA]:
        return None
    
    return entity_json[KEY_TEST_DATA][key]

# --------------------------------------------------------------------------------------------------------------

test_data: dict = load_file_json()
#print(test_data)

# js_dict = test_data['offer']

# #print(','.join(list(map(lambda k: f"'{k}'", list(js_dict.keys())))) )

# offer = Offer.from_json(js_dict)
# print(offer)
# print(offer.to_grpc())
# print(Offer.to_grpc(offer))

case = Case.from_json(test_data)
print(case)

case_json: dict = case.to_json_value()
offer_json: dict = getKey(case_json, 'offer')

# --------------------------------------------------------------------------------------------------------------
# FeAPI - process OFFER
# --------------------------------------------------------------------------------------------------------------
req = offer_json

# call FE API endpoint 
print(f'process_offer.req [{req}]')
res = FeAPI.Offer.simulate_mortgage(req)
print(f'process_offer.res [{res}]')

# set test data
setTestKey(offer_json, KEY_OFFER_ID, res['offerId'])

# response serialization & persistence
#res_str = json.dumps(res)

# --------------------------------------------------------------------------------------------------------------
# FeAPI - process CASE
# --------------------------------------------------------------------------------------------------------------

#https://fat.noby.cz/api/customer-on-sa/291/income

def create_incomes(customer_json: dict):

    incomes_json = getKey(customer_json, 'incomes')

    if incomes_json is None:
        return

    customer_on_sa_id: int = getTestKey(customer_json, KEY_CUSTOMER_ON_SA_ID)

    def get_req_data(income_json: dict) -> dict:
        
        data_json = getKey(income_json, 'data')

        if (data_json is None):
            return None

        # employer
        employer: dict = None
        employer_json = getKey(data_json, 'employer')
        if employer_json is not None:
            employer = dict(
                name = getKey(employer_json, 'name'),
                cin = getKey(employer_json, 'cin'),
                birthNumber = getKey(employer_json, 'birthNumber'),
                countryId = getKey(employer_json, 'countryId'),
            )

        # job
        job: dict = None
        job_json = getKey(data_json, 'job')
        if job_json is not None:
            job = dict(
                jobDescription = getKey(job_json, 'jobDescription'),
                firstWorkContractSince = getKey(job_json, 'firstWorkContractSince'),
                employmentTypeId = getKey(job_json, 'employmentTypeId'),
                currentWorkContractSince = getKey(job_json, 'currentWorkContractSince'),
                currentWorkContractTo = getKey(job_json, 'currentWorkContractTo'),
                grossAnnualIncome = getKey(job_json, 'grossAnnualIncome'),
                isInTrialPeriod = getKey(job_json, 'isInTrialPeriod'),
                isInProbationaryPeriod = getKey(job_json, 'isInProbationaryPeriod'),
            )
        
        # wageDeduction
        wage_deduction: dict = None
        wage_deduction_json = getKey(data_json, 'wageDeduction')
        if wage_deduction_json is not None:
            wage_deduction = dict(
                deductionDecision = getKey(wage_deduction_json, 'deductionDecision'),
                deductionPayments = getKey(wage_deduction_json, 'deductionPayments'),
                deductionOther = getKey(wage_deduction_json, 'deductionOther'),
            )

        # incomeConfirmation
        income_confirmation: dict = None
        income_confirmation_json = getKey(data_json, 'incomeConfirmation')
        if wage_deduction_json is not None:
            income_confirmation = dict(
                confirmationDate = getKey(income_confirmation_json, 'confirmationDate'),
                confirmationPerson = getKey(income_confirmation_json, 'confirmationPerson'),
                confirmationContact = getKey(income_confirmation_json, 'confirmationContact'),
                isIssuedByExternalAccountant = getKey(income_confirmation_json, 'isIssuedByExternalAccountant'),
            )

        # data
        return dict(
            hasProofOfIncome = getKey(data_json, 'hasProofOfIncome'),
            foreignIncomeTypeId = getKey(data_json, 'foreignIncomeTypeId'),
            hasWageDeduction = getKey(data_json, 'hasWageDeduction'),
            cin = getKey(data_json, 'cin'),
            birthNumber = getKey(data_json, 'birthNumber'),
            countryOfResidenceId = getKey(data_json, 'countryOfResidenceId'),
            incomeOtherTypeId = getKey(data_json, 'incomeOtherTypeId'),

            employer = employer,
            job = job,
            wageDeduction = wage_deduction,
            incomeConfirmation = income_confirmation,
        )

    # create incomes
    for income_json in incomes_json:
        req = dict(
            sum = income_json['sum'],
            currencyCode = getKey(income_json, 'currencyCode'),
            incomeTypeId = getKey(income_json, 'incomeTypeId'),
            data = get_req_data(income_json)
        )

        print(f'create_incomes.req [{req}]')
        res = FeAPI.CustomerOnSa.create_income(customer_on_sa_id, req)
        print(f'create_incomes.res [{res}]')

        setTestKey(income_json, KEY_INCOME_ID, res)


def create_obligations(customer_json: dict):

    obligations_json = getKey(customer_json, 'obligations')

    if obligations_json is None:
        return

    customer_on_sa_id: int = getTestKey(customer_json, KEY_CUSTOMER_ON_SA_ID)

    # create obligations
    for obligation_json in obligations_json:

        creditor: dict = None
        if 'creditor' in obligation_json:
            creditor_json = obligation_json['creditor']
            creditor = dict(
                creditorId = getKey(creditor_json, 'creditorId'),
                name = getKey(creditor_json, 'name'),
                isExternal = getKey(creditor_json, 'isExternal'),
            )

        correction: dict = None
        if 'correction' in obligation_json:
            correction_json = obligation_json['correction']
            correction = dict(
                correctionTypeId = getKey(correction_json, 'correctionTypeId'),
                installmentAmountCorrection = getKey(correction_json, 'installmentAmountCorrection'),
                loanPrincipalAmountCorrection = getKey(correction_json, 'loanPrincipalAmountCorrection'),
                creditCardLimitCorrection = getKey(correction_json, 'creditCardLimitCorrection'),
            )

        req = dict(
            obligationTypeId = getKey(obligation_json, 'obligationTypeId'),
            obligationState = getKey(obligation_json, 'obligationState'),
            installmentAmount = getKey(obligation_json, 'installmentAmount'),
            loanPrincipalAmount = getKey(obligation_json, 'loanPrincipalAmount'),
            creditCardLimit = getKey(obligation_json, 'creditCardLimit'),
            amountConsolidated = getKey(obligation_json, 'amountConsolidated'),
            
            creditor = creditor,
            correction = correction,
        )

        print(f'create_obligations.req [{req}]')
        res = FeAPI.CustomerOnSa.create_obligation(customer_on_sa_id, req)
        print(f'create_obligations.res [{res}]')

        setTestKey(obligation_json, KEY_OBLIGATION_ID, res)

def create_customers(household_json: dict):
    household_id: int = getTestKey(household_json, KEY_HOUSEHOLD_ID)

    def get_req_customer(customer_json: dict) -> dict:

        if (customer_json is None):
            return None

        customer_on_sa_id: int = getTestKey(customer_json, KEY_CUSTOMER_ON_SA_ID)
        identity: dict = getKey(customer_json, 'identity', None)

        customer = dict(
            customerOnSAId = customer_on_sa_id,
            firstName = getKey(customer_json, 'firstName', ''),
            lastName = getKey(customer_json, 'lastName', ''),
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

    print(f'create_customers.req [{req}]')
    res = FeAPI.Household.set_household_customers(household_id, req)
    print(f'create_customers.res [{res}]')

    # set test data (ids of customers on SA)
    setTestKey(household_json['customer1'], KEY_CUSTOMER_ON_SA_ID, res['customerOnSAId1'])
    if household_json['customer2'] is not None:
        setTestKey(household_json['customer2'], KEY_CUSTOMER_ON_SA_ID, res['customerOnSAId2'])

    # create incomes & obligations
    create_incomes(household_json['customer1'])
    create_obligations(household_json['customer1'])
    if household_json['customer2'] is not None:
        create_incomes(household_json['customer2'])
        create_obligations(household_json['customer2'])

def create_households(case_json: dict):

    sales_arrangement_id = getTestKey(case_json, KEY_SALES_ARRAMGEMENT_ID)

    household_by_type: dict = dict()

    # create households
    for household_json in case_json['households']:
        household_id: int = getTestKey(household_json, KEY_HOUSEHOLD_ID)
        household_type_id = household_json['householdTypeId']

        household_by_type[household_type_id] = household_json

        if household_id is None:
            req = dict(
                salesArrangementId = sales_arrangement_id,
                householdTypeId = household_type_id
            )

            # call FE API endpoint
            print(f'create_households.req [{req}]')
            res = FeAPI.Household.create_household(req)
            print(f'create_households.res [{res}]')

            household_id = res['householdId']
            setTestKey(household_json, KEY_HOUSEHOLD_ID, household_id)

        expenses_json = getKey(household_json, 'expenses')
        req = dict(
            data = dict(
                childrenUpToTenYearsCount = getKey(household_json, 'childrenUpToTenYearsCount', 0),
                childrenOverTenYearsCount = getKey(household_json, 'childrenOverTenYearsCount', 0),
                areCustomersPartners = getKey(household_json, 'areCustomersPartners', False),
            ),
            expenses = dict(
                savingExpenseAmount = getKey(expenses_json, 'savingExpenseAmount', None),
                insuranceExpenseAmount = getKey(expenses_json, 'insuranceExpenseAmount', None),
                housingExpenseAmount = getKey(expenses_json, 'housingExpenseAmount', None),
                otherExpenseAmount = getKey(expenses_json, 'otherExpenseAmount', None),
            )
        )

        # call FE API endpoint
        print(f'create_households_parameters.req [{req}]')
        res = FeAPI.Household.set_household_parameters(household_id, req)
        print(f'create_households_parameters.res [{res}]')
        
    # set test data (ids of customers)
    customers = FeAPI.SalesArrangement.get_customers(sales_arrangement_id)
    for c in customers:
        customer_on_sa_id = c['id']
        household_type_id = c['customerRoleId']
        household_json = household_by_type[household_type_id]
        household_customer_json = initKey(household_json,'customer1')
        setTestKey(household_customer_json, KEY_CUSTOMER_ON_SA_ID, customer_on_sa_id)

    # create customers for households
    for household_json in case_json['households']:
        create_customers(household_json)

def create_parameters(case_json: dict):

    parameters_json = getKey(case_json, 'parameters')

    if parameters_json is None:
        return

    sales_arrangement_id = getTestKey(case_json, KEY_SALES_ARRAMGEMENT_ID)

    def map_loan_real_estate(loan_real_estate_json: dict)-> dict:
        return dict(
            realEstateTypeId = getKey(loan_real_estate_json, 'realEstateTypeId'),
            isCollateral = getKey(loan_real_estate_json, 'isCollateral'),
            realEstatePurchaseTypeId = getKey(loan_real_estate_json, 'realEstatePurchaseTypeId'),
        )

    loan_real_estates_json = getKey(parameters_json, 'loanRealEstates', [])
    loan_real_estates = list(map(lambda i_json: map_loan_real_estate(i_json), loan_real_estates_json))

    req = dict(
        parameters = dict(
            incomeCurrencyCode = getKey(parameters_json, 'incomeCurrencyCode'),
            residencyCurrencyCode = getKey(parameters_json, 'residencyCurrencyCode'),
            contractSignatureTypeId = getKey(parameters_json, 'contractSignatureTypeId'),
            agent = getKey(parameters_json, 'agent'),
            agentConsentWithElCom = getKey(parameters_json, 'agentConsentWithElCom'),
            loanRealEstates = loan_real_estates,
        )
    )

    # call FE API endpoint
    print(f'create_parameters.req [{req}]')
    res = FeAPI.SalesArrangement.set_parameters(sales_arrangement_id, req)
    print(f'create_parameters.res [{res}]')


def create_case(case_json: dict, offer_id: int):
    
    household_json = list(filter(lambda i: int(i['householdTypeId']) == EHouseholdType.Main.value, case_json['households']))[0]
    customer_json = getKey(household_json,'customer1')

    req = dict(
        offerId = offer_id,
        firstName = getKey(customer_json, 'firstName', ''),
        lastName = getKey(customer_json, 'lastName', ''),
        dateOfBirth = getKey(customer_json, 'dateOfBirth'),
        phoneNumberForOffer = getKey(customer_json, 'phoneNumberForOffer'),
        emailForOffer = getKey(customer_json, 'emailForOffer'),
        identity = getKey(customer_json, 'identity'),
    )

    # call FE API endpoint
    print(f'create_case.req [{req}]')
    res = FeAPI.Offer.create_case(req)
    print(f'create_case.res [{res}]')

    # set test data
    setTestKey(case_json, KEY_CASE_ID, res['caseId'])
    setTestKey(case_json, KEY_SALES_ARRAMGEMENT_ID, res['salesArrangementId'])

    setTestKey(household_json, KEY_HOUSEHOLD_ID, res['householdId'])
    setTestKey(customer_json, KEY_CUSTOMER_ON_SA_ID, res['customerOnSAId'])

    # create households
    create_households(case_json)

    # create parameters
    create_parameters(case_json)

# --------------------------------------------------------------------------------------------------------------

offer_id: int = getTestKey(offer_json, KEY_OFFER_ID)
create_case(case_json, offer_id)

case_id: int = getTestKey(case_json, KEY_CASE_ID)
print(f'https://fat.noby.cz/undefined#/mortgage/case-detail/{case_id}')

# --------------------------------------------------------------------------------------------------------------


# --------------------------------------------------------------------------------------------------------------
# FeAPI - process CASE HOUSEHOLD
# --------------------------------------------------------------------------------------------------------------

# def toReqPostIncome(income: Income) -> dict:
#     if income is None:
#         return None

#     income_json = income.to_json_value()

#     data: dict = None

  
#     # JSON_KEYS = ['incomeSource','hasProofOfIncome','incomeTypeId','sum','currencyCode']
#     return dict(
#         sum = income_json['sum'],
#         currencyCode = income_json['currencyCode'],
#         incomeTypeId = income_json['incomeTypeId'],
#         data = data
#     )

# def toReqPostObligation(obligation: Obligation) -> dict:
#     if obligation is None:
#         return None

#     obligation_json = obligation.to_json_value()

#     creditor: dict = None
#     if 'creditor' in obligation_json:
#         creditor_json = obligation_json['creditor']
#         creditor = dict(
#             name = creditor_json['name'],
#             isExternal = creditor_json['isExternal'],
#         )
        
#     correction: dict = None
#     if 'correction' in obligation_json:
#         correction_json = obligation_json['correction']
#         correction = dict(
#             correctionTypeId = correction_json['correctionTypeId'],
#             installmentAmountCorrection = correction_json['installmentAmountCorrection'],
#             loanPrincipalAmountCorrection = correction_json['loanPrincipalAmountCorrection'],
#             creditCardLimitCorrection = correction_json['creditCardLimitCorrection'],
#         )

#     # JSON_KEYS = ['obligationTypeId', 'installmentAmount', 'loanPrincipalAmount','creditor','correction']
#     return dict(
#         obligationTypeId = obligation_json['obligationTypeId'],
#         installmentAmount = obligation_json['installmentAmount'],
#         loanPrincipalAmount = obligation_json['loanPrincipalAmount'],
#         creditor = creditor,
#         correction = correction,
#     )

# def setCustomer(customer: dict, customer_id: int = None):
#     print(f'setCustomer: {customer_id} [{customer}')

#     incomes = customer.get_value('incomes')
#     if incomes is not None:
#         for i in incomes:
#             req = toReqPostIncome(i)
#             # call FE API endpoint
#             print(f'process_income.req [{req}]')
#             res = FeAPI.CustomerOnSa.create_income(customer_id, req)
#             print(f'process_income.res [{res}]')

#     obligations = customer.get_value('obligations')
#     if obligations is not None:
#         for o in obligations:
#             req = toReqPostObligation(o)
#             # call FE API endpoint
#             print(f'process_obligation.req [{req}]')
#             res = FeAPI.CustomerOnSa.create_obligation(customer_id, req)
#             print(f'process_obligation.res [{res}]')

# def addHousehold(household: dict, household_id: int = None):
#     is_main = household.get_value('householdTypeId') == EHouseholdType.Main.value

#     customer1 = household.get_value('customer1')
#     customer1_id = customer_on_sa_id if is_main else None
#     setCustomer(customer1, customer1_id)

#     customer2 = household.get_value('customer2')
#     if (customer2 is not None):
#         setCustomer(customer2, None)

#     household_json = household.to_json_value()
#     data = dict(
#         childrenUpToTenYearsCount = household_json['childrenUpToTenYearsCount'],
#         childrenOverTenYearsCount = household_json['childrenOverTenYearsCount'],
#         areCustomersPartners = household_json['areCustomersPartners'],
#         expenses = household_json['expenses'],
#     )

#     req = dict(data = data)
#     # call FE API endpoint
#     print(f'process_household.req [{req}]')
#     res = FeAPI.Household.set_household_parameters(household_id, req)
#     print(f'process_household.res [{res}]')

# def setHousehold(household_json: dict):

#     household_id: int = getTestKey(household_json, KEY_HOUSEHOLD_ID)

#     req = household_json.copy()
#     req['householdId'] = household_id
#     req['customer1']['customerOnSAId'] = getTestKey(req['customer1'], KEY_CUSTOMER_ON_SA_ID)

#     print(f'process_household.req [{req}]')
#     res = FeAPI.Household.set_household_customers(household_id, req)
#     print(f'process_household.res [{res}]')

#     setTestKey(household_json['customer2'], KEY_CUSTOMER_ON_SA_ID, res['customerOnSAId2'])

#     data = dict(
#         childrenUpToTenYearsCount = household_json['childrenUpToTenYearsCount'],
#         childrenOverTenYearsCount = household_json['childrenOverTenYearsCount'],
#         areCustomersPartners = household_json['areCustomersPartners'],
#         expenses = household_json['expenses'],
#     )

#     req = dict(data = data)

#     # call FE API endpoint
#     print(f'process_household.req [{req}]')
#     res = FeAPI.Household.set_household_parameters(household_id, req)
#     print(f'process_household.res [{res}]')


# --------------------------------------------------------------------------------------------------------------
# FE - WORKFLOW
# --------------------------------------------------------------------------------------------------------------

# https://fat.noby.cz/#/mortgage/case-detail/3011321

# POST https://noby-fat.vsskb.cz/api/offer/mortgage/create-case
# {'offerId': 691, 'firstName': 'JAN', 'lastName': 'NOVÁK', 'dateOfBirth': '1980-01-01T00:00:00', 'phoneNumberForOffer': '+420 777543234', 'emailForOffer': 'novak@testcm.cz', 'identity': {'id': 0, 'scheme': 0}}
# {'caseId': 3011321, 'salesArrangementId': 226, 'offerId': 691, 'householdId': 223, 'customerOnSAId': 262}

# GET https://fat.noby.cz/api/case/3011321
# {"caseOwner":{"cpm":"990614w","icp":""},"caseId":3011321,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","state":1,"stateName":"Příprava žádosti","contractNumber":"","targetAmount":5000000,"productName":"Hypoteční úvěr","createdTime":"2023-01-25T11:40:00.117","createdBy":"Filip Tůma","stateUpdated":"2023-01-25T11:40:00.113","emailForOffer":"novak@testcm.cz","phoneNumberForOffer":"+420 777543234"}

# add household to SA
# POST https://fat.noby.cz/api/household
# {salesArrangementId: "226", householdTypeId: 2}
# {"householdId":225,"householdTypeId":2,"householdTypeName":"Spoludlužnická"}

# load households for SA (salesArrangementId: 226)
# GET https://fat.noby.cz/api/household/list/226
# [{"householdId":223,"householdTypeId":1,"householdTypeName":"Hlavní"},{"householdId":225,"householdTypeId":2,"householdTypeName":"Spoludlužnická"}]

# set params of household
# PUT https://fat.noby.cz/api/household/225
# {data: {childrenUpToTenYearsCount: 2, childrenOverTenYearsCount: 0}, expenses: {}}

# load customers on SA
# GET https://fat.noby.cz/api/sales-arrangement/243/customers
# [{"id":291,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","customerRoleId":1},{"id":292,"firstName":"","lastName":"","customerRoleId":2},{"id":293,"firstName":"","lastName":"","customerRoleId":128}]

# add income (customerId = 291)
# POST https://fat.noby.cz/api/customer-on-sa/291/income
# {"sum":50000,"currencyCode":"CZK","incomeTypeId":1,"data":{"employer":{"name":"Agro","cin":"123","birthNumber":null,"countryId":16},"job":{"jobDescription":null,"firstWorkContractSince":null,"employmentTypeId":null,"currentWorkContractSince":null,"currentWorkContractTo":null,"grossAnnualIncome":null,"isInTrialPeriod":false,"isInProbationaryPeriod":false},"wageDeduction":{"deductionDecision":null,"deductionPayments":null,"deductionOther":null},"incomeConfirmation":{"confirmationDate":null,"confirmationPerson":null,"confirmationContact":null,"isIssuedByExternalAccountant":false},"hasProofOfIncome":false,"foreignIncomeTypeId":null,"hasWageDeduction":false,"cin":null,"birthNumber":null,"countryOfResidenceId":null,"incomeOtherTypeId":null}}
# 68

# GET https://fat.noby.cz/api/household/252

# add other parameters
# PUT https://fat.noby.cz/api/sales-arrangement/243/parameters
# {parameters: {incomeCurrencyCode: "CZK", residencyCurrencyCode: "CZK", contractSignatureTypeId: 3,…}

#
# PUT https://fat.noby.cz/api/sales-arrangement/334/parameters
# {"parameters":{"incomeCurrencyCode":"CZK","residencyCurrencyCode":"CZK","contractSignatureTypeId":3,"loanRealEstates":[{"isCollateral":true,"realEstatePurchaseTypeId":2,"realEstateTypeId":2}],"agent":522,"agentConsentWithElCom":true}}
# {"parameters":{"incomeCurrencyCode":"CZK","residencyCurrencyCode":"CZK","contractSignatureTypeId":1,"loanRealEstates":[{"realEstateTypeId":2,"isCollateral":true,"realEstatePurchaseTypeId":2}],"agent":522,"agentConsentWithElCom":true}}


# --------------------------------------------------------------------------------------------------------------
