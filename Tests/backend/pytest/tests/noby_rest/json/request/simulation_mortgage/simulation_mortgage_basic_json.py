from Tests.backend.pytest.tests.noby_rest.conftest import get_guarantee_date_from, get_expected_date_of_drawing

json_req_mortgage_basic_params = \
    {"productTypeId": 20001,
     "loanKindId": 2000,
     "loanAmount": 5000000,
     "loanDuration": 245,
     "fixedRatePeriod": 60,
     "collateralAmount": 9000000,
     "paymentDay": 15,
     "isEmployeeBonusRequested": False,
     "expectedDateOfDrawing": get_expected_date_of_drawing(),
     "withGuarantee": False,
     "financialResourcesOwn": 500000,
     "financialResourcesOther": 60000,
     "drawingTypeId": 2,
     "drawingDurationId": None,
     "loanPurposes": [
         {"id": 202, "sum": 4000000},
         {"id": 204, "sum": 1000000}],
     "interestRateDiscount": 0,
     "interestRateDiscountToggle": False,
     "marketingActions":
         {"domicile": True,
          "healthRiskInsurance": True,
          "realEstateInsurance": True,
          "incomeLoanRatioDiscount": True,
          "userVip": False},
     "developer": None,
     "riskLifeInsurance": {"sum": None, "frequency": None},
     "realEstateInsurance": {"sum": None, "frequency": None},
     "resourceProcessId": "ebc2d97b-17b1-4458-a612-1ac1e7a2488a",
     "fees": []}

json_req_mortgage_basic_mandatory_params = \
    {"productTypeId": 20001,
     "loanKindId": 2000,
     "loanAmount": 1000000,
     "loanDuration": 60,
     "fixedRatePeriod": 12,
     "collateralAmount": 1000000,
     "paymentDay": 15,
     "statementTypeId": 1,
     "isEmployeeBonusRequested": False,
     "expectedDateOfDrawing": get_expected_date_of_drawing(),
     "guaranteeDateFrom": get_guarantee_date_from(),
     "financialResourcesOwn": None,
     "financialResourcesOther": None,
     "interestRateDiscount": 0,
     "drawingType": 2,
     "drawingDuration": None,
     "loanPurposes": [
         {
             "id": 201,
             "sum": 1000000
         }
     ],
     "interestRateDeviation": 0,
     "interestRateDeviationToogle": False,
     "marketingActions": {
         "domicile": True,
         "healthRiskInsurance": True,
         "realEstateInsurance": False,
         "incomeLoanRatioDiscount": False,
         "userVip": False
     },
     "developer": {
         "developerId": 0,
         "projectId": 0,
         "newDeveloperName": None,
         "newDeveloperProjectName": None,
         "newDeveloperCin": None
     },
     "resourceProcessId": "a148fa5d-45b5-44bd-aa69-81d47e4c163c",
     "fees": []}