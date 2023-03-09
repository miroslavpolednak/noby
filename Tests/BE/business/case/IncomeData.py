from ..base.Base import Base
from .IncomeDataEmployer import IncomeDataEmployer
from .IncomeDataJob import IncomeDataJob
from .IncomeDataWageDeduction import IncomeDataWageDeduction
from .IncomeDataIncomeConfirmation import IncomeDataIncomeConfirmation

from common import Convertor

from typing import List

DISPATCHES = {
            'income_source': lambda value: Convertor.to_str(value),             #TODO: Income x IncomeData ?
            'has_proof_of_income': lambda value: Convertor.to_bool(value),      #TODO: Income x IncomeData ?
            'foreign_income_type_id': lambda value: Convertor.to_int(value),
            'has_wage_deduction': lambda value: Convertor.to_bool(value),
            'cin': lambda value: Convertor.to_str(value),
            'birth_number': lambda value: Convertor.to_str(value),
            'country_of_residence_id': lambda value: Convertor.to_int(value),
            'income_other_type_id': lambda value: Convertor.to_int(value),

            'employer': lambda value: IncomeDataEmployer.from_json(value),
            'job': lambda value: IncomeDataJob.from_json(value),
            'wage_deduction': lambda value: IncomeDataWageDeduction.from_json(value),
            'income_confirmation': lambda value: IncomeDataIncomeConfirmation.from_json(value),
        }

JSON_KEYS = ['incomeSource', 'hasProofOfIncome', 'foreignIncomeTypeId', 'hasWageDeduction', 'cin', 'birthNumber', 'countryOfResidenceId', 'incomeOtherTypeId', 'employer', 'job', 'wageDeduction', 'incomeConfirmation']

class IncomeData(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return IncomeData(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: IncomeData.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, IncomeData)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS


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
