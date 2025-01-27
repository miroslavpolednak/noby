# Generated by the gRPC Python protocol compiler plugin. DO NOT EDIT!
"""Client and server classes corresponding to protobuf-defined services."""
import grpc

import GetOffer_pb2 as GetOffer__pb2
from Mortgage import GetMortgageOfferDetail_pb2 as Mortgage_dot_GetMortgageOfferDetail__pb2
from Mortgage import GetMortgageOfferFPSchedule_pb2 as Mortgage_dot_GetMortgageOfferFPSchedule__pb2
from Mortgage import GetMortgageOffer_pb2 as Mortgage_dot_GetMortgageOffer__pb2
from Mortgage import SimulateMortgage_pb2 as Mortgage_dot_SimulateMortgage__pb2
import OfferIdRequest_pb2 as OfferIdRequest__pb2


class OfferServiceStub(object):
    """Missing associated documentation comment in .proto file."""

    def __init__(self, channel):
        """Constructor.

        Args:
            channel: A grpc.Channel.
        """
        self.GetOffer = channel.unary_unary(
                '/DomainServices.OfferService.v1.OfferService/GetOffer',
                request_serializer=OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
                response_deserializer=GetOffer__pb2.GetOfferResponse.FromString,
                )
        self.GetMortgageOffer = channel.unary_unary(
                '/DomainServices.OfferService.v1.OfferService/GetMortgageOffer',
                request_serializer=OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
                response_deserializer=Mortgage_dot_GetMortgageOffer__pb2.GetMortgageOfferResponse.FromString,
                )
        self.GetMortgageOfferDetail = channel.unary_unary(
                '/DomainServices.OfferService.v1.OfferService/GetMortgageOfferDetail',
                request_serializer=OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
                response_deserializer=Mortgage_dot_GetMortgageOfferDetail__pb2.GetMortgageOfferDetailResponse.FromString,
                )
        self.SimulateMortgage = channel.unary_unary(
                '/DomainServices.OfferService.v1.OfferService/SimulateMortgage',
                request_serializer=Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageRequest.SerializeToString,
                response_deserializer=Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageResponse.FromString,
                )
        self.GetMortgageOfferFPSchedule = channel.unary_unary(
                '/DomainServices.OfferService.v1.OfferService/GetMortgageOfferFPSchedule',
                request_serializer=OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
                response_deserializer=Mortgage_dot_GetMortgageOfferFPSchedule__pb2.GetMortgageOfferFPScheduleResponse.FromString,
                )


class OfferServiceServicer(object):
    """Missing associated documentation comment in .proto file."""

    def GetOffer(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def GetMortgageOffer(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def GetMortgageOfferDetail(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def SimulateMortgage(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def GetMortgageOfferFPSchedule(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')


def add_OfferServiceServicer_to_server(servicer, server):
    rpc_method_handlers = {
            'GetOffer': grpc.unary_unary_rpc_method_handler(
                    servicer.GetOffer,
                    request_deserializer=OfferIdRequest__pb2.OfferIdRequest.FromString,
                    response_serializer=GetOffer__pb2.GetOfferResponse.SerializeToString,
            ),
            'GetMortgageOffer': grpc.unary_unary_rpc_method_handler(
                    servicer.GetMortgageOffer,
                    request_deserializer=OfferIdRequest__pb2.OfferIdRequest.FromString,
                    response_serializer=Mortgage_dot_GetMortgageOffer__pb2.GetMortgageOfferResponse.SerializeToString,
            ),
            'GetMortgageOfferDetail': grpc.unary_unary_rpc_method_handler(
                    servicer.GetMortgageOfferDetail,
                    request_deserializer=OfferIdRequest__pb2.OfferIdRequest.FromString,
                    response_serializer=Mortgage_dot_GetMortgageOfferDetail__pb2.GetMortgageOfferDetailResponse.SerializeToString,
            ),
            'SimulateMortgage': grpc.unary_unary_rpc_method_handler(
                    servicer.SimulateMortgage,
                    request_deserializer=Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageRequest.FromString,
                    response_serializer=Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageResponse.SerializeToString,
            ),
            'GetMortgageOfferFPSchedule': grpc.unary_unary_rpc_method_handler(
                    servicer.GetMortgageOfferFPSchedule,
                    request_deserializer=OfferIdRequest__pb2.OfferIdRequest.FromString,
                    response_serializer=Mortgage_dot_GetMortgageOfferFPSchedule__pb2.GetMortgageOfferFPScheduleResponse.SerializeToString,
            ),
    }
    generic_handler = grpc.method_handlers_generic_handler(
            'DomainServices.OfferService.v1.OfferService', rpc_method_handlers)
    server.add_generic_rpc_handlers((generic_handler,))


 # This class is part of an EXPERIMENTAL API.
class OfferService(object):
    """Missing associated documentation comment in .proto file."""

    @staticmethod
    def GetOffer(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/DomainServices.OfferService.v1.OfferService/GetOffer',
            OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
            GetOffer__pb2.GetOfferResponse.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def GetMortgageOffer(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/DomainServices.OfferService.v1.OfferService/GetMortgageOffer',
            OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
            Mortgage_dot_GetMortgageOffer__pb2.GetMortgageOfferResponse.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def GetMortgageOfferDetail(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/DomainServices.OfferService.v1.OfferService/GetMortgageOfferDetail',
            OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
            Mortgage_dot_GetMortgageOfferDetail__pb2.GetMortgageOfferDetailResponse.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def SimulateMortgage(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/DomainServices.OfferService.v1.OfferService/SimulateMortgage',
            Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageRequest.SerializeToString,
            Mortgage_dot_SimulateMortgage__pb2.SimulateMortgageResponse.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)

    @staticmethod
    def GetMortgageOfferFPSchedule(request,
            target,
            options=(),
            channel_credentials=None,
            call_credentials=None,
            insecure=False,
            compression=None,
            wait_for_ready=None,
            timeout=None,
            metadata=None):
        return grpc.experimental.unary_unary(request, target, '/DomainServices.OfferService.v1.OfferService/GetMortgageOfferFPSchedule',
            OfferIdRequest__pb2.OfferIdRequest.SerializeToString,
            Mortgage_dot_GetMortgageOfferFPSchedule__pb2.GetMortgageOfferFPScheduleResponse.FromString,
            options, channel_credentials,
            insecure, call_credentials, compression, wait_for_ready, timeout, metadata)
