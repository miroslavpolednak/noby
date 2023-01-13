from .Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'fee_id': lambda value: Convertor.to_int(value),
            'discount_percentage': lambda value: Convertor.to_decimal(value),
        }

class Fee(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Fee(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda d: Fee.from_json(d), js_dict_list))
