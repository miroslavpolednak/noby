from .WBase import WBase

from Types_pb2 import RealEstateInsurance

class WRealEstateInsurance(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=RealEstateInsurance(), js_dict = js_dict)

# message RealEstateInsurance {
# 	cis.types.GrpcDecimal Sum = 1;
# 	google.protobuf.Int32Value Frequency = 2;
# }