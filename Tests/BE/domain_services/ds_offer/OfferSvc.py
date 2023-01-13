from common import config, EService, EServiceType
from domain_services.DomainService import DomainService

from OfferService_v1_pb2_grpc import OfferServiceStub # file renamed 'OfferService.v1_pb2_grpc.py' -> 'OfferService_v1_pb2_grpc.py'

from OfferIdRequest_pb2 import OfferIdRequest
from Mortgage.SimulateMortgage_pb2 import SimulateMortgageRequest, SimulateMortgageResponse


class OfferSvc(DomainService):

    def __init__(self):
        #super().__init__(EService.DS_Offer, OfferService.OfferServiceStub)
        super().__init__(EService.DS_Offer, OfferServiceStub)
        #self._grpc_url = config.get_service_url(EService.DS_Offer, EServiceType.GRPC)

    
    def print(self):
        print('OfferSvc')

    def create_offer_id_request(self, offer_id:int):
        req = OfferIdRequest(OfferId=offer_id)
        return req

    # get offer
    def get_offer(self, offer_id: int):
        req = self.create_offer_id_request(offer_id)
        res = self.call('GetOffer', req)
        return res

    # get mortgage offer
    def get_mortgage_offer(self, offer_id:int):
        req = self.create_offer_id_request(offer_id)
        res = self.call('GetMortgageOffer', req)
        return res

    # get mortgage offer detail
    def get_mortgage_offer_detail(self, offer_id:int):
        req = self.create_offer_id_request(offer_id)
        res = self.call('GetMortgageOfferDetail', req)
        return res

    # get mortgage offer full payment schedule
    def get_mortgage_offer_full_payment_schedule(self, offer_id:int):
        req = self.create_offer_id_request(offer_id)
        res = self.call('GetMortgageOfferFPSchedule', req)
        return res

    # simulate mortgage
    def simulate_mortgage(self, req: SimulateMortgageRequest)->SimulateMortgageResponse:
        res = self.call('SimulateMortgage', req)
        return res
