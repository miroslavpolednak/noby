from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'saving_expense_amount': lambda value: Convertor.to_int(value),
            'insurance_expense_amount': lambda value: Convertor.to_int(value),
            'housing_expense_amount': lambda value: Convertor.to_int(value),
            'other_expense_amount': lambda value: Convertor.to_int(value),
        }

JSON_KEYS = ['savingExpenseAmount','insuranceExpenseAmount','housingExpenseAmount','otherExpenseAmount']

class Expenses(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Expenses(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Expenses.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Expenses)
        return value.to_grpc()

    @staticmethod
    def to_grpc_list(value_list: List[object]):
        if (value_list is None):
            return None
        assert isinstance(value_list, list)
        return list(map(lambda value: Expenses.to_grpc(value), value_list))

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS


# --------------------------------------------------------------------------------------------
#             "expenses": {
#                 "savingExpenseAmount": 10000,
#                 "insuranceExpenseAmount": 3000,
#                 "housingExpenseAmount": 10000,
#                 "otherExpenseAmount": 7500
#             }
# --------------------------------------------------------------------------------------------        