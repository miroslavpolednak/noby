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
        return list(map(lambda i: LoanPurpose.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, LoanPurpose)
        return value.to_grpc()

    @staticmethod
    def to_grpc_list(value_list: List[object]):
        if (value_list is None):
            return None
        assert isinstance(value_list, list)
        return list(map(lambda value: LoanPurpose.to_grpc(value), value_list))

    def to_grpc(self) -> dict:

        # message LoanPurpose {
        #     int32 LoanPurposeId = 1;
        #     cis.types.GrpcDecimal Sum = 2;
        # }

        return dict(
            LoanPurposeId = Convertor.to_grpc(self.get_value('id')),
            Sum = Convertor.to_grpc(self.get_value('sum')),
        )

# --------------------------------------------------------------------------------------------

# class LoanPurpose():

#     def __init__(self, js_dict: dict = None):

#         self.__id = None
#         self.__sum = None

# --------------------------------------------------------------------------------------------        