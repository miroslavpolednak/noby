from ..base.Base import Base

from common import Convertor

from typing import List

DISPATCHES = {
            'job_description': lambda value: Convertor.to_str(value),
            'first_work_contract_since': lambda value: Convertor.to_date(value),
            'employment_type_id': lambda value: Convertor.to_int(value),
            'current_work_contract_since': lambda value: Convertor.to_date(value),
            'current_work_contract_to': lambda value: Convertor.to_date(value),
            'gross_annual_income': lambda value: Convertor.to_decimal(value),
            'is_in_trial_period': lambda value: Convertor.to_bool(value),
            'is_in_probationary_period': lambda value: Convertor.to_bool(value),
        }

JSON_KEYS = ['jobDescription', 'firstWorkContractSince', 'employmentTypeId', 'currentWorkContractSince', 'currentWorkContractTo', 'grossAnnualIncome', 'isInTrialPeriod', 'isInProbationaryPeriod']

class IncomeDataJob(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @staticmethod
    def from_json(js_dict: dict):
        return IncomeDataJob(js_dict = js_dict)

    @staticmethod
    def from_json_list(js_dict_list: List[dict]):
        return list(map(lambda i: IncomeDataJob.from_json(i), js_dict_list))

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, IncomeDataJob)
        return value.to_grpc()

    def to_grpc(self) -> dict:
       return dict()

    def _get_json_keys(self):
        return JSON_KEYS

# --------------------------------------------------------------------------------------------
                            # "job": {
                            #     "jobDescription": null,
                            #     "firstWorkContractSince": null,
                            #     "employmentTypeId": null,
                            #     "currentWorkContractSince": null,
                            #     "currentWorkContractTo": null,
                            #     "grossAnnualIncome": null,
                            #     "isInTrialPeriod": false,
                            #     "isInProbationaryPeriod": false
                            # },
# --------------------------------------------------------------------------------------------