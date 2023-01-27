from ..base.Base import Base

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
        return list(map(lambda i: Fee.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Fee)
        return value.to_grpc()

    def to_grpc(self) -> dict:

        # message InputFee {
        #     int32 FeeId = 1;
        #     cis.types.GrpcDecimal DiscountPercentage = 2;
        # }

        return dict(
            FeeId = Convertor.to_grpc(self.get_value('fee_id')),
            DiscountPercentage = Convertor.to_grpc(self.get_value('discount_percentage')),
        )