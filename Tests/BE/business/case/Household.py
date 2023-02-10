from ..base.Base import Base
from .Customer import Customer
from .Expenses import Expenses

from common import Convertor

from typing import List

DISPATCHES = {
            'household_type_id': lambda value: Convertor.to_int(value),
            'children_up_to_ten_years_count': lambda value: Convertor.to_int(value),
            'children_over_ten_years_count': lambda value: Convertor.to_int(value),
            'are_customers_partners': lambda value: Convertor.to_bool(value),

            'customer1': lambda value: Customer.from_json(value),
            'customer2': lambda value: Customer.from_json(value),
            'expenses': lambda value: Expenses.from_json(value), 
        }

JSON_KEYS = ['householdTypeId','childrenUpToTenYearsCount','childrenOverTenYearsCount','areCustomersPartners','customer1','customer2','expenses']

class Household(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Household(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Household.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Household)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS


# --------------------------------------------------------------------------------------------
#    {
#             "HouseholdType": "Main",
#             "childrenUpToTenYearsCount": 1,
#             "childrenOverTenYearsCount": 1,
#             "areCustomersPartners": false,
#             "customer1": {
#                 "firstName": "JAN",
#                 "lastName": "Novák",
#                 "dateOfBirth": "1980-01-01T00:00:00",
#                 "phoneNumberForOffer": "+420 777543234",
#                 "emailForOffer": "novak@testcm.cz",
#                 "identity": {
#                     "id": 0,
#                     "scheme": 0
#                 },
#                 "roleId": 1,
#                 "incomes": [
#                     {
#                         "incomeId": 68,
#                         "incomeSource": "Agro",
#                         "hasProofOfIncome": false,
#                         "incomeTypeId": 1,
#                         "sum": 50000,
#                         "currencyCode": "CZK"
#                     }
#                 ],
#                 "obligations": [
#                     {
#                         "obligationId": 43,
#                         "customerOnSAId": 291,
#                         "obligationTypeId": 1,
#                         "installmentAmount": 10000,
#                         "loanPrincipalAmount": 1000000,
#                         "creditor": {
#                             "creditorId": "",
#                             "name": "",
#                             "isExternal": true
#                         },
#                         "correction": {
#                             "correctionTypeId": 1
#                         }
#                     }
#                 ]
#             },
#             "customer2": {
#                 "firstName": "JANA",
#                 "lastName": "Nováková",
#                 "dateOfBirth": "1980-01-01T00:00:00",
#                 "phoneNumberForOffer": "+420 777543234",
#                 "emailForOffer": "novak@testcm.cz",
#                 "identity": {
#                     "id": 0,
#                     "scheme": 0
#                 },
#                 "roleId": 2
#             },
#             "expenses": {
#                 "savingExpenseAmount": 10000,
#                 "insuranceExpenseAmount": 3000,
#                 "housingExpenseAmount": 10000,
#                 "otherExpenseAmount": 7500
#             }
#         }
# --------------------------------------------------------------------------------------------