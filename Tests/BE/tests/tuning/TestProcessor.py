import json
from typing import List

from fe_api import FeAPI

test_data: dict = dict(
    offer = [
        # offer_first
        dict(
            code = 'offer_first',
            req = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}',
            option = '{"ExpectedDateOfDrawing": 10}',
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
            res = None
        ),
        # case_xyz
        dict(
            code = 'case_xyz',
            req = '{"offerId":0,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","phoneNumberForOffer":"+420 777543234","emailForOffer":"novak@testcm.cz", "identity":{"id":951070688,"scheme":2} }',
            option = '{"Offer": "offer_second"}',
            res = None
        ),
    ]
)

def to_date(days: int = 0) -> str:
    from datetime import datetime, timedelta

    date: datetime = datetime.today() + timedelta(days=days)

    #return date.isoformat()
    return date.strftime('%Y-%m-%d')


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

        def apply_options(offer: dict, options: dict) -> dict:

            if (options is None):
                return offer
            
            if ('ExpectedDateOfDrawing' in options):
                offer['expectedDateOfDrawing'] = to_date(int(options['ExpectedDateOfDrawing']))
            
            return offer

        req = json.loads(data['req'])
        option_json = json.loads(data['option'])

        req = apply_options(req, option_json)

        print(f'process_offer.req [{req}]')

        res = FeAPI.Offer.simulate_mortgage(req)

        print(f'process_offer.res [{res}]')

        res_str = json.dumps(res)

        data['res'] = res_str

    def process_case(self, data:dict):

        def apply_options(case: dict, options: dict) -> dict:

            if (options is None):
                return case
            
            if ('Offer' in options):
                offer_code = options['Offer']
                offer = self.__offer_by_code[offer_code]

                if (offer['res'] is None):
                    self.process_offer(offer)

                offer_res = json.loads(offer['res'])
                offer_id = offer_res['offerId']

                case['offerId'] = int(offer_id)
            
            return case

        req = json.loads(data['req'])
        option_json = json.loads(data['option'])

        req = apply_options(req, option_json)

        print(f'process_case.req [{req}]')

        res = FeAPI.Offer.create_case(req)

        print(f'process_case.res [{res}]')

        res_str = json.dumps(res)

        data['res'] = res_str

# print(to_date(0))
# print(to_date(10))
# print(to_date(-5))