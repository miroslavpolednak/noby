# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: OfferService.v1.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


import OfferIdRequest_pb2 as OfferIdRequest__pb2
import GetOffer_pb2 as GetOffer__pb2
from Mortgage import GetMortgageOffer_pb2 as Mortgage_dot_GetMortgageOffer__pb2
from Mortgage import GetMortgageOfferDetail_pb2 as Mortgage_dot_GetMortgageOfferDetail__pb2
from Mortgage import SimulateMortgage_pb2 as Mortgage_dot_SimulateMortgage__pb2
from Mortgage import GetMortgageOfferFPSchedule_pb2 as Mortgage_dot_GetMortgageOfferFPSchedule__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x15OfferService.v1.proto\x12\x1e\x44omainServices.OfferService.v1\x1a\x14OfferIdRequest.proto\x1a\x0eGetOffer.proto\x1a\x1fMortgage/GetMortgageOffer.proto\x1a%Mortgage/GetMortgageOfferDetail.proto\x1a\x1fMortgage/SimulateMortgage.proto\x1a)Mortgage/GetMortgageOfferFPSchedule.proto2\x81\x05\n\x0cOfferService\x12\x66\n\x08GetOffer\x12+.DomainServices.OfferService.OfferIdRequest\x1a-.DomainServices.OfferService.GetOfferResponse\x12v\n\x10GetMortgageOffer\x12+.DomainServices.OfferService.OfferIdRequest\x1a\x35.DomainServices.OfferService.GetMortgageOfferResponse\x12\x82\x01\n\x16GetMortgageOfferDetail\x12+.DomainServices.OfferService.OfferIdRequest\x1a;.DomainServices.OfferService.GetMortgageOfferDetailResponse\x12\x7f\n\x10SimulateMortgage\x12\x34.DomainServices.OfferService.SimulateMortgageRequest\x1a\x35.DomainServices.OfferService.SimulateMortgageResponse\x12\x8a\x01\n\x1aGetMortgageOfferFPSchedule\x12+.DomainServices.OfferService.OfferIdRequest\x1a?.DomainServices.OfferService.GetMortgageOfferFPScheduleResponseB+\xaa\x02(DomainServices.OfferService.Contracts.v1b\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'OfferService.v1_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\252\002(DomainServices.OfferService.Contracts.v1'
  _OFFERSERVICE._serialized_start=244
  _OFFERSERVICE._serialized_end=885
# @@protoc_insertion_point(module_scope)