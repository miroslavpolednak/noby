
from Tests.backend.pytest.tests.noby_rest.conftest import get_guarantee_date_from

json_req_simulation_mortgage_full_not_developer_json = {
  "productTypeId": 20001,
  "loanKindId": 2001,
  "loanAmount": 3000000,
  "loanDuration": 331,
  "fixedRatePeriod": 108,
  "collateralAmount": 2000000,
  "paymentDay": 15,
  "statementTypeId": 6,
  "isEmployeeBonusRequested": True,
  "expectedDateOfDrawing": "2022-10-28T00:00:00.000Z",
  "guaranteeDateFrom": get_guarantee_date_from(),
  "financialResourcesOwn": 600000,
  "financialResourcesOther": 400000,
  "drawingTypeId": 1,
  "drawingDurationId": 2,
  "loanPurposes": [],
  "interestRateDiscount": "0.5",
  "interestRateDiscountToggle": True,
  "marketingActions": {
    "domicile": True,
    "healthRiskInsurance": True,
    "realEstateInsurance": True,
    "incomeLoanRatioDiscount": True,
    "userVip": True
  },
  "developer": None,
  "riskLifeInsurance": {
    "sum": 4500,
    "frequency": None
  },
  "realEstateInsurance": {
    "sum": 50000,
    "frequency": None
  },
  "resourceProcessId": "2bae7a88-8aa5-4e3a-bb88-4f59b96a0b27",
  "fees": []
}


json_req_simulation_mortgage_full_developer_json ={
  "productTypeId": 20001,
  "loanKindId": 2000,
  "loanAmount": 6000000,
  "loanDuration": 201,
  "fixedRatePeriod": 60,
  "collateralAmount": 12000000,
  "paymentDay": 20,
  "statementTypeId": 6,
  "isEmployeeBonusRequested": False,
  "expectedDateOfDrawing": "2022-11-01T00:00:00.000Z",
  "guaranteeDateFrom": "2022-10-31T00:00:00",
  "financialResourcesOwn": 5000000,
  "financialResourcesOther": 1000000,
  "drawingTypeId": 1,
  "drawingDurationId": 3,
  "loanPurposes": [
    {
      "id": 202,
      "sum": 6000000
    }
  ],
  "interestRateDiscount": None,
  "interestRateDiscountToggle": False,
  "marketingActions": {
    "domicile": True,
    "healthRiskInsurance": True,
    "realEstateInsurance": True,
    "incomeLoanRatioDiscount": True,
    "userVip": False
  },
  "developer": {
    "developerId": 3262,
    "projectId": 1,
    "newDeveloperName": None,
    "newDeveloperProjectName": None,
    "newDeveloperCin": None
  },
  "riskLifeInsurance": {
    "sum": 3500,
    "frequency": None
  },
  "realEstateInsurance": {
    "sum": 6000,
    "frequency": None
  },
  "resourceProcessId": "38c75043-0dc4-473f-81ff-810ff39564d4",
  "fees": []
}