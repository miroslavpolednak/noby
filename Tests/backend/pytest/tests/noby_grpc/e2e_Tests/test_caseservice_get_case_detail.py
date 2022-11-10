import sys
from os.path import dirname
import importlib.util 

import grpc
import ssl


#Import Stubs for CIS Types
sys.path.append(dirname(__file__) + '/../../../grpc/CisTypes')

#Import Stubs for Domain Services, fell free to append any needed
sys.path.append(dirname(__file__) + '/../../../grpc/CaseService')

#Import Stub for Service. This way is necessary because of dot in the name of file
spec = importlib.util.spec_from_file_location(
    name="CaseService",
    location="../../../grpc/CaseService/CaseService.v1_pb2_grpc.py",
)
CaseService = importlib.util.module_from_spec(spec)
spec.loader.exec_module(CaseService)
CaseServiceStub = CaseService.CaseServiceStub

#Import stub for method
import GetCaseDetail_pb2 as GetCaseDetail


#Basic Auth
class AuthGateway(grpc.AuthMetadataPlugin):
    def __call__(self, context, callback):
        callback((('authorization', 'Basic YTph'),), None)

def test_run():
    authCredentials=grpc.metadata_call_credentials(
        AuthGateway(), name='BasicAuth')
    #override cert target domain
    cert_cn = "adpra191.vsskb.cz"
    options = (('grpc.ssl_target_name_override', cert_cn,),)
    #get cert from server - we trust servers certificate
    channelCredentials=grpc.ssl_channel_credentials(ssl.get_server_certificate(('adpra191.vsskb.cz', 31001)).encode('utf-8'))
    #add both auth and cert into one object - secure_channel can only accept one parameter of this kind, so combined one should be provided
    compositeCredentials = grpc.composite_channel_credentials(
        channelCredentials,
        authCredentials,
    )

    #create channel
    with grpc.secure_channel('adpra191:31001', compositeCredentials, options) as channel:
            stub = CaseServiceStub(channel)
            req = GetCaseDetail.GetCaseDetailRequest(CaseId=2975970)
            case = stub.GetCaseDetail(req)
            print(case)
            print(case.Customer)

            assert case.Customer.FirstNameNaturalPerson == "klient"
            assert case.CaseOwner.userName == "Filip Tůma"

            req = GetCaseDetail.GetCaseDetailRequest(CaseId=2975950)
            case = stub.GetCaseDetail(req)
            print(case)
            print(case.Customer)

            assert case.Customer.FirstNameNaturalPerson == "Business"
            assert case.CaseOwner.userName == "Jan Novák"

