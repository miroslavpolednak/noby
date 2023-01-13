from .WBase import WBase

from Types_pb2 import LoanPurpose

class WLoanPurpose(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=LoanPurpose(), js_dict = js_dict)

# message LoanPurpose {
# 	int32 LoanPurposeId = 1;
# 	cis.types.GrpcDecimal Sum = 2;
# }