from .WBase import WBase

from Types_pb2 import RiskLifeInsurance

class WRiskLifeInsurance(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=RiskLifeInsurance(), js_dict = js_dict)

# message RiskLifeInsurance {
# 	cis.types.GrpcDecimal Sum = 1;
# 	google.protobuf.Int32Value Frequency = 2;
# }