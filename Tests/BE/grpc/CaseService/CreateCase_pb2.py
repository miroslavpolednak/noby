# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: CreateCase.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


import CustomerData_pb2 as CustomerData__pb2
import CaseData_pb2 as CaseData__pb2
import OfferContacts_pb2 as OfferContacts__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x10\x43reateCase.proto\x12\x1a\x44omainServices.CaseService\x1a\x12\x43ustomerData.proto\x1a\x0e\x43\x61seData.proto\x1a\x13OfferContacts.proto\"\xde\x01\n\x11\x43reateCaseRequest\x12:\n\x08\x43ustomer\x18\x01 \x01(\x0b\x32(.DomainServices.CaseService.CustomerData\x12\x17\n\x0f\x43\x61seOwnerUserId\x18\x02 \x01(\x05\x12\x32\n\x04\x44\x61ta\x18\x03 \x01(\x0b\x32$.DomainServices.CaseService.CaseData\x12@\n\rOfferContacts\x18\x04 \x01(\x0b\x32).DomainServices.CaseService.OfferContacts\"$\n\x12\x43reateCaseResponse\x12\x0e\n\x06\x43\x61seId\x18\x01 \x01(\x03\x42\'\xaa\x02$DomainServices.CaseService.Contractsb\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'CreateCase_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\252\002$DomainServices.CaseService.Contracts'
  _CREATECASEREQUEST._serialized_start=106
  _CREATECASEREQUEST._serialized_end=328
  _CREATECASERESPONSE._serialized_start=330
  _CREATECASERESPONSE._serialized_end=366
# @@protoc_insertion_point(module_scope)
