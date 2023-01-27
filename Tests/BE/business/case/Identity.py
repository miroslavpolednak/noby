from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'id': lambda value: Convertor.to_int(value),
            'scheme': lambda value: Convertor.to_int(value),
        }

class Identity(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Identity(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: Identity.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Identity)
        return value.to_grpc()

    @staticmethod
    def to_grpc_list(value_list: List[object]):
        if (value_list is None):
            return None
        assert isinstance(value_list, list)
        return list(map(lambda value: Identity.to_grpc(value), value_list))

    def to_grpc(self) -> dict:
       return dict()

# --------------------------------------------------------------------------------------------
#                 "identity": {
#                     "id": 0,
#                     "scheme": 0
#                 },
# --------------------------------------------------------------------------------------------        