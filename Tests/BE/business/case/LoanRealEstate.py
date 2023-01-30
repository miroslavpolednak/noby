from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'real_estate_type_id': lambda value: Convertor.to_int(value),
            'is_collateral': lambda value: Convertor.to_bool(value),
            'real_estate_purchase_type_id': lambda value: Convertor.to_int(value),
        }

JSON_KEYS = ['realEstateTypeId', 'isCollateral','realEstatePurchaseTypeId']

class LoanRealEstate(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return LoanRealEstate(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: LoanRealEstate.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, LoanRealEstate)
        return value.to_grpc()

    @staticmethod
    def to_grpc_list(value_list: List[object]):
        if (value_list is None):
            return None
        assert isinstance(value_list, list)
        return list(map(lambda value: LoanRealEstate.to_grpc(value), value_list))

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
    #           {
    #               "realEstateTypeId": 1,
    #               "isCollateral": true,
    #               "realEstatePurchaseTypeId": 1
    #           },
# --------------------------------------------------------------------------------------------        