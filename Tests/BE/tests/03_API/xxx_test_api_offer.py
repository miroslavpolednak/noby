import pytest

from typing import List

from fe_api.FeAPI import FeAPI


ids = dict(offer_id = 0, case_id = 0)
#offer_id: int = 0
#case_id: int = 0

def test_simulate_mortgage():
    """Test API mortgage simulation."""
    import json
    data_str = '{"productTypeId":20001,"loanKindId":2000,"loanAmount":2000000,"loanDuration":186,"fixedRatePeriod":36,"collateralAmount":8000000,"paymentDay":15,"statementTypeId":1,"isEmployeeBonusRequested":true,"expectedDateOfDrawing":"2023-02-11T00:00:00.000Z","withGuarantee":false,"financialResourcesOwn":300000,"financialResourcesOther":20000,"drawingTypeId":2,"drawingDurationId":null,"loanPurposes":[{"id":202,"sum":1500000},{"id":203,"sum":500000}],"interestRateDiscount":0.15,"interestRateDiscountToggle":true,"marketingActions":{"domicile":true,"healthRiskInsurance":true,"realEstateInsurance":true,"incomeLoanRatioDiscount":true,"userVip":false},"developer":{"developerId":1845,"projectId":23,"newDeveloperName":null,"newDeveloperProjectName":null,"newDeveloperCin":null},"riskLifeInsurance":{"sum":15000,"frequency":null},"realEstateInsurance":{"sum":40000,"frequency":null},"resourceProcessId":"e7b24267-4728-4139-9552-cdd61e1b1439","fees":[]}'
    data_json = json.loads(data_str)

    mortgage = FeAPI.Offer.simulate_mortgage(data_json)
    assert isinstance(mortgage, dict)

    ids['offer_id'] = mortgage['offerId']
    assert isinstance(ids['offer_id'], int)
    print(f"- mortgage: [{ids['offer_id']}]")


def test_create_case():
    """Test API case creation."""
    import json
    data_str = '{"offerId":0,"firstName":"JAN","lastName":"NOVÁK","dateOfBirth":"1980-01-01T00:00:00","phoneNumberForOffer":"+420 777543234","emailForOffer":"novak@testcm.cz", "identity":{"id":951070688,"scheme":2} }'

    print(f"- mortgage: [{ids['offer_id']}]")

    data_json = json.loads(data_str)
    data_json['offerId'] = ids['offer_id']

    print(f"- mortgage: [{str(data_json)}]")

    case = FeAPI.Offer.create_case(data_json)
    assert isinstance(case, dict)

    print(f"- case: [{str(case)}]")

    case_id = case['CaseId']
    assert isinstance(case_id, int)
