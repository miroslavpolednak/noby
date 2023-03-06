from typing import List

from business.offer import Offer

from fe_api import FeAPI
from common import Log

class ApiReaderOffer():

    def __init__(self):
        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')
   

    def load(self, offer_id: int) -> Offer | Exception:

        result: Offer | Exception = None

        try:
            result = self.__load_offer(offer_id)
        except Exception as e:
            result = e

        return result


    def __load_offer(self, offer_id: int) -> Offer:

        res_offer = FeAPI.Offer.get_offer(offer_id)

        # ['productTypeId','loanKindId','loanAmount','loanDuration','fixedRatePeriod','collateralAmount','paymentDay','statementTypeId','isEmployeeBonusRequested','expectedDateOfDrawing','withGuarantee','financialResourcesOwn','financialResourcesOther','drawingTypeId','drawingDurationId','loanPurposes','interestRateDiscount','interestRateDiscountToggle','marketingActions','developer','riskLifeInsurance','realEstateInsurance','resourceProcessId','fees']
        json_dict = dict(

        )

        return Offer(json_dict)
