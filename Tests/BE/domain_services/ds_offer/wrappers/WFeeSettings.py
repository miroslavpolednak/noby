from .WBase import WBase

from Types_pb2 import FeeSettings

class WFeeSettings(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=FeeSettings(), js_dict = js_dict)

# message FeeSettings {
# 	google.protobuf.Int32Value FeeTariffPurpose = 1;
# 	bool IsStatementCharged = 2;
# }