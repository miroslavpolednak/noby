
from Tests.backend.pytest.tests.noby_rest.conftest import get_guarantee_date_from


json_req_mortgage_basic_params = {"productTypeId": 20001,
            "loanKindId": 2000,
            "loanAmount": 1000000,
            "loanDuration": 60,
            "fixedRatePeriod": 12,
            "collateralAmount": 1000000,
            "paymentDay": 15,
            "statementTypeId": 1,
            "isEmployeeBonusRequested": False,
            "expectedDateOfDrawing": None,
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