print("Package: ds_offer.models")

from .Base import Base
from .Offer import Offer
from .Developer import Developer
from .Insurance import Insurance
from .LoanPurpose import LoanPurpose
from .MarketingActions import MarketingActions


def build_models():
    import json

    js_dict = json.loads('{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}')
    #js_dict = json.loads('{"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}')

    developer = Developer.from_json(js_dict['developer'])
    print(developer)

    insurance_re = Insurance(js_dict['realEstateInsurance'])
    print(insurance_re)

    insurance_rl = Insurance(js_dict['riskLifeInsurance'])
    print(insurance_rl)
 
    loanPurpose = LoanPurpose(js_dict['loanPurposes'][0])
    print(loanPurpose)

    marketingActions = MarketingActions(js_dict['marketingActions'])
    print(marketingActions)

    offer = Offer.from_json(js_dict)
    print(offer)
    # print(offer.product_type_id)

    return dict(offer=offer, developer=developer)
