from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'confirmation_date': lambda value: Convertor.to_date(value),
            'confirmation_person': lambda value: Convertor.to_str(value),
            'confirmation_contact': lambda value: Convertor.to_str(value),
            'is_issued_by_external_accountant': lambda value: Convertor.to_bool(value),
        }

JSON_KEYS = ['confirmationDate', 'confirmationPerson', 'confirmationContact', 'isIssuedByExternalAccountant']

#     "confirmationDate": null,
                            #     "confirmationPerson": null,
                            #     "confirmationContact": null,
                            #     "isIssuedByExternalAccountant": false

class IncomeDataIncomeConfirmation(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return IncomeDataIncomeConfirmation(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: IncomeDataIncomeConfirmation.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, IncomeDataIncomeConfirmation)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
                            # "incomeConfirmation": {
                            #     "confirmationDate": null,
                            #     "confirmationPerson": null,
                            #     "confirmationContact": null,
                            #     "isIssuedByExternalAccountant": false
                            # },
# --------------------------------------------------------------------------------------------