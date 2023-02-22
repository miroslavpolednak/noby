from typing import List

from .EProcessKey import EProcessKey
from .Processing import Processing

from business.offer import Offer
from business.case import Case

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI
from common import Log

class ApiProcessor():

    def __init__(self, case: Case):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')

        self.__case = case
        self.__case_json = case.to_json_value()


    @staticmethod
    def process_list(cases: List[Case]) -> List[dict]:

        results: List[dict] = []

        for case in cases:
            r: dict = ApiProcessor(case).process()
            results.append(r)

        return results


    def __create_offer(self):

        offer_json: dict = Processing.get_key(self.__case_json, 'offer')
        assert offer_json is not None, 'Offer data not provided!'

        req = offer_json

        # call FE API endpoint 
        self.__log.debug(f'process_offer.req [{req}]')
        res = FeAPI.Offer.simulate_mortgage(req)
        self.__log.debug(f'process_offer.res [{res}]')

        # set process data
        offer_id: int = res['offerId']
        Processing.set_process_key(offer_json, EProcessKey.OFFER_ID, offer_id)


    def process(self) -> dict:

        offer_id: int = self.__create_offer()

        offer_json: dict = Processing.get_key(self.__case_json, 'offer')
        offer_id = Processing.get_process_key(offer_json, EProcessKey.OFFER_ID) 

        return dict(
            offer_id = offer_id,
        )
