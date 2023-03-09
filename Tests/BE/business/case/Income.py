from ..base.Base import Base
from .IncomeData import IncomeData

from common import Convertor

from typing import List

DISPATCHES = {
            'income_id': lambda value: Convertor.to_int(value),            
            'income_type_id': lambda value: Convertor.to_int(value),
            'sum': lambda value: Convertor.to_decimal(value),
            'currency_code': lambda value: Convertor.to_str(value),

            'data': lambda value: IncomeData.from_json(value),
        }

JSON_KEYS = ['incomeTypeId', 'sum', 'currencyCode', 'data']

class Income(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Income(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Income.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Income)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS


# --------------------------------------------------------------------------------------------
#                     {
#                         "incomeId": 68,
#                         "incomeSource": "Agro",
#                         "hasProofOfIncome": false,
#                         "incomeTypeId": 1,
#                         "sum": 50000,
#                         "currencyCode": "CZK"
#                     }
# --------------------------------------------------------------------------------------------