from .Base import Base
from .LoanPurpose import LoanPurpose
from .Developer import Developer
from .MarketingActions import MarketingActions
from .Insurance import Insurance
from .Fee import Fee

from common import Convertor

DISPATCHES = {
            'product_type_id': lambda value: Convertor.to_int(value),
            'loan_kind_id': lambda value: Convertor.to_int(value),
            'loan_amount': lambda value: Convertor.to_decimal(value),
            'loan_duration': lambda value: Convertor.to_int(value),
            'fixed_rate_period': lambda value: Convertor.to_int(value),
            'collateral_amount': lambda value: Convertor.to_decimal(value),
            'payment_day': lambda value: Convertor.to_int(value),
            'statement_type_id': lambda value: Convertor.to_int(value),
            'is_employee_bonus_requested': lambda value: Convertor.to_bool(value),
            'expected_date_of_drawing': lambda value: Convertor.to_date(value),
            'with_guarantee': lambda value: Convertor.to_bool(value),
            'financial_resources_own': lambda value: Convertor.to_decimal(value),
            'financial_resources_other': lambda value: Convertor.to_decimal(value),
            'drawing_type_id': lambda value: Convertor.to_int(value),
            'drawing_duration_id': lambda value: Convertor.to_int(value),
            'interest_rate_discount': lambda value: Convertor.to_decimal(value),
            'interest_rate_discount_toggle': lambda value: Convertor.to_bool(value),
            'resource_process_id': lambda value: Convertor.to_guid(value),

            'loan_purposes': lambda value: LoanPurpose.from_json_list(value),
            'marketing_actions': lambda value: MarketingActions.from_json(value),
            'developer': lambda value: Developer.from_json(value),
            'risk_life_insurance': lambda value: Insurance.from_json(value),
            'real_estate_insurance': lambda value: Insurance.from_json(value),
            'fees': lambda value: Fee.from_json_list(value),
        }

# CREATE TABLE SimulateMortgageRequest (SimulateMortgageRequestPk INTEGER PRIMARY KEY, ResourceProcessId TEXT NOT NULL /*string*/, BasicParametersPk INTEGER NULL /*None*/, SimulationInputsPk INTEGER NULL /*None*/);
# CREATE TABLE BasicParameters (BasicParametersPk INTEGER PRIMARY KEY, FinancialResourcesOwn REAL NULL /*NullableGrpcDecimal*/, FinancialResourcesOther REAL NULL /*NullableGrpcDecimal*/, GuaranteeDateTo REAL NULL /*NullableGrpcDate*/, StatementTypeId INTEGER NULL /*Int32Value*/);
# CREATE TABLE MortgageSimulationInputs (MortgageSimulationInputsPk INTEGER PRIMARY KEY, ExpectedDateOfDrawing REAL NULL /*NullableGrpcDate*/, ProductTypeId INTEGER NOT NULL /*int32*/, LoanKindId INTEGER NOT NULL /*int32*/, LoanAmount REAL NOT NULL 
# /*GrpcDecimal*/, LoanDuration INTEGER NULL /*Int32Value*/, InterestRateDiscount REAL NULL /*NullableGrpcDecimal*/, FixedRatePeriod INTEGER NULL /*Int32Value*/, DrawingTypeId INTEGER NULL /*Int32Value*/, DrawingDurationId INTEGER NULL /*Int32Value*/, PaymentDay INTEGER NULL /*Int32Value*/, IsEmployeeBonusRequested INTEGER NULL /*BoolValue*/, DeveloperPk INTEGER NULL /*None*/, FeeSettingsPk INTEGER NULL /*None*/, MarketingActionsPk INTEGER NULL /*None*/, RiskLifeInsurancePk INTEGER NULL /*None*/, RealEstateInsurancePk INTEGER NULL /*None*/);
# CREATE TABLE Developer (DeveloperPk INTEGER PRIMARY KEY, DeveloperId INTEGER NULL /*Int32Value*/, ProjectId INTEGER NULL /*Int32Value*/, NewDeveloperName TEXT NOT NULL /*string*/, NewDeveloperProjectName TEXT NOT NULL /*string*/, NewDeveloperCin TEXT NOT NULL /*string*/);
# CREATE TABLE LoanPurpose (LoanPurposePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, LoanPurposeId INTEGER NOT NULL /*int32*/, Sum REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(MortgageSimulationInputsPk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
# CREATE TABLE FeeSettings (FeeSettingsPk INTEGER PRIMARY KEY, FeeTariffPurpose INTEGER NULL /*Int32Value*/, IsStatementCharged INTEGER NOT NULL /*bool*/);
# CREATE TABLE InputMarketingAction (InputMarketingActionPk INTEGER PRIMARY KEY, Domicile INTEGER NOT NULL /*bool*/, HealthRiskInsurance INTEGER NOT NULL /*bool*/, RealEstateInsurance INTEGER NOT NULL /*bool*/, IncomeLoanRatioDiscount INTEGER NOT NULL /*bool*/, UserVip INTEGER NOT NULL /*bool*/);
# CREATE TABLE InputFee (InputFeePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, FeeId INTEGER NOT NULL /*int32*/, DiscountPercentage REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(MortgageSimulationInputsPk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
# CREATE TABLE RiskLifeInsurance (RiskLifeInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);
# CREATE TABLE RealEstateInsurance (RealEstateInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);

class Offer(Base):

    def __init__(self, js_dict: dict = None):
        super().__init__(dispatches= DISPATCHES , js_dict = js_dict)

    @property
    def product_type_id(self) -> int:
        return self.__product_type_id

    @staticmethod
    def from_json(js_dict: dict):
        return Offer(js_dict = js_dict)

    @staticmethod
    def to_grpc(value: object):
        if (value is None):
            return None
        assert isinstance(value, Offer)
        return value.to_grpc()

    def to_grpc(self) -> dict:

        # message SimulateMortgageRequest {
        #     string ResourceProcessId = 1;
        #     BasicParameters BasicParameters = 2;
        #     MortgageSimulationInputs SimulationInputs = 3;
        # }

        # message BasicParameters {
        #     cis.types.NullableGrpcDecimal FinancialResourcesOwn  = 1;
        #     cis.types.NullableGrpcDecimal FinancialResourcesOther  = 2;
        #     cis.types.NullableGrpcDate GuaranteeDateTo = 3;
        #     google.protobuf.Int32Value StatementTypeId = 4;
        # }

        # message MortgageSimulationInputs {
        #     cis.types.NullableGrpcDate ExpectedDateOfDrawing = 1;
        #     int32 ProductTypeId = 2;
        #     int32 LoanKindId = 3;
        #     cis.types.GrpcDecimal LoanAmount = 4;
        #     google.protobuf.Int32Value LoanDuration = 5;			//nullable kvůli validaci na NotNull
        #     cis.types.GrpcDate GuaranteeDateFrom = 6;
        #     cis.types.NullableGrpcDecimal InterestRateDiscount = 7;
        #     google.protobuf.Int32Value FixedRatePeriod = 8;			//nullable kvůli validaci na NotNull
        #     cis.types.GrpcDecimal CollateralAmount = 9;
        #     google.protobuf.Int32Value DrawingTypeId = 10; 
        #     google.protobuf.Int32Value DrawingDurationId = 11;
        #     google.protobuf.Int32Value PaymentDay = 12;
        #     google.protobuf.BoolValue IsEmployeeBonusRequested = 13;

        #     Developer Developer = 14;
        #     repeated LoanPurpose LoanPurposes = 15;
        #     FeeSettings FeeSettings = 16;
        #     InputMarketingAction MarketingActions = 17;
        #     repeated InputFee Fees = 18;
        #     RiskLifeInsurance RiskLifeInsurance = 19;
        #     RealEstateInsurance RealEstateInsurance = 20;
        # }

        basic_parameters = dict(
            FinancialResourcesOwn = Convertor.to_grpc(self.get_value('financial_resources_own')),
            FinancialResourcesOther = Convertor.to_grpc(self.get_value('financial_resources_other')),
            # GuaranteeDateTo = Convertor.to_grpc(self.get_value('?')),
            StatementTypeId = Convertor.to_grpc(self.get_value('statement_type_id')),
        )

        simulation_inputs = dict(
            ExpectedDateOfDrawing = Convertor.to_grpc(self.get_value('expected_date_of_drawing')),
            ProductTypeId = Convertor.to_grpc(self.get_value('product_type_id')),
            LoanKindId = Convertor.to_grpc(self.get_value('loan_kind_id')),
            LoanAmount = Convertor.to_grpc(self.get_value('loan_amount')),
            LoanDuration = Convertor.to_grpc(self.get_value('loan_duration')),
            # GuaranteeDateFrom = Convertor.to_grpc(self.get_value('?')),
            InterestRateDiscount = Convertor.to_grpc(self.get_value('interest_rate_discount')),
            FixedRatePeriod = Convertor.to_grpc(self.get_value('fixed_rate_period')),
            CollateralAmount = Convertor.to_grpc(self.get_value('collateral_amount')),
            DrawingTypeId = Convertor.to_grpc(self.get_value('drawing_type_id')),
            DrawingDurationId = Convertor.to_grpc(self.get_value('drawing_duration_id')),
            PaymentDay = Convertor.to_grpc(self.get_value('payment_day')),
            IsEmployeeBonusRequested = Convertor.to_grpc(self.get_value('is_employee_bonus_requested')),

            Developer = Developer.to_grpc(self.get_value('developer')),
            LoanPurposes = LoanPurpose.to_grpc_list(self.get_value('loan_purposes')),
            # FeeSettings = Convertor.to_grpc(self.get_value('financial_resources_own')),
            MarketingActions = MarketingActions.to_grpc(self.get_value('marketing_actions')),
            # Fees = Convertor.to_grpc(self.get_value('financial_resources_own')),
            RiskLifeInsurance = Insurance.to_grpc(self.get_value('risk_life_insurance')),
            RealEstateInsurance = Insurance.to_grpc(self.get_value('real_estate_insurance')),
        )

        return dict(
            ResourceProcessId = Convertor.to_grpc(self.get_value('resource_process_id')),
            BasicParameters = basic_parameters,
            SimulationInputs = simulation_inputs
        )        



# ?????:
# "withGuarantee":false,
# "interestRateDiscountToggle":true,


# --------------------------------------------------------------------------------------------

# class OfferPrev():
#     def __init__(self, js_dict: dict = None):

#         dispatches = {
#             'product_type_id': lambda value: Convertor.to_int(value),
#             'loan_kind_id': lambda value: Convertor.to_val(value),
#             'loan_amount': lambda value: Convertor.to_val(value),
#             'loan_duration': lambda value: Convertor.to_val(value),
#             'fixed_rate_period': lambda value: Convertor.to_val(value),
#             'collateral_amount': lambda value: Convertor.to_val(value),
#             'payment_day': lambda value: Convertor.to_val(value),
#             'statement_type_id': lambda value: Convertor.to_val(value),
#             'is_employee_bonus_requested': lambda value: Convertor.to_val(value),
#             'expected_date_of_drawing': lambda value: Convertor.to_val(value),
#             'with_guarantee': lambda value: Convertor.to_val(value),
#             'financial_resources_own': lambda value: Convertor.to_val(value),
#             'financial_resources_other': lambda value: Convertor.to_val(value),
#             'drawing_type_id': lambda value: Convertor.to_val(value),
#             'drawing_duration_id': lambda value: Convertor.to_val(value),
#             'interest_rate_discount': lambda value: Convertor.to_val(value),
#             'interest_rate_discount_toggle': lambda value: Convertor.to_val(value),
#             'resource_process_id': lambda value: Convertor.to_val(value),

#             'loan_purposes': lambda value: Convertor.to_val(value),
#             'marketing_actions': lambda value: Convertor.to_val(value),
#             'developer': lambda value: Convertor.to_val(value),
#             'risk_life_insurance': lambda value: Convertor.to_val(value),
#             'real_estate_insurance': lambda value: Convertor.to_val(value),
#             'fees': lambda value: Convertor.to_val(value),
#         }

#         # replace '_' in dict keys 
#         for k in list(dispatches.keys()):
#             k_next = k.replace('_', '').lower()
#             dispatches[k_next] = dict(fce = dispatches[k], key = k)
#             if k != k_next:
#                 del dispatches[k]

#         self.__dispatches = dispatches
#         self.__init_from_json(js_dict)
        
#     def __init_from_json(self, js_dict: dict):
#         assert js_dict is not None, f'Json data must be provided!'
#         print(js_dict)

#         for k in list(js_dict.keys()):
#             assert k.lower() in self.__dispatches, f"Invalid attribute '{k}'"

#             dispatch = self.__dispatches[k.lower()]
#             dispatch_fce = dispatch['fce']
#             dispatch_key = dispatch['key']

#             val_in = js_dict[k]
#             val_out = dispatch_fce(val_in)

#             attr = f'_{self.__class__.__name__}__{dispatch_key}'
#             setattr(self, attr, val_out)

#     def __str__ (self):
#         return f'Offer [product_type_id:{self.__product_type_id}]'

#     @property
#     def product_type_id(self) -> int:
#         return self.__product_type_id
        
# --------------------------------------------------------------------------------------------


