print("Package: business.offer")

from .Offer import Offer
from .Developer import Developer
from .Fee import Fee
from .Insurance import Insurance
from .LoanPurpose import LoanPurpose
from .MarketingActions import MarketingActions

from fe_api.FeAPI import FeAPI

def build_models():
    import json

    js_dict = json.loads('{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}')
    #js_dict = json.loads('{"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}')

    developer = Developer.from_json(js_dict['developer'])
    print(developer)
    print(developer.to_grpc())
    print(Developer.to_grpc(developer))

    fee = Fee.from_json(json.loads('{"FeeId": 1, "DiscountPercentage": 1.1}'))
    print(fee)
    print(fee.to_grpc())
    print(Fee.to_grpc(fee))

    insurance_re = Insurance(js_dict['realEstateInsurance'])
    print(insurance_re)
    print(insurance_re.to_grpc())
    print(Insurance.to_grpc(insurance_re))

    insurance_rl = Insurance(js_dict['riskLifeInsurance'])
    print(insurance_rl)
    print(insurance_rl.to_grpc())
    print(Insurance.to_grpc(insurance_rl))
 
    loanPurpose = LoanPurpose(js_dict['loanPurposes'][0])
    print(loanPurpose)
    print(loanPurpose.to_grpc())
    print(LoanPurpose.to_grpc(loanPurpose))

    marketingActions = MarketingActions(js_dict['marketingActions'])
    print(marketingActions)
    print(marketingActions.to_grpc())
    print(MarketingActions.to_grpc(marketingActions))

    offer = Offer.from_json(js_dict)
    print(offer)
    print(offer.to_grpc())
    print(Offer.to_grpc(offer))
    # print(offer.get_value('loanKindId'))

    # """Test API get codebooks."""
    # from fe_api.enums.ECodebook import ECodebook
    # items = FeAPI.Codebooks.get_codebook(ECodebook.AcademicDegreesAfter)
    # print(items)
   


    return dict(offer=offer, developer=developer)


