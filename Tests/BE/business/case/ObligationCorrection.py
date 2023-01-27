from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'correction_type_id': lambda value: Convertor.to_int(value),
            'installment_amount_correction': lambda value: Convertor.to_decimal(value),
            'loan_principal_amount_correction': lambda value: Convertor.to_decimal(value),
            'credit_card_limit_correction': lambda value: Convertor.to_decimal(value),
        }

class ObligationCorrection(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return ObligationCorrection(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: ObligationCorrection.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, ObligationCorrection)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()


# --------------------------------------------------------------------------------------------
#                         "correction": {
#                             "correctionTypeId": 1
#                         }
# --------------------------------------------------------------------------------------------