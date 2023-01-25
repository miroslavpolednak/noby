import json
from typing import List

from business.codebooks import EProductType, ELoanKind, EHouseholdType
from fe_api import FeAPI

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# DATA
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# EProductType:     Mortgage, MortgageBridging, MortgageWithoutIncome, MortgageNonPurposePart, MortgageAmerican
# ELoanKind:        Standard, MortgageWithoutRealty
# EHouseholdType:   Main, Codebtor, Garantor
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

test_data: dict = dict(
    offer = [
        # offer_first
        dict(
            code = 'offer_first',
            #req = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}',
            #req = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":5000000,"loanDuration":120,"fixedRatePeriod":60,"collateralAmount":1000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":false,"expectedDateOfDrawing":"2023-01-26T10:09:53.127Z","withGuarantee":false,"financialResourcesOwn":null,"financialResourcesOther":null,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":103,"sum":3000000},{"id":129,"sum":2000000}],"interestRateDiscount":0,"interestRateDiscountToggle":false,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":false,"incomeLoanRatioDiscount":false,"userVip":false},"developer":null,"riskLifeInsurance":{"sum":null,"frequency":null},"realEstateInsurance":{"sum":null,"frequency":null},"resourceProcessId":"127532f3-093f-4e28-abb4-ed409bd97d43","fees":[]}',
            req = '{"loanAmount":5000000,"loanDuration":120,"fixedRatePeriod":60,"collateralAmount":1000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":false,"expectedDateOfDrawing":"2023-01-26T10:09:53.127Z","withGuarantee":false,"financialResourcesOwn":null,"financialResourcesOther":null,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":3000000},{"id":203,"sum":2000000}],"interestRateDiscount":0,"interestRateDiscountToggle":false,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":false,"incomeLoanRatioDiscount":false,"userVip":false},"developer":null,"riskLifeInsurance":{"sum":null,"frequency":null},"realEstateInsurance":{"sum":null,"frequency":null},"resourceProcessId":"127532f3-093f-4e28-abb4-ed409bd97d43","fees":[]}', # "productTypeId":20001,"loanKindId":2000,
            option = '{"ExpectedDateOfDrawing": 1, "ProductType": "Mortgage", "LoanKind": "Standard"}',
            res = None
        ),
        # offer_second
        dict(
            code = 'offer_second',
            req = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}',
            option = '{"ExpectedDateOfDrawing": 5}',
            res = None
        ),
    ],
    case = [
        # case_abc
        dict(
            code = 'case_abc',
            req = '{"offerId":0,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","phoneNumberForOffer":"+420 777543234","emailForOffer":"novak@testcm.cz", "identity":{"id":0,"scheme":0} }',
            option = '{"Offer": "offer_first"}',
            households = """[
                {"HouseholdType": "Codebtor", "childrenUpToTenYearsCount": 1, "childrenOverTenYearsCount": 1},
                {"HouseholdType": "Garantor", "childrenUpToTenYearsCount": 0, "childrenOverTenYearsCount": 3}
            ]""",
            res = None
        ),
        # case_xyz
        dict(
            code = 'case_xyz',
            req = '{"offerId":0,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","phoneNumberForOffer":"+420 777543234","emailForOffer":"novak@testcm.cz", "identity":{"id":951070688,"scheme":2} }',
            option = '{"Offer": "offer_second"}',
            res = None
        ),
    ],   
)

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
# OPTION RESOLVERS
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
def to_date(days: int = 0) -> str:
    from datetime import datetime, timedelta

    date: datetime = datetime.today() + timedelta(days=days)

    #return date.isoformat()
    return date.strftime('%Y-%m-%d')

def to_product_type_id(product_type) -> int:
    e = EProductType[product_type]
    return e.value

def to_loan_kind_id(loan_kind) -> int:
    e = ELoanKind[loan_kind]
    return e.value

def to_household_type_id(household_type) -> int:
    e = EHouseholdType[household_type]
    return e.value

def resolve_options(options: dict, option_resolvers: dict) -> dict:

    if (options is None):
        return None

    result = {}

    for k in options.keys():
        assert k in option_resolvers.keys(), f'Option resolver not specified [{k}]'
        target_key = option_resolvers[k]['target_key']
        fce = option_resolvers[k]['fce']
        value_in = options[k]
        value_out = fce(value_in)
        result[target_key] = value_out

    return result          

option_resolvers_household = {
    'HouseholdType': dict(fce = lambda value: to_household_type_id(value), target_key = 'householdTypeId'),
}

option_resolvers_offer = {
    'ExpectedDateOfDrawing': dict(fce = lambda value: to_date(value), target_key = 'expectedDateOfDrawing'),
    'ProductType': dict(fce = lambda value: to_product_type_id(value), target_key = 'productTypeId'),
    'LoanKind': dict(fce = lambda value: to_loan_kind_id(value), target_key = 'loanKindId'),
}

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------

class TestProcessor():

    __instance = None

    def __init__(self):
        #self.__data = test_data

        offer_by_code = {}
        case_by_code = {}

        for i in test_data['offer']:
            offer_by_code[i['code']] = i

        for i in test_data['case']:
            case_by_code[i['code']] = i

        self.__offer_by_code = offer_by_code
        self.__case_by_code = case_by_code

    @staticmethod
    def instance():
        if (TestProcessor.__instance is None):
            TestProcessor.__instance = TestProcessor()
        return TestProcessor.__instance
        
    def get_offer_codes(self) -> List[str]:
        return list(self.__offer_by_code.keys())

    def get_case_codes(self) -> List[str]:
        return list(self.__case_by_code.keys())

    def run_offers(self, codes: List[str]):
        offer_codes =  self.get_offer_codes()

        # validate codes
        for c in codes:
            assert c in offer_codes, f'Offer code [{c}] not found!'

        # process
        for c in codes:
            data = self.__offer_by_code[c]
            self.process_offer(data)

    def run_cases(self, codes: List[str]):
        case_codes =  self.get_case_codes()

        # validate codes
        for c in codes:
            assert c in case_codes, f'Case code [{c}] not found!'

        # process
        for c in codes:
            data = self.__case_by_code[c]
            self.process_case(data)

    def process_offer(self, data:dict):

        # load base payload
        req = json.loads(data['req'])

        # load options
        option_json = json.loads(data['option'])

        # resolve options
        option_results = resolve_options(option_json, option_resolvers_offer)

        # merge base payload & option results
        req = {**req, **option_results}

        # call FE API endpoint 
        print(f'process_offer.req [{req}]')
        res = FeAPI.Offer.simulate_mortgage(req)
        print(f'process_offer.res [{res}]')

        # response serialization & persistence
        res_str = json.dumps(res)
        data['res'] = res_str

    def process_case(self, data:dict):

        def resolve_options_case(options: dict) -> dict:

            if (options is None):
                return None

            result = {}

            if ('Offer' in options):
                offer_code = options['Offer']
                offer = self.__offer_by_code[offer_code]

                if (offer['res'] is None):
                    self.process_offer(offer)

                offer_res = json.loads(offer['res'])
                offer_id = offer_res['offerId']

                result['offerId'] = int(offer_id)

            return result

        # load base payload
        req = json.loads(data['req'])

        # load options
        option_json = json.loads(data['option'])

        # resolve options
        option_results = resolve_options_case(option_json)

        # merge base payload & option results
        req = {**req, **option_results}

        # call FE API endpoint
        print(f'process_case.req [{req}]')
        res = FeAPI.Offer.create_case(req)
        print(f'process_case.res [{res}]')

        # response serialization & persistence
        res_str = json.dumps(res)
        data['res'] = res_str

        # add households
        self.process_households(data)

    def process_households(self, case_data: dict):

        if ('households' not in case_data):
            return

        sales_arrangement_id = json.loads(case_data['res'])['salesArrangementId']
        households: List[dict] = json.loads(case_data['households'])

        for h in households:
            # ----- CREATE HOUSEHOLD -----
            # load base payload
            req = dict( salesArrangementId = sales_arrangement_id)
            options = dict( HouseholdType = h['HouseholdType'] )
            # resolve options
            option_results = resolve_options(options, option_resolvers_household)
            # merge base payload & option results
            req = {**req, **option_results}
            # call FE API endpoint
            print(f'process_household.req [{req}]')
            res = FeAPI.Household.create_household(req)
            print(f'process_household.res [{res}]')

            # ----- SET HOUSEHOLD PARAMETERS -----
            household_id = res['householdId']
            req_data = h.copy()
            del req_data['HouseholdType']
            req = dict(data = req_data)

            # call FE API endpoint
            print(f'process_household_parameters.req [{req}]')
            res = FeAPI.Household.set_household_parameters(household_id, req) 
            print(f'process_household_parameters.res [{res}]')

            # TODO: set params to main household!
            

# print(to_date(0))
# print(to_date(10))
# print(to_date(-5))


# --------------------------------------------------------------------------------------------------------------
# FE - WORKFLOW
# --------------------------------------------------------------------------------------------------------------

# https://fat.noby.cz/#/mortgage/case-detail/3011321

# POST https://noby-fat.vsskb.cz/api/offer/mortgage/create-case
# {'offerId': 691, 'firstName': 'JAN', 'lastName': 'NOVÁK', 'dateOfBirth': '1980-01-01T00:00:00', 'phoneNumberForOffer': '+420 777543234', 'emailForOffer': 'novak@testcm.cz', 'identity': {'id': 0, 'scheme': 0}}
# {'caseId': 3011321, 'salesArrangementId': 226, 'offerId': 691, 'householdId': 223, 'customerOnSAId': 262}

# GET https://fat.noby.cz/api/case/3011321
# {"caseOwner":{"cpm":"990614w","icp":""},"caseId":3011321,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","state":1,"stateName":"Příprava žádosti","contractNumber":"","targetAmount":5000000,"productName":"Hypoteční úvěr","createdTime":"2023-01-25T11:40:00.117","createdBy":"Filip Tůma","stateUpdated":"2023-01-25T11:40:00.113","emailForOffer":"novak@testcm.cz","phoneNumberForOffer":"+420 777543234"}

# add household to SA
# POST https://fat.noby.cz/api/household
# {salesArrangementId: "226", householdTypeId: 2}
# {"householdId":225,"householdTypeId":2,"householdTypeName":"Spoludlužnická"}

# load households for SA (salesArrangementId: 226)
# GET https://fat.noby.cz/api/household/list/226
# [{"householdId":223,"householdTypeId":1,"householdTypeName":"Hlavní"},{"householdId":225,"householdTypeId":2,"householdTypeName":"Spoludlužnická"}]

# set params of household
# PUT https://fat.noby.cz/api/household/225
# {data: {childrenUpToTenYearsCount: 2, childrenOverTenYearsCount: 0}, expenses: {}}

# --------------------------------------------------------------------------------------------------------------
