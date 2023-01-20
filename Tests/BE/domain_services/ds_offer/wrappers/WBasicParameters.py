from .WBase import WBase
from domain_services.ds_offer import db_provider

from BasicParameters_pb2 import BasicParameters

class WBasicParameters(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=BasicParameters(), js_dict = js_dict)

    # @classmethod
    # def from_db(cls, id: int):
    #     #print(f'WBasicParameters.from_db({id})')
    #     db_row = db_provider.loadBasicParameters(id)

    #     js_dict = dict( StatementTypeId = 1 )


    #     print(f'WBasicParameters.from_db({id}) [db_row: {db_row}]')
    #     return cls(js_dict)

# message BasicParameters {
# 	cis.types.NullableGrpcDecimal FinancialResourcesOwn  = 1;
# 	cis.types.NullableGrpcDecimal FinancialResourcesOther  = 2;
# 	cis.types.NullableGrpcDate GuaranteeDateTo = 3;
# 	google.protobuf.Int32Value StatementTypeId = 4;
# }