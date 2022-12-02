import sys
from os.path import dirname
import importlib.util 

import grpc
import ssl

PATH_TO_GRPC:str = 'D:/Users/992466q/source/repos/OneSolution/Tests/backend/pytest/grpc'
SERVER_CERTIFICATE_ADDR:tuple[str, int] = ('adpra191.vsskb.cz', 30001)
CHANNEL_TARGET = 'adpra191:30006'

#Import Stubs for CIS Types
sys.path.append(f'{PATH_TO_GRPC}/CisTypes')

#Import Stubs for Domain Service
sys.path.append(f'{PATH_TO_GRPC}/OfferService')

#Import Stub for Service. This way is necessary because of dot in the name of file
spec = importlib.util.spec_from_file_location(name="OfferService", location=f'{PATH_TO_GRPC}/OfferService/OfferService.v1_pb2_grpc.py')

OfferService = importlib.util.module_from_spec(spec)
spec.loader.exec_module(OfferService)
OfferServiceStub = OfferService.OfferServiceStub


#Basic Auth
class AuthGateway(grpc.AuthMetadataPlugin):
    def __call__(self, context, callback):
        callback((('authorization', 'Basic YTph'),), None)


def get_composite_credentials():
    authCredentials=grpc.metadata_call_credentials(AuthGateway(), name='BasicAuth')
    #get cert from server - we trust servers certificate
    channelCredentials=grpc.ssl_channel_credentials(ssl.get_server_certificate(SERVER_CERTIFICATE_ADDR).encode('utf-8'))
    #add both auth and cert into one object - secure_channel can only accept one parameter of this kind, so combined one should be provided
    composite_credentials = grpc.composite_channel_credentials(
        channelCredentials,
        authCredentials,
    )
    return composite_credentials

def get_options():
    #override cert target domain
    cert_cn = SERVER_CERTIFICATE_ADDR[0] #"adpra191.vsskb.cz"
    options = (('grpc.ssl_target_name_override', cert_cn,),)
    return options


# calls endpoint of service by name
def call_service_endpoint(endpoint_name: str, req: any) -> object:

    credentials = get_composite_credentials()
    options = get_options()
   
    #create channel
    with grpc.secure_channel(CHANNEL_TARGET, credentials, options) as channel:
        stub = OfferServiceStub(channel)
        func = getattr(stub, endpoint_name)
        res = func(req)
        return res


# GET OFFER
def test_get_offer(offer_id:int):

    import GetOffer_pb2 as GetOffer # Import stub for endpoint

    req = GetOffer.GetOfferResponse(OfferId=offer_id)
    offer = call_service_endpoint('GetOffer', req)
    print(offer)

    # assert it
    assert (offer is not None), f"Empty response!"
    assert (offer.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER 
def test_get_mortgage_offer(offer_id:int):

    import Mortgage.GetMortgageOffer_pb2 as GetMortgageOffer # Import stub for endpoint

    req = GetMortgageOffer.GetMortgageOfferResponse(OfferId=offer_id)
    mortgage = call_service_endpoint('GetMortgageOffer', req)
    print(mortgage)

    # assert it
    assert (mortgage is not None), f"Empty response!"
    assert (mortgage.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER DETAIL
def test_get_mortgage_offer_detail(offer_id:int):

    import Mortgage.GetMortgageOfferDetail_pb2 as GetMortgageOfferDetail # Import stub for endpoint

    req = GetMortgageOfferDetail.GetMortgageOfferDetailResponse(OfferId=offer_id)
    mortgage_detail = call_service_endpoint('GetMortgageOfferDetail', req)
    print(mortgage_detail)

    # assert it
    assert (mortgage_detail is not None), f"Empty response!"
    assert (mortgage_detail.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER FPSCHEDULE
def test_get_mortgage_offer_fpschedule(offer_id:int):

    import Mortgage.GetMortgageOfferFPSchedule_pb2 as GetMortgageOfferFPSchedule # Import stub for endpoint

    # TODO: Protocol message GetMortgageOfferFPScheduleResponse has no "OfferId" field.
    req = GetMortgageOfferFPSchedule.GetMortgageOfferFPScheduleResponse(OfferId=offer_id)
    mortgage_schedule = call_service_endpoint('GetMortgageOfferFPSchedule', req)
    print(mortgage_schedule)

    # assert it
    assert (mortgage_schedule is not None), f"Empty response!"
    assert (mortgage_schedule.OfferId == offer_id), f"Response with incorrect OfferId!"

    #rpc GetMortgageOfferFPSchedule (DomainServices.OfferService.OfferIdRequest) returns (DomainServices.OfferService.GetMortgageOfferFPScheduleResponse);


test_get_offer(1)
test_get_mortgage_offer(1)
test_get_mortgage_offer_detail(1)
test_get_mortgage_offer_fpschedule(1)
