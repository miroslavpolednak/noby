from ..base.Base import Base
from .Identity import Identity
from .Income import Income
from .Obligation import Obligation

from common import Convertor

from typing import List

DISPATCHES = {
            'customer_on_sa_id': lambda value: Convertor.to_int(value),
            'first_name': lambda value: Convertor.to_str(value),
            'last_name': lambda value: Convertor.to_str(value),
            'date_of_birth': lambda value: Convertor.to_date(value),
            'role_id': lambda value: Convertor.to_int(value),

            'identity': lambda value: Identity.from_json(value),
            'incomes': lambda value: Income.from_json_list(value),
            'obligations': lambda value: Obligation.from_json_list(value),
        }

JSON_KEYS = ['firstName','lastName','dateOfBirth','roleId','identity','incomes','obligations']

class Customer(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Customer(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Customer.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Customer)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS


# ?????:

# --------------------------------------------------------------------------------------------
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
#             }
# --------------------------------------------------------------------------------------------