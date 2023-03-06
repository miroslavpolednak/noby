from ..base.Base import Base
from ..offer.Offer import Offer
from .Household import Household
from .Parameters import Parameters

from common import Convertor

DISPATCHES = {
            #'offer': lambda value: Offer.from_json(value),

            'phone_number_for_offer': lambda value: Convertor.to_str(value),
            'email_for_offer': lambda value: Convertor.to_str(value),
            'households': lambda value: Household.from_json_list(value),
            'parameters': lambda value: Parameters.from_json(value),
        }

JSON_KEYS = ['phoneNumberForOffer','emailForOffer','households','parameters']
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

    def _get_json_keys(self):
        return JSON_KEYS


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