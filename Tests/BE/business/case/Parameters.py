from ..base.Base import Base
from .LoanRealEstate import LoanRealEstate

from common import Convertor

DISPATCHES = {
            'expected_date_of_drawing': lambda value: Convertor.to_date(value),
            'income_currency_code': lambda value: Convertor.to_str(value),
            'residency_currency_code': lambda value: Convertor.to_str(value),
            'contract_signature_type_id': lambda value: Convertor.to_int(value),
            'agent': lambda value: Convertor.to_int(value),
            'agent_consent_with_elcom': lambda value: Convertor.to_bool(value),

            'loan_real_estates': lambda value: LoanRealEstate.from_json_list(value),
        }

JSON_KEYS = ['expectedDateOfDrawing','incomeCurrencyCode','residencyCurrencyCode','contractSignatureTypeId','agent','agentConsentWithElCom','loanRealEstates']

class Parameters(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return Parameters(js_dict = js_dict)

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Parameters)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
    # "parameters": {
    #     "expectedDateOfDrawing": "2023-01-27T00:00:00.000Z",
    #     "incomeCurrencyCode": "CAD",
    #     "residencyCurrencyCode": "CZK",
    #     "contractSignatureTypeId": 2,
    #     "loanRealEstates": [
    #         {
    #             "realEstateTypeId": 1,
    #             "isCollateral": true,
    #             "realEstatePurchaseTypeId": 1
    #         },
    #         {
    #             "realEstateTypeId": 5,
    #             "isCollateral": false,
    #             "realEstatePurchaseTypeId": 2
    #         }
    #     ],
    #     "agent": 291,
    #     "agentConsentWithElCom": false
    # }
# --------------------------------------------------------------------------------------------