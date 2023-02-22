from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'deduction_decision': lambda value: Convertor.to_decimal(value),
            'deduction_payments': lambda value: Convertor.to_decimal(value),
            'deduction_other': lambda value: Convertor.to_decimal(value),
        }

JSON_KEYS = ['deductionDecision', 'deductionPayments', 'deductionOther']

class IncomeDataWageDeduction(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return IncomeDataWageDeduction(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: IncomeDataWageDeduction.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, IncomeDataWageDeduction)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
                            # "wageDeduction": {
                            #     "deductionDecision": null,
                            #     "deductionPayments": null,
                            #     "deductionOther": null
                            # },
# --------------------------------------------------------------------------------------------