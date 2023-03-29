from .Base import Base

from common import Convertor

DISPATCHES = {
            'domicile': lambda value: Convertor.to_bool(value),
            'health_risk_insurance': lambda value: Convertor.to_bool(value),
            'real_estate_insurance': lambda value: Convertor.to_bool(value),
            'income_loan_ratio_discount': lambda value: Convertor.to_bool(value),
            'user_vip': lambda value: Convertor.to_bool(value),
        }

class MarketingActions(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return MarketingActions(js_dict = js_dict)

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, MarketingActions)
        return value.to_grpc()

    def to_grpc(self) -> dict:

        # message InputMarketingAction {
        #     bool Domicile = 1;
        #     bool HealthRiskInsurance = 2;
        #     bool RealEstateInsurance = 3;
        #     bool IncomeLoanRatioDiscount = 4;
        #     bool UserVip = 5;
        # }

        return dict(
            Domicile = Convertor.to_grpc(self.get_value('domicile')),
            HealthRiskInsurance = Convertor.to_grpc(self.get_value('health_risk_insurance')),
            RealEstateInsurance = Convertor.to_grpc(self.get_value('real_estate_insurance')),
            IncomeLoanRatioDiscount = Convertor.to_grpc(self.get_value('income_loan_ratio_discount')),
            UserVip = Convertor.to_grpc(self.get_value('user_vip')),
        )        


# --------------------------------------------------------------------------------------------

# class MarketingActions():

#     def __init__(self, js_dict: dict = None):

#         self.__domicile = None
#         self.__health_risk_insurance = None
#         self.__real_estate_insurance = None
#         self.__income_loan_ratio_discount = None
#         self.__user_vip = None

# --------------------------------------------------------------------------------------------        