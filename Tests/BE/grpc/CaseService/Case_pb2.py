# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: Case.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


import UserInfo_pb2 as UserInfo__pb2
import CustomerData_pb2 as CustomerData__pb2
import OfferContacts_pb2 as OfferContacts__pb2
import CaseData_pb2 as CaseData__pb2
import GrpcDateTime_pb2 as GrpcDateTime__pb2
import ModificationStamp_pb2 as ModificationStamp__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\nCase.proto\x12\x1a\x44omainServices.CaseService\x1a\x0eUserInfo.proto\x1a\x12\x43ustomerData.proto\x1a\x13OfferContacts.proto\x1a\x0e\x43\x61seData.proto\x1a\x12GrpcDateTime.proto\x1a\x17ModificationStamp.proto\"\x96\x03\n\x04\x43\x61se\x12\x0e\n\x06\x43\x61seId\x18\x01 \x01(\x03\x12\x32\n\x04\x44\x61ta\x18\x02 \x01(\x0b\x32$.DomainServices.CaseService.CaseData\x12\r\n\x05State\x18\x03 \x01(\x05\x12/\n\x0eStateUpdatedOn\x18\x04 \x01(\x0b\x32\x17.cis.types.GrpcDateTime\x12:\n\x08\x43ustomer\x18\x06 \x01(\x0b\x32(.DomainServices.CaseService.CustomerData\x12@\n\rOfferContacts\x18\n \x01(\x0b\x32).DomainServices.CaseService.OfferContacts\x12&\n\tCaseOwner\x18\x07 \x01(\x0b\x32\x13.cis.types.UserInfo\x12-\n\x07\x43reated\x18\x08 \x01(\x0b\x32\x1c.cis.types.ModificationStamp\x12\x35\n\x05Tasks\x18\t \x03(\x0b\x32&.DomainServices.CaseService.ActiveTask\"3\n\nActiveTask\x12\x15\n\rTaskProcessId\x18\x01 \x01(\x05\x12\x0e\n\x06TypeId\x18\x02 \x01(\x05\x42\'\xaa\x02$DomainServices.CaseService.Contractsb\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'Case_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\252\002$DomainServices.CaseService.Contracts'
  _CASE._serialized_start=161
  _CASE._serialized_end=567
  _ACTIVETASK._serialized_start=569
  _ACTIVETASK._serialized_end=620
# @@protoc_insertion_point(module_scope)