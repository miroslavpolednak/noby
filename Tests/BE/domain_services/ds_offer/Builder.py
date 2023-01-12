import sys
import importlib.util 

PATH_TO_PYTEST:str = 'D:/Users/992466q/source/repos/OneSolution/Tests/backend/pytest'
PATH_TO_GRPC:str = 'D:/Users/992466q/source/repos/OneSolution/Tests/backend/pytest/grpc'
SERVER_CERTIFICATE_ADDR:tuple[str, int] = ('adpra191.vsskb.cz', 30001)
CHANNEL_TARGET = 'adpra191:30006'

#Import Stubs for CIS Types
sys.path.append(f'{PATH_TO_GRPC}/CisTypes')

#Import Stubs for Domain Service
sys.path.append(f'{PATH_TO_GRPC}/OfferService')

sys.path.append(f'{PATH_TO_PYTEST}/tests/noby_grpc/OfferService')

#from Wrappers import WInputMarketingAction, WLoanPurpose, WBasicParameters, WMortgageSimulationInputs
from wrappers import *
from transformers import *

#Import Stub for Service. This way is necessary because of dot in the name of file
spec = importlib.util.spec_from_file_location(name="OfferService", location=f'{PATH_TO_GRPC}/OfferService/OfferService.v1_pb2_grpc.py')

# from common import DB
# print(DB)
# print(Config.env_name)
# ---------------------------------------------------------------------------------------------------------------------------------------

# from Mortgage.MortgageSimulationInputs_pb2 import MortgageSimulationInputs
# from Types_pb2 import InputMarketingAction
# from google.protobuf import json_format
# js_dict = dict(
#     ExpectedDateOfDrawing = dict(year = 2022, month = 5, day = 3),
#     ProductTypeId = 20001,
#     LoanKindId = 2000,
#     LoanAmount = dict(units = 3000000),
#     LoanDuration = 36,
#     GuaranteeDateFrom = dict(year = 2022, month = 5, day = 3),
#     FixedRatePeriod = 24,
#     CollateralAmount = dict(units = 2500000),
#     MarketingActions = dict(Domicile = True,IncomeLoanRatioDiscount = True,UserVip = True),
#     LoanPurposes = [dict(LoanPurposeId=201), dict(LoanPurposeId=201)]
# )
# stub = MortgageSimulationInputs()
# json_format.ParseDict(js_dict, stub)
# print(stub)

# ---------------------------------------------------------------------------------------------------------------------------------------

# WBasicParameters
basic_parameters = WBasicParameters(dict(FinancialResourcesOther = dict(units = 1))).add(dict(StatementTypeId = 1, GuaranteeDateTo = dict(year = 2022, month = 5, day = 3)))
print(basic_parameters)
print(basic_parameters.stub)


# WLoanPurpose
loan_purpose_1 = WLoanPurpose(dict(LoanPurposeId=201)).add(dict(Sum=dict(units = 1000000)))
loan_purpose_2 = WLoanPurpose(dict(LoanPurposeId=202)).add(dict(Sum=dict(units = 2000000)))
loan_purposes = [loan_purpose_1, loan_purpose_2]
print(*loan_purposes, sep = "\n")


# WInputMarketingAction
marketing_actions = WInputMarketingAction(dict(Domicile = True,IncomeLoanRatioDiscount = True,UserVip = True))
print(marketing_actions)
print(marketing_actions.stub)


# WInputFee
fee_1 = WInputFee(dict(FeeId = 1, DiscountPercentage = dict(units = 5)))
fee_2 = WInputFee(dict(FeeId = 2, DiscountPercentage = dict(units = 8)))
fees = [fee_1, fee_2]


# WMortgageSimulationInputs
simulation_inputs = WMortgageSimulationInputs(
    dict(
        ExpectedDateOfDrawing = dict(year = 2022, month = 5, day = 3),
        ProductTypeId = 20001,
        LoanKindId = 2000,
        LoanAmount = dict(units = 3000000),
        LoanDuration = 36,
        GuaranteeDateFrom = dict(year = 2022, month = 5, day = 3),
        FixedRatePeriod = 24,
        InterestRateDiscount = dict(units = 1),
        CollateralAmount = dict(units = 2500000),
        DrawingTypeId = 1,
        DrawingDurationId = 1,
        PaymentDay = 1,
        IsEmployeeBonusRequested = True,
        Developer = WDeveloper(dict(DeveloperId = 1, ProjectId = 1, NewDeveloperName = 'Dev Name', NewDeveloperProjectName = 'Dev Project Name', NewDeveloperCin = 'Dev Cin')),
        LoanPurposes = loan_purposes,
        FeeSettings = WFeeSettings(dict(FeeTariffPurpose = 1, IsStatementCharged = False )),
        MarketingActions = WInputMarketingAction(dict(Domicile = True,IncomeLoanRatioDiscount = True,UserVip = True)),
        Fees = fees,
        RiskLifeInsurance = WRiskLifeInsurance(dict(Sum = dict(units = 1000000), Frequency = 2)),
        RealEstateInsurance = WRealEstateInsurance(dict(Sum = dict(units = 500000), Frequency = 3)),
    )
)
print(simulation_inputs)
print(simulation_inputs.stub)


# WSimulateMortgageRequest
simulate_mortgage_request = WSimulateMortgageRequest(
    dict(
        ResourceProcessId = '4D115798-0E05-4CF0-8A5A-1A3F871B3727',
        BasicParameters = basic_parameters,
        SimulationInputs = simulation_inputs
    )
)
print(simulate_mortgage_request)
print(simulate_mortgage_request.stub)