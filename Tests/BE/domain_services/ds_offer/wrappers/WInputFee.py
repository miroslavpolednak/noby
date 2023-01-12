from .WBase import WBase

from Types_pb2 import InputFee

class WInputFee(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=InputFee(), js_dict = js_dict)

# message InputFee {
# 	int32 FeeId = 1;
# 	cis.types.GrpcDecimal DiscountPercentage = 2;
# }