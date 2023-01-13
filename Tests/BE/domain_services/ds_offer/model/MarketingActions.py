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


# --------------------------------------------------------------------------------------------

# class MarketingActions():

#     def __init__(self, js_dict: dict = None):

#         self.__domicile = None
#         self.__health_risk_insurance = None
#         self.__real_estate_insurance = None
#         self.__income_loan_ratio_discount = None
#         self.__user_vip = None

# --------------------------------------------------------------------------------------------        