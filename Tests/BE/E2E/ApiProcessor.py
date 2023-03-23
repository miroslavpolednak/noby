import json

from typing import List

from .Processing import Processing

from common import Log
from DATA import JsonDataModificator

from business.offer import Offer
from business.case import Case

from .ApiWriterOffer import ApiWriterOffer
from .ApiWriterCase import ApiWriterCase

class ApiProcessor():

    def __init__(self, data_json: dict | str):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')

        if isinstance(data_json, str):
            self.__offer_case_json = json.loads(data_json)
        elif isinstance(data_json, dict):
            self.__offer_case_json = data_json
        else:
            raise 'ApiProcessor.init - Invalid data json !'


    @staticmethod
    def process_list(offer_case_jsons: List[dict]) -> List[dict]:

        results: List[dict] = []

        for json in offer_case_jsons:
            r: dict = ApiProcessor(json).process()
            results.append(r)

        return results

    def process(self) -> dict | Exception:

        result: dict = None

        try:
            result = self.__process()
        except Exception as e:
            result = e

        return result

    def __process(self) -> dict:

        offer_json: dict = Processing.get_key(self.__offer_case_json, 'offer')
        case_json: dict = Processing.get_key(self.__offer_case_json, 'case')

        # -------------------------------------------------
        # resolve modifications
        # -------------------------------------------------
        offer_json = JsonDataModificator.modify(offer_json)
        case_json = JsonDataModificator.modify(case_json)
        # -------------------------------------------------

        offer_id: int = None
        case_id: int = None

        if (offer_json is not None):
            offer = Offer.from_json(offer_json)
            build_result = ApiWriterOffer(offer).build()

            if isinstance(build_result, Exception):
                raise build_result
            
            offer_id = build_result

        if (case_json is not None):
            case = Case.from_json(case_json)

            build_result = ApiWriterCase(case, offer_id).build()

            if isinstance(build_result, Exception):
                raise build_result
            
            case_id = build_result


        return dict(
            offer_id = offer_id,
            case_id = case_id,
        )

    # def read_case(case_id) -> Case | Exception:

    #     result: Case | Exception = None

    #     try:
    #         result = ApiCaseLoader().load(case_id)
    #     except Exception as e:
    #         result = e

    #     return result


# ----------------------------------------------------------------------------------------------------------------------------------------------
# SCORING
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/sales-arrangement/337/loan-application-assessment?newAssessmentRequired=true
# {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"SalesArrangement.RiskBusinessCaseId not defined.","status":400}


# ----------------------------------------------------------------------------------------------------------------------------------------------