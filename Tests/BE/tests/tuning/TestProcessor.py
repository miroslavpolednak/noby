# ----------------------------
import _setup
# ----------------------------

import json
from typing import List

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI

from business.base import Base
from business.offer import Offer
from business.case import Case

from tests.tuning.OptionsResolver import OptionsResolver
from tests.tuning.data import load_file_json

test_data: dict = load_file_json()
#print(test_data)

js_dict = test_data['offer']

#print(','.join(list(map(lambda k: f"'{k}'", list(js_dict.keys())))) )

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

case = Case.from_json(test_data)
print(case)


# --------------------------------------------------------------------------------------------------------------
# FeAPI - process OFFER
# --------------------------------------------------------------------------------------------------------------

offer = case.get_value('offer')
req = offer.to_json_value()
#print(req)

#req = test_data['offer']
#print(test_data['offer'])

# call FE API endpoint 
print(f'process_offer.req [{req}]')
res = FeAPI.Offer.simulate_mortgage(req)
print(f'process_offer.res [{res}]')

offer_id = res['offerId']
print(offer_id)


# response serialization & persistence
#res_str = json.dumps(res)

# --------------------------------------------------------------------------------------------------------------
# FeAPI - process CASE
# --------------------------------------------------------------------------------------------------------------

households = case.get_value('households')
household_main = list(filter(lambda i: i.get_value('householdTypeId') == 1, households))[0]
household_main_customer1 = household_main.get_value('customer1')

print(households)
print(household_main)
print(household_main_customer1)

customer_json = household_main_customer1.to_json_value()
print(customer_json)

req = dict(
    offerId = offer_id,
    firstName = customer_json['firstName'],
    lastName = customer_json['lastName'],
    dateOfBirth = customer_json['dateOfBirth'],
    phoneNumberForOffer = customer_json['phoneNumberForOffer'],
    emailForOffer = customer_json['emailForOffer'],
    identity = customer_json['identity'],
)

# call FE API endpoint
print(f'process_case.req [{req}]')
res = FeAPI.Offer.create_case(req)
print(f'process_case.res [{res}]')

#{'offerId': 691, 'firstName': 'JAN', 'lastName': 'NOVÁK', 'dateOfBirth': '1980-01-01T00:00:00', 'phoneNumberForOffer': '+420 777543234', 'emailForOffer': 'novak@testcm.cz', 'identity': {'id': 0, 'scheme': 0}}




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
