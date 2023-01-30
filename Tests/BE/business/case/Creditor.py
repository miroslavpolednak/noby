from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'name': lambda value: Convertor.to_str(value),
            'is_external': lambda value: Convertor.to_bool(value),
        }

JSON_KEYS = ['name','isExternal']

class Creditor(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Creditor(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Creditor.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Creditor)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# ?????:
# creditorId

# --------------------------------------------------------------------------------------------
#                         "creditor": {
#                             "creditorId": "",
#                             "name": "",
#                             "isExternal": true
#                         },
# --------------------------------------------------------------------------------------------
