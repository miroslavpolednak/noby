import sys
from os.path import dirname
import importlib.util 

import grpc
import ssl

from decimal import Decimal

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

from OfferIdRequest_pb2 import OfferIdRequest

from Mortgage.SimulateMortgage_pb2 import SimulateMortgageRequest, SimulateMortgageResponse
from Mortgage.MortgageSimulationInputs_pb2 import MortgageSimulationInputs
from BasicParameters_pb2 import BasicParameters
from Types_pb2 import SimulationResultWarning, LoanPurpose, FeeSettings, InputMarketingAction, ResultMarketingAction, InputFee, ResultFee, Developer, PaymentScheduleSimple, RiskLifeInsurance, RealEstateInsurance
from GrpcDecimal_pb2 import GrpcDecimal
from GrpcDate_pb2 import GrpcDate
from NullableGrpcDate_pb2 import NullableGrpcDate

from datetime import datetime

from google.protobuf.wrappers_pb2 import Int32Value


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


def create_offer_id_request(offer_id:int):
    req = OfferIdRequest(OfferId=offer_id)
    return req


# GET OFFER
def test_get_offer(offer_id:int):
    req = create_offer_id_request(offer_id)
    offer = call_service_endpoint('GetOffer', req)
    print(offer)

    # assert it
    assert (offer is not None), f"Empty response!"
    assert (offer.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER 
def test_get_mortgage_offer(offer_id:int):
    req = create_offer_id_request(offer_id)
    mortgage = call_service_endpoint('GetMortgageOffer', req)
    print(mortgage)

    # assert it
    assert (mortgage is not None), f"Empty response!"
    assert (mortgage.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER DETAIL
def test_get_mortgage_offer_detail(offer_id:int):
    req = create_offer_id_request(offer_id)
    mortgage_detail = call_service_endpoint('GetMortgageOfferDetail', req)
    print(mortgage_detail)

    # assert it
    assert (mortgage_detail is not None), f"Empty response!"
    assert (mortgage_detail.OfferId == offer_id), f"Response with incorrect OfferId!"


# GET MORTGAGE OFFER FPSCHEDULE
def test_get_mortgage_offer_fpschedule(offer_id:int):
    req = create_offer_id_request(offer_id)
    mortgage_schedule = call_service_endpoint('GetMortgageOfferFPSchedule', req)
    print(mortgage_schedule)

    # assert it
    assert (mortgage_schedule is not None), f"Empty response!"
    assert (mortgage_schedule.OfferId == offer_id), f"Response with incorrect OfferId!"


def to_grpc_decimal(value):
    return GrpcDecimal(units= value, nanos=0)

def to_grpc_int(value)->Int32Value:
    return Int32Value(value= value)

def to_grpc_date(value: datetime)->GrpcDate:
    return GrpcDate(year = value.year, month = value.month, day = value.day)

def to_nullable_grpc_date(value: datetime)->GrpcDate:
    return NullableGrpcDate(year = value.year, month = value.month, day = value.day)


def build_simulation_inputs()->SimulateMortgageRequest:

    def build_basic_parameters()->BasicParameters:
        basic_parameters = BasicParameters(
            # cis.types.NullableGrpcDecimal FinancialResourcesOwn  = 1;
            # cis.types.NullableGrpcDecimal FinancialResourcesOther  = 2;
            # cis.types.NullableGrpcDate GuaranteeDateTo = 3;
            # google.protobuf.Int32Value StatementTypeId = 4;
        )
        return basic_parameters

    def build_simulation_inputs()->MortgageSimulationInputs:

        loan_purposes = [
            LoanPurpose(LoanPurposeId=201, Sum=to_grpc_decimal(1000000)),
            LoanPurpose(LoanPurposeId=202, Sum=to_grpc_decimal(2000000)),
            ]

        marketing_actions = InputMarketingAction(
            Domicile = True,
            HealthRiskInsurance = True,
            RealEstateInsurance = True,
            IncomeLoanRatioDiscount = True,
            UserVip = True,
        )

        simulation_inputs = MortgageSimulationInputs(
            ExpectedDateOfDrawing = to_nullable_grpc_date(datetime(2022, 12, 7)),
            ProductTypeId = 20001,
            LoanKindId = 2000,
            LoanAmount = to_grpc_decimal(3000000),
            LoanDuration = to_grpc_int(36),
            GuaranteeDateFrom = to_grpc_date(datetime(2022, 12, 6)),
            #InterestRateDiscount = to_grpc_decimal(1),
            FixedRatePeriod = to_grpc_int(24),
            CollateralAmount = to_grpc_decimal(6500000),
            # DrawingTypeId =
            # DrawingDurationId = 
            # PaymentDay = 
            # IsEmployeeBonusRequested =
            # Developer =
            LoanPurposes = loan_purposes,
            # FeeSettings =
            MarketingActions = marketing_actions
            # Fees =
            # RiskLifeInsurance =
            # RealEstateInsurance =

            # -----------------------------

            # cis.types.NullableGrpcDate ExpectedDateOfDrawing = 1;
            # int32 ProductTypeId = 2;
            # int32 LoanKindId = 3;
            # cis.types.GrpcDecimal LoanAmount = 4;
            # google.protobuf.Int32Value LoanDuration = 5;			//nullable kvůli validaci na NotNull
            # cis.types.GrpcDate GuaranteeDateFrom = 6;
            # cis.types.NullableGrpcDecimal InterestRateDiscount = 7;
            # google.protobuf.Int32Value FixedRatePeriod = 8;			//nullable kvůli validaci na NotNull
            # cis.types.GrpcDecimal CollateralAmount = 9;
            # google.protobuf.Int32Value DrawingTypeId = 10; 
            # google.protobuf.Int32Value DrawingDurationId = 11;
            # google.protobuf.Int32Value PaymentDay = 12;
            # google.protobuf.BoolValue IsEmployeeBonusRequested = 13;
            # Developer Developer = 14;
            # repeated LoanPurpose LoanPurposes = 15;
            # FeeSettings FeeSettings = 16;
            # InputMarketingAction MarketingActions = 17;
            # repeated InputFee Fees = 18;
            # RiskLifeInsurance RiskLifeInsurance = 19;
            # RealEstateInsurance RealEstateInsurance = 20;
        )

        return simulation_inputs

    basic_parameters = build_basic_parameters()
    simulation_inputs = build_simulation_inputs()

    req = SimulateMortgageRequest(
        ResourceProcessId = '4D115798-0E05-4CF0-8A5A-1A3F871B3727',
        BasicParameters = basic_parameters,
        SimulationInputs = simulation_inputs
    )

    return req
    

# SIMULATE MORTGAGE
def test_simulate_mortgage(req :SimulateMortgageRequest):
    simulation = call_service_endpoint('SimulateMortgage', req)
    print(simulation)


""" 
test_get_offer(1)
test_get_mortgage_offer(1)
test_get_mortgage_offer_detail(1)
test_get_mortgage_offer_fpschedule(1)
"""

test_get_offer(1)

simulation_inputs:SimulateMortgageRequest = build_simulation_inputs()
test_simulate_mortgage(simulation_inputs)
