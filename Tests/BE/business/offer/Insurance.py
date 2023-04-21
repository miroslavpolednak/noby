from ..base.Base import Base

from common import Convertor

DISPATCHES = {
            'sum': lambda value: Convertor.to_decimal(value),
            'frequency': lambda value: Convertor.to_int(value),
        }

JSON_KEYS = ['sum', 'frequency']

class Insurance(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Insurance(js_dict = js_dict)

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Insurance)
        return value.to_grpc()

    def to_grpc(self) -> dict:

        #    message RiskLifeInsurance {
        #         cis.types.NullableGrpcDecimal Sum = 1;
        #         google.protobuf.Int32Value Frequency = 2;
        #     }

        #     message RealEstateInsurance {
        #         cis.types.NullableGrpcDecimal Sum = 1;
        #         google.protobuf.Int32Value Frequency = 2;
        #     }


        return dict(
            Sum = Convertor.to_grpc(self.get_value('sum')),
            Frequency = Convertor.to_grpc(self.get_value('frequency')),
        )
    
    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------

# class Insurance():
#     def __init__(self, js_dict: dict = None):

#         self.__sum = None
#         self.__frequency = None

# --------------------------------------------------------------------------------------------