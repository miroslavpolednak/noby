from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'creditor_id': lambda value: Convertor.to_str(value),
            'name': lambda value: Convertor.to_str(value),
            'is_external': lambda value: Convertor.to_bool(value),
        }

JSON_KEYS = ['name','isExternal', 'creditorId']

class ObligationCreditor(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return ObligationCreditor(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: ObligationCreditor.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, ObligationCreditor)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
#                         "creditor": {
#                             "creditorId": "",
#                             "name": "",
#                             "isExternal": true
#                         },
# --------------------------------------------------------------------------------------------
