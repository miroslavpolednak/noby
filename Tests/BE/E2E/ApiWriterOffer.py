from typing import List

from .EProcessKey import EProcessKey
from .Processing import Processing

from business.offer import Offer

from fe_api import FeAPI
from common import Log

class ApiWriterOffer():

    def __init__(self, offer: Offer):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')

        self.__offer = offer
        self.__offer_json = offer.to_json_value()


    def build(self) -> int | Exception:

        try:
            self.__create_offer()
        except Exception as e:
            return e

        return Processing.get_process_key(self.__offer_json, EProcessKey.OFFER_ID)


    def __create_offer(self):

        req = self.__offer_json

        # call FE API endpoint 
        res = FeAPI.Offer.simulate_mortgage(req)
        assert isinstance(res, dict), f'Invalid response [{res}]'

        # set process data
        offer_id: int = res['offerId']
        Processing.set_process_key(self.__offer_json, EProcessKey.OFFER_ID, offer_id)
