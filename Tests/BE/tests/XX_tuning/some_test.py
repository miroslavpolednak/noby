# # ----------------------------
# import _setup
# # ----------------------------
# from typing import List
# #from OfferIdRequest_pb2 import OfferIdRequest
# # ----------------------------

# def simulate_mortgage():
#     import json
#     from fe_api import FeAPI
    
#     data_str = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}'
#     data_json = json.loads(data_str)

#     mortgage = FeAPI.Offer.simulate_mortgage(data_json)
#     assert isinstance(mortgage, dict)

#     offer_id = mortgage['offerId']
#     assert isinstance(offer_id, int)
#     print(f"- mortgage: {offer_id}")

# def run_offers():
#     from tuning.TestProcessor import TestProcessor
#     offers_to_test: List[str] = TestProcessor.instance().get_offer_codes()
#     TestProcessor.instance().run_offers(offers_to_test)

# def run_cases():
#     from tuning.TestProcessor import TestProcessor
#     cases_to_test: List[str] = TestProcessor.instance().get_case_codes()[0:1]
#     TestProcessor.instance().run_cases(cases_to_test)

# from tuning.TestProcessor import TestProcessor
# TestProcessor.instance().process_case()

# #simulate_mortgage()
# #run_offers()
# #run_cases()

# print('FINISHED!')
