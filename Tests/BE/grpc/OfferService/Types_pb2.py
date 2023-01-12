# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: Types.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from google.protobuf import wrappers_pb2 as google_dot_protobuf_dot_wrappers__pb2
import GrpcDecimal_pb2 as GrpcDecimal__pb2
import NullableGrpcDecimal_pb2 as NullableGrpcDecimal__pb2
import NullableGrpcDate_pb2 as NullableGrpcDate__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x0bTypes.proto\x12\x1b\x44omainServices.OfferService\x1a\x1egoogle/protobuf/wrappers.proto\x1a\x11GrpcDecimal.proto\x1a\x19NullableGrpcDecimal.proto\x1a\x16NullableGrpcDate.proto\"\xbc\x01\n\x17SimulationResultWarning\x12\x30\n\x0bWarningCode\x18\x01 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x31\n\x0bWarningText\x18\x02 \x01(\x0b\x32\x1c.google.protobuf.StringValue\x12<\n\x16WarningInternalMessage\x18\x03 \x01(\x0b\x32\x1c.google.protobuf.StringValue\"I\n\x0bLoanPurpose\x12\x15\n\rLoanPurposeId\x18\x01 \x01(\x05\x12#\n\x03Sum\x18\x02 \x01(\x0b\x32\x16.cis.types.GrpcDecimal\"`\n\x0b\x46\x65\x65Settings\x12\x35\n\x10\x46\x65\x65TariffPurpose\x18\x01 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x1a\n\x12IsStatementCharged\x18\x02 \x01(\x08\"\x94\x01\n\x14InputMarketingAction\x12\x10\n\x08\x44omicile\x18\x01 \x01(\x08\x12\x1b\n\x13HealthRiskInsurance\x18\x02 \x01(\x08\x12\x1b\n\x13RealEstateInsurance\x18\x03 \x01(\x08\x12\x1f\n\x17IncomeLoanRatioDiscount\x18\x04 \x01(\x08\x12\x0f\n\x07UserVip\x18\x05 \x01(\x08\"\xdf\x01\n\x15ResultMarketingAction\x12\x0c\n\x04\x43ode\x18\x01 \x01(\t\x12\x11\n\tRequested\x18\x02 \x01(\x05\x12,\n\x07\x41pplied\x18\x03 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x36\n\x11MarketingActionId\x18\x04 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x31\n\tDeviation\x18\x05 \x01(\x0b\x32\x1e.cis.types.NullableGrpcDecimal\x12\x0c\n\x04Name\x18\x06 \x01(\t\"M\n\x08InputFee\x12\r\n\x05\x46\x65\x65Id\x18\x01 \x01(\x05\x12\x32\n\x12\x44iscountPercentage\x18\x02 \x01(\x0b\x32\x16.cis.types.GrpcDecimal\"\xa1\x04\n\tResultFee\x12\r\n\x05\x46\x65\x65Id\x18\x01 \x01(\x05\x12\x32\n\x12\x44iscountPercentage\x18\x02 \x01(\x0b\x32\x16.cis.types.GrpcDecimal\x12\x31\n\tTariffSum\x18\x03 \x01(\x0b\x32\x1e.cis.types.NullableGrpcDecimal\x12\x33\n\x0b\x43omposedSum\x18\x04 \x01(\x0b\x32\x1e.cis.types.NullableGrpcDecimal\x12\x30\n\x08\x46inalSum\x18\x05 \x01(\x0b\x32\x1e.cis.types.NullableGrpcDecimal\x12\x36\n\x11MarketingActionId\x18\x06 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x0c\n\x04Name\x18\x07 \x01(\t\x12\x1b\n\x13ShortNameForExample\x18\x08 \x01(\t\x12\x12\n\nTariffName\x18\t \x01(\t\x12\x11\n\tUsageText\x18\n \x01(\t\x12\x1c\n\x14TariffTextWithAmount\x18\x0b \x01(\t\x12\x0e\n\x06\x43odeKB\x18\x0c \x01(\t\x12\x1d\n\x15\x44isplayAsFreeOfCharge\x18\r \x01(\x08\x12\x15\n\rIncludeInRPSN\x18\x0e \x01(\x08\x12\x13\n\x0bPeriodicity\x18\x0f \x01(\t\x12\x34\n\x0f\x41\x63\x63ountDateFrom\x18\x10 \x01(\x0b\x32\x1b.cis.types.NullableGrpcDate\"\xc1\x01\n\tDeveloper\x12\x30\n\x0b\x44\x65veloperId\x18\x01 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12.\n\tProjectId\x18\x02 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\x12\x18\n\x10NewDeveloperName\x18\x03 \x01(\t\x12\x1f\n\x17NewDeveloperProjectName\x18\x04 \x01(\t\x12\x17\n\x0fNewDeveloperCin\x18\x05 \x01(\t\"p\n\x15PaymentScheduleSimple\x12\x14\n\x0cPaymentIndex\x18\x01 \x01(\x05\x12\x15\n\rPaymentNumber\x18\x02 \x01(\t\x12\x0c\n\x04\x44\x61te\x18\x03 \x01(\t\x12\x0c\n\x04Type\x18\x04 \x01(\t\x12\x0e\n\x06\x41mount\x18\x05 \x01(\t\"h\n\x11RiskLifeInsurance\x12#\n\x03Sum\x18\x01 \x01(\x0b\x32\x16.cis.types.GrpcDecimal\x12.\n\tFrequency\x18\x02 \x01(\x0b\x32\x1b.google.protobuf.Int32Value\"j\n\x13RealEstateInsurance\x12#\n\x03Sum\x18\x01 \x01(\x0b\x32\x16.cis.types.GrpcDecimal\x12.\n\tFrequency\x18\x02 \x01(\x0b\x32\x1b.google.protobuf.Int32ValueB(\xaa\x02%DomainServices.OfferService.Contractsb\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'Types_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  DESCRIPTOR._serialized_options = b'\252\002%DomainServices.OfferService.Contracts'
  _SIMULATIONRESULTWARNING._serialized_start=147
  _SIMULATIONRESULTWARNING._serialized_end=335
  _LOANPURPOSE._serialized_start=337
  _LOANPURPOSE._serialized_end=410
  _FEESETTINGS._serialized_start=412
  _FEESETTINGS._serialized_end=508
  _INPUTMARKETINGACTION._serialized_start=511
  _INPUTMARKETINGACTION._serialized_end=659
  _RESULTMARKETINGACTION._serialized_start=662
  _RESULTMARKETINGACTION._serialized_end=885
  _INPUTFEE._serialized_start=887
  _INPUTFEE._serialized_end=964
  _RESULTFEE._serialized_start=967
  _RESULTFEE._serialized_end=1512
  _DEVELOPER._serialized_start=1515
  _DEVELOPER._serialized_end=1708
  _PAYMENTSCHEDULESIMPLE._serialized_start=1710
  _PAYMENTSCHEDULESIMPLE._serialized_end=1822
  _RISKLIFEINSURANCE._serialized_start=1824
  _RISKLIFEINSURANCE._serialized_end=1928
  _REALESTATEINSURANCE._serialized_start=1930
  _REALESTATEINSURANCE._serialized_end=2036
# @@protoc_insertion_point(module_scope)