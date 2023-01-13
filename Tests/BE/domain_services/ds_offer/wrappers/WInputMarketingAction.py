from .WBase import WBase

from Types_pb2 import InputMarketingAction

class WInputMarketingAction(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=InputMarketingAction(), js_dict = js_dict)

# message InputMarketingAction {
# 	bool Domicile = 1;
# 	bool HealthRiskInsurance = 2;
# 	bool RealEstateInsurance = 3;
# 	bool IncomeLoanRatioDiscount = 4;
# 	bool UserVip = 5;
# }