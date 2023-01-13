from .Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'id': lambda value: Convertor.to_int(value),
            'sum': lambda value: Convertor.to_decimal(value),
        }

class LoanPurpose(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return LoanPurpose(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda d: LoanPurpose.from_json(d), js_dict_list))


# --------------------------------------------------------------------------------------------

# class LoanPurpose():

#     def __init__(self, js_dict: dict = None):

#         self.__id = None
#         self.__sum = None

# --------------------------------------------------------------------------------------------        