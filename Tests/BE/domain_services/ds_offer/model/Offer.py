from .Base import Base
from .LoanPurpose import LoanPurpose
from .Developer import Developer
from .MarketingActions import MarketingActions
from .Insurance import Insurance
from .Fee import Fee

from common import Convertor

DISPATCHES = {
            'product_type_id': lambda value: Convertor.to_int(value),
            'loan_kind_id': lambda value: Convertor.to_int(value),
            'loan_amount': lambda value: Convertor.to_val(value),
            'loan_duration': lambda value: Convertor.to_val(value),
            'fixed_rate_period': lambda value: Convertor.to_val(value),
            'collateral_amount': lambda value: Convertor.to_val(value),
            'payment_day': lambda value: Convertor.to_val(value),
            'statement_type_id': lambda value: Convertor.to_int(value),
            'is_employee_bonus_requested': lambda value: Convertor.to_val(value),
            'expected_date_of_drawing': lambda value: Convertor.to_date(value),
            'with_guarantee': lambda value: Convertor.to_val(value),
            'financial_resources_own': lambda value: Convertor.to_val(value),
            'financial_resources_other': lambda value: Convertor.to_val(value),
            'drawing_type_id': lambda value: Convertor.to_int(value),
            'drawing_duration_id': lambda value: Convertor.to_int(value),
            'interest_rate_discount': lambda value: Convertor.to_val(value),
            'interest_rate_discount_toggle': lambda value: Convertor.to_val(value),
            'resource_process_id': lambda value: Convertor.to_str(value),

            'loan_purposes': lambda value: LoanPurpose.from_json_list(value),
            'marketing_actions': lambda value: MarketingActions.from_json(value),
            'developer': lambda value: Developer.from_json(value),
            'risk_life_insurance': lambda value: Insurance.from_json(value),
            'real_estate_insurance': lambda value: Insurance.from_json(value),
            'fees': lambda value: Fee.from_json_list(value),
        }


class Offer(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @property
    def product_type_id(self) -> int:
        return self.__product_type_id

    @staticmethod
    def from_json(js_dict: dict):
        return Offer(js_dict = js_dict)






# --------------------------------------------------------------------------------------------

# class OfferPrev():
#     def __init__(self, js_dict: dict = None):

#         dispatches = {
#             'product_type_id': lambda value: Convertor.to_int(value),
#             'loan_kind_id': lambda value: Convertor.to_val(value),
#             'loan_amount': lambda value: Convertor.to_val(value),
#             'loan_duration': lambda value: Convertor.to_val(value),
#             'fixed_rate_period': lambda value: Convertor.to_val(value),
#             'collateral_amount': lambda value: Convertor.to_val(value),
#             'payment_day': lambda value: Convertor.to_val(value),
#             'statement_type_id': lambda value: Convertor.to_val(value),
#             'is_employee_bonus_requested': lambda value: Convertor.to_val(value),
#             'expected_date_of_drawing': lambda value: Convertor.to_val(value),
#             'with_guarantee': lambda value: Convertor.to_val(value),
#             'financial_resources_own': lambda value: Convertor.to_val(value),
#             'financial_resources_other': lambda value: Convertor.to_val(value),
#             'drawing_type_id': lambda value: Convertor.to_val(value),
#             'drawing_duration_id': lambda value: Convertor.to_val(value),
#             'interest_rate_discount': lambda value: Convertor.to_val(value),
#             'interest_rate_discount_toggle': lambda value: Convertor.to_val(value),
#             'resource_process_id': lambda value: Convertor.to_val(value),

#             'loan_purposes': lambda value: Convertor.to_val(value),
#             'marketing_actions': lambda value: Convertor.to_val(value),
#             'developer': lambda value: Convertor.to_val(value),
#             'risk_life_insurance': lambda value: Convertor.to_val(value),
#             'real_estate_insurance': lambda value: Convertor.to_val(value),
#             'fees': lambda value: Convertor.to_val(value),
#         }

#         # replace '_' in dict keys 
#         for k in list(dispatches.keys()):
#             k_next = k.replace('_', '').lower()
#             dispatches[k_next] = dict(fce = dispatches[k], key = k)
#             if k != k_next:
#                 del dispatches[k]

#         self.__dispatches = dispatches
#         self.__init_from_json(js_dict)
        
#     def __init_from_json(self, js_dict: dict):
#         assert js_dict is not None, f'Json data must be provided!'
#         print(js_dict)

#         for k in list(js_dict.keys()):
#             assert k.lower() in self.__dispatches, f"Invalid attribute '{k}'"

#             dispatch = self.__dispatches[k.lower()]
#             dispatch_fce = dispatch['fce']
#             dispatch_key = dispatch['key']

#             val_in = js_dict[k]
#             val_out = dispatch_fce(val_in)

#             attr = f'_{self.__class__.__name__}__{dispatch_key}'
#             setattr(self, attr, val_out)

#     def __str__ (self):
#         return f'Offer [product_type_id:{self.__product_type_id}]'

#     @property
#     def product_type_id(self) -> int:
#         return self.__product_type_id
        
# --------------------------------------------------------------------------------------------


