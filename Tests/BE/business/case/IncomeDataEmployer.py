from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'name': lambda value: Convertor.to_str(value),
            'cin': lambda value: Convertor.to_str(value),
            'birth_number': lambda value: Convertor.to_str(value),
            'country_id': lambda value: Convertor.to_int(value),
        }

JSON_KEYS = ['name', 'cin', 'birthNumber','countryId']

class IncomeDataEmployer(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return IncomeDataEmployer(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: IncomeDataEmployer.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, IncomeDataEmployer)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
                            # "employer": {
                            #     "name": "Agro",
                            #     "cin": "123",
                            #     "birthNumber": null,
                            #     "countryId": 16
                            # },
# --------------------------------------------------------------------------------------------