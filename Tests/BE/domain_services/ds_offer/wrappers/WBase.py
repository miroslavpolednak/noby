from GrpcDecimal_pb2 import GrpcDecimal
from GrpcDate_pb2 import GrpcDate
from NullableGrpcDate_pb2 import NullableGrpcDate
from NullableGrpcDecimal_pb2 import NullableGrpcDecimal

# https://proto-plus-python.readthedocs.io/en/stable/reference/message.html

from google.protobuf import json_format
from google.protobuf.wrappers_pb2 import Int32Value

from datetime import datetime

class WBase():
    def __init__(self, stub: object, js_dict: dict = None):

        assert stub is not None, f'Stub must be provided!'
        self._stub = stub
        
        self._js_dict = dict()
        self.add(js_dict)
        
        
    def __str__ (self):
        return f'WBase.{self.class_name} [{self._js_dict}]'

    @property
    def class_name(self) -> object:
        return 'WBase' if self._stub is None else self._stub.__class__.__name__

    @property
    def stub(self) -> object:
        return self._stub

    @property
    def js_dict(self) -> dict:
        return self._js_dict

    def to_grpc_decimal(self, value)->GrpcDecimal:
        return GrpcDecimal(units= value, nanos=0)

    def to_nullable_grpc_decimal(self, value)->NullableGrpcDecimal:
        return NullableGrpcDecimal(units= value, nanos=0)

    def to_grpc_int(self, value)->Int32Value:
        return Int32Value(value= value)

    def to_grpc_date(self, value: datetime)->GrpcDate:
        return GrpcDate(year = value.year, month = value.month, day = value.day)

    def to_nullable_grpc_date(self, value: datetime)->NullableGrpcDate:
        return NullableGrpcDate(year = value.year, month = value.month, day = value.day)

    def _inject(self):
        try:
            self._stub.Clear()
            json_format.ParseDict(self.js_dict, self._stub)
        except Exception as e:
            print(e)
            raise e

    def add(self, js_dict: dict) -> object:

        if js_dict is None:
            return self

        js_dict_resolved: dict = WBase.resolve_js_dict(self._stub, js_dict)

        self._js_dict = {**self._js_dict, **js_dict_resolved}

        self._inject()
        
        return self

    def remove(self, key: str) -> object:

        assert hasattr(self._stub, key), f'Unknown key [{key}]'

        if key in self._js_dict:
            del self._js_dict[key]

        self._inject()

        return self

    
    @staticmethod
    def resolve_js_dict(stub: object, js_dict: dict) -> dict:

        if (js_dict is None):
            return js_dict

        js_dict_resolved: dict = dict()
            
        for key in js_dict.keys():

            assert hasattr(stub, key), f'Unknown key [{key}]'

            attr = getattr(stub, key)

            value = WBase.resolve_attr_value(attr, js_dict[key])

            js_dict_resolved[key] = value

            # try:
            #     json_format.ParseDict(stub, value)
            # except AttributeError as e:
            #     print(e)
            #     raise e

        return js_dict_resolved


    @staticmethod 
    def resolve_attr_value(attr: any, value: any) -> any:

        if (attr is None) or (value is None):
            return None

        # find out the class of property (attr)
        attr_class = attr.__class__

        # return if value is instance of property class
        if isinstance(value, attr_class):
            return value

        # map array if property is collection
        if attr_class.__name__ == 'RepeatedCompositeContainer':
            assert hasattr(value, "__len__"), f"Type [{type(value).__name__}] is not convertable to 'RepeatedCompositeContainer'!"
            value_repeated = []

            for i in value:
                value_repeated.append(i.js_dict if isinstance(i, WBase) else i)
            
            return value_repeated

        # create instance of property
        attr_instance = attr_class()

        # if instance is wrapper, take it's dict as value
        if isinstance(value, WBase):
            value = value.js_dict

        # check if value is convertable to property class
        try:
            json_format.ParseDict(value, attr_instance)
        except Exception as e:
            print(e)
            raise e
        
        # return
        return value
        
    def load_from_db(self, id: int):
        from domain_services.ds_offer import db_provider

        #print(f'WBasicParameters.from_db({id})')
        db_row = db_provider.loadBasicParameters(id)

        print(f'{self.class_name}.load_from_db({id}) [db_row: {db_row}]')

        js_dict = dict( StatementTypeId = 1 )

        
        
