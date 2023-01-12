import grpc
import ssl

from common import config, EService, EServiceType

#Basic Auth
class AuthGateway(grpc.AuthMetadataPlugin):
    def __call__(self, context, callback):
        callback((('authorization', 'Basic YTph'),), None)

class DomainService():

    def __init__(self, service: EService, service_stub):
        self._grpc_url = config.get_service_url(service, EServiceType.GRPC)
        self._service_stub = service_stub

    @property
    def grpc_url(self) -> str:
        return self._grpc_url

    def get_composite_credentials(self, port: int):
        authCredentials=grpc.metadata_call_credentials(AuthGateway(), name='BasicAuth')
        #get cert from server - we trust servers certificate
        channelCredentials=grpc.ssl_channel_credentials(ssl.get_server_certificate((config.server, port)).encode('utf-8'))

        #add both auth and cert into one object - secure_channel can only accept one parameter of this kind, so combined one should be provided
        composite_credentials = grpc.composite_channel_credentials(channelCredentials,authCredentials)
        return composite_credentials

    def get_options(self):
        #override cert target domain
        cert_cn = config.server #"adpra191.vsskb.cz"
        options = (('grpc.ssl_target_name_override', cert_cn,),)
        return options

    # calls endpoint of service by name
    def call(self, endpoint_name: str, req: any) -> object:

        port = self._grpc_url[-5:]
        target = f'{config.server}:{port}'
        #print(f'target: {target} !')

        credentials = self.get_composite_credentials(port)
        options = self.get_options()
    
        #create channel
        with grpc.secure_channel(target, credentials, options) as channel:
            stub = self._service_stub(channel)
            func = getattr(stub, endpoint_name)
            res = func(req)
            return res
