from ..base.Base import Base

from ..offer.Offer import Offer

from .Household import Household
from .Parameters import Parameters
# from .Developer import Developer
# from .MarketingActions import MarketingActions
# from .Insurance import Insurance
# from .Fee import Fee

from common import Convertor

DISPATCHES = {
            'offer': lambda value: Offer.from_json(value),
            # 'product_type_id': lambda value: Convertor.to_int(value),
            # 'loan_kind_id': lambda value: Convertor.to_int(value),
            # 'loan_amount': lambda value: Convertor.to_decimal(value),
            # 'loan_duration': lambda value: Convertor.to_int(value),
            # 'fixed_rate_period': lambda value: Convertor.to_int(value),
            # 'collateral_amount': lambda value: Convertor.to_decimal(value),
            # 'payment_day': lambda value: Convertor.to_int(value),
            # 'statement_type_id': lambda value: Convertor.to_int(value),
            # 'is_employee_bonus_requested': lambda value: Convertor.to_bool(value),
            # 'expected_date_of_drawing': lambda value: Convertor.to_date(value),
            # 'with_guarantee': lambda value: Convertor.to_bool(value),
            # 'financial_resources_own': lambda value: Convertor.to_decimal(value),
            # 'financial_resources_other': lambda value: Convertor.to_decimal(value),
            # 'drawing_type_id': lambda value: Convertor.to_int(value),
            # 'drawing_duration_id': lambda value: Convertor.to_int(value),
            # 'interest_rate_discount': lambda value: Convertor.to_decimal(value),
            # 'interest_rate_discount_toggle': lambda value: Convertor.to_bool(value),
            # 'resource_process_id': lambda value: Convertor.to_guid(value),

            'households': lambda value: Household.from_json_list(value),
            'parameters': lambda value: Parameters.from_json(value),

            # 'developer': lambda value: Developer.from_json(value),
            # 'risk_life_insurance': lambda value: Insurance.from_json(value),
            # 'real_estate_insurance': lambda value: Insurance.from_json(value),
            # 'fees': lambda value: Fee.from_json_list(value),
        }

class Case(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @property
    def product_type_id(self) -> int:
        return self.__product_type_id

    @staticmethod
    def from_json(js_dict: dict):
        return Case(js_dict = js_dict)

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Case)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()



# ?????:

# --------------------------------------------------------------------------------------------

# {
#     "households": [
#         {
#             "HouseholdType": "Main",
#             "childrenUpToTenYearsCount": 1,
#             "childrenOverTenYearsCount": 1,
#             "areCustomersPartners": false,
#             "customer1": {
#                 "firstName": "JAN",
#                 "lastName": "Novák",
#                 "dateOfBirth": "1980-01-01T00:00:00",
#                 "phoneNumberForOffer": "+420 777543234",
#                 "emailForOffer": "novak@testcm.cz",
#                 "identity": {
#                     "id": 0,
#                     "scheme": 0
#                 },
#                 "roleId": 1,
#                 "incomes": [
#                     {
#                         "incomeId": 68,
#                         "incomeSource": "Agro",
#                         "hasProofOfIncome": false,
#                         "incomeTypeId": 1,
#                         "sum": 50000,
#                         "currencyCode": "CZK"
#                     }
#                 ],
#                 "obligations": [
#                     {
#                         "obligationId": 43,
#                         "customerOnSAId": 291,
#                         "obligationTypeId": 1,
#                         "installmentAmount": 10000,
#                         "loanPrincipalAmount": 1000000,
#                         "creditor": {
#                             "creditorId": "",
#                             "name": "",
#                             "isExternal": true
#                         },
#                         "correction": {
#                             "correctionTypeId": 1
#                         }
#                     }
#                 ]
#             },
#             "customer2": {
#                 "firstName": "JANA",
#                 "lastName": "Nováková",
#                 "dateOfBirth": "1980-01-01T00:00:00",
#                 "phoneNumberForOffer": "+420 777543234",
#                 "emailForOffer": "novak@testcm.cz",
#                 "identity": {
#                     "id": 0,
#                     "scheme": 0
#                 },
#                 "roleId": 2
#             },
#             "expenses": {
#                 "savingExpenseAmount": 10000,
#                 "insuranceExpenseAmount": 3000,
#                 "housingExpenseAmount": 10000,
#                 "otherExpenseAmount": 7500
#             }
#         }
#     ],
#     "parameters": {
#         "expectedDateOfDrawing": "2023-01-27T00:00:00.000Z",
#         "incomeCurrencyCode": "CAD",
#         "residencyCurrencyCode": "CZK",
#         "contractSignatureTypeId": 2,
#         "loanRealEstates": [
#             {
#                 "realEstateTypeId": 1,
#                 "isCollateral": true,
#                 "realEstatePurchaseTypeId": 1
#             },
#             {
#                 "realEstateTypeId": 5,
#                 "isCollateral": false,
#                 "realEstatePurchaseTypeId": 2
#             }
#         ],
#         "agent": 291,
#         "agentConsentWithElCom": false
#     }
# }
        
# --------------------------------------------------------------------------------------------


