# ----------------------------
import _setup
# ----------------------------

import json
from typing import List

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI

from business.offer import Offer
from business.case import Case

from tests.tuning.data import load_file_json

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# DATA
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# EProductType:     Mortgage, MortgageBridging, MortgageWithoutIncome, MortgageNonPurposePart, MortgageAmerican
# ELoanKind:        Standard, MortgageWithoutRealty
# EHouseholdType:   Main, Codebtor, Garantor
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

test_data: dict = load_file_json()
#print(test_data)

js_dict = test_data['offer']
offer = Offer.from_json(js_dict)
print(offer)
print(offer.to_grpc())
print(Offer.to_grpc(offer))

js_dict = dict(
    offer = test_data['offer'],
    households = test_data['households'],
    parameters = test_data['parameters'],
    )

case = Case.from_json(js_dict)
print(case)


# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# OPTION RESOLVERS
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
def to_date(days: int = 0) -> str:
    from datetime import datetime, timedelta

    date: datetime = datetime.today() + timedelta(days=days)

    #return date.isoformat()
    return date.strftime('%Y-%m-%d')

def to_product_type_id(product_type) -> int:
    e = EProductType[product_type]
    return e.value

def to_loan_kind_id(loan_kind) -> int:
    e = ELoanKind[loan_kind]
    return e.value

def to_household_type_id(household_type) -> int:
    e = EHouseholdType[household_type]
    return e.value

def resolve_options(options: dict, option_resolvers: dict) -> dict:

    if (options is None):
        return None

    result = {}

    for k in options.keys():
        assert k in option_resolvers.keys(), f'Option resolver not specified [{k}]'
        target_key = option_resolvers[k]['target_key']
        fce = option_resolvers[k]['fce']
        value_in = options[k]
        value_out = fce(value_in)
        result[target_key] = value_out

    return result          

option_resolvers_household = {
    'HouseholdType': dict(fce = lambda value: to_household_type_id(value), target_key = 'householdTypeId'),
}

option_resolvers_offer = {
    'ExpectedDateOfDrawing': dict(fce = lambda value: to_date(value), target_key = 'expectedDateOfDrawing'),
    'ProductType': dict(fce = lambda value: to_product_type_id(value), target_key = 'productTypeId'),
    'LoanKind': dict(fce = lambda value: to_loan_kind_id(value), target_key = 'loanKindId'),
}

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

class TestProcessor():

    __instance = None

    def __init__(self):
        self.__data = test_data
        
    @staticmethod
    def instance():
        if (TestProcessor.__instance is None):
            TestProcessor.__instance = TestProcessor()
        return TestProcessor.__instance
        
    def process_case(self):
        print(self.__data)



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

# --------------------------------------------------------------------------------------------------------------
