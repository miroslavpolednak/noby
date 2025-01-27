from .WBase import WBase

from Mortgage.MortgageSimulationInputs_pb2 import MortgageSimulationInputs

class WMortgageSimulationInputs(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=MortgageSimulationInputs(), js_dict = js_dict)

# message MortgageSimulationInputs {
# 	cis.types.NullableGrpcDate ExpectedDateOfDrawing = 1;
# 	int32 ProductTypeId = 2;
# 	int32 LoanKindId = 3;
# 	cis.types.GrpcDecimal LoanAmount = 4;
# 	google.protobuf.Int32Value LoanDuration = 5;			//nullable kvůli validaci na NotNull
# 	cis.types.GrpcDate GuaranteeDateFrom = 6;
# 	cis.types.NullableGrpcDecimal InterestRateDiscount = 7;
# 	google.protobuf.Int32Value FixedRatePeriod = 8;			//nullable kvůli validaci na NotNull
#     cis.types.GrpcDecimal CollateralAmount = 9;
# 	google.protobuf.Int32Value DrawingTypeId = 10; 
# 	google.protobuf.Int32Value DrawingDurationId = 11;
# 	google.protobuf.Int32Value PaymentDay = 12;
# 	google.protobuf.BoolValue IsEmployeeBonusRequested = 13;
# 	Developer Developer = 14;
# 	repeated LoanPurpose LoanPurposes = 15;
# 	FeeSettings FeeSettings = 16;
# 	InputMarketingAction MarketingActions = 17;
# 	repeated InputFee Fees = 18;
# 	RiskLifeInsurance RiskLifeInsurance = 19;
# 	RealEstateInsurance RealEstateInsurance = 20;
# }
