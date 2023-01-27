from ..base.Base import Base
from .Creditor import Creditor
from .ObligationCorrection import ObligationCorrection

from common import Convertor

from typing import List

DISPATCHES = {
            'obligation_type_id': lambda value: Convertor.to_int(value),
            'installment_amount': lambda value: Convertor.to_decimal(value),
            'loan_principal_amount': lambda value: Convertor.to_decimal(value),

            'creditor': lambda value: Creditor.from_json(value),
            'correction': lambda value: ObligationCorrection.from_json(value),
        }

class Obligation(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Obligation(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Obligation.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Obligation)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()


# ?????:
# obligationId
# scustomerOnSAId

# --------------------------------------------------------------------------------------------
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
# --------------------------------------------------------------------------------------------
