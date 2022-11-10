import json
from Tests.backend.pytest.tests.noby_rest.conftest import current_datetime_000z


# productTypeId 20001, loanKindId 2000
def data_loan_kind_2000(current_datetime_000z):
    return {"productTypeId": 20001,
            "loanKindId": 2000,
            "loanAmount": 2000000,
            "loanDuration": 300,
            "fixedRatePeriod": 60,
            "collateralAmount": 3000000,
            "paymentDay": None,
            "isEmployeeBonusRequested": False,
            "guaranteeDateFrom": current_datetime_000z,
            "financialResourcesOwn": 600000,
            "financialResourcesOther": 400000,
            "simulationToggleSettings": True,
            "interestRateDiscount": 0,
            "drawingType": 0,
            "drawingDuration": 0,
            "loanPurposes": [
                {"id": 202, "sum": 1500000},
                {"id": 204, "sum": 500000}
            ],
            "marketingActions":
                {"domicile": True,
                 "healthRiskInsurance": True,
                 "realEstateInsurance": False,
                 "incomeLoanRatioDiscount": False,
                 "userVip": False
                 },
            "resourceProcessId": "f64d3ea9-185c-4cc7-9d97-13fa31d3d967"}


json_req_loan_kind_2000 = json.dumps(data_loan_kind_2000())


# productTypeId 20001, loanKindId 2001
def data_loan_kind_2001(current_datetime_000z):
    return {"productTypeId": 20001,
            "loanKindId": 2001,
            "loanAmount": 2000000,
            "loanDuration": 300,
            "fixedRatePeriod": 60,
            "collateralAmount": 3000000,
            "paymentDay": None,
            "isEmployeeBonusRequested": False,
            "guaranteeDateFrom": current_datetime_000z,
            "financialResourcesOwn": 600000,
            "financialResourcesOther": 400000,
            "simulationToggleSettings": True,
            "interestRateDiscount": 0,
            "drawingType": 0,
            "drawingDuration": 0,
            "loanPurposes": [
                {"id": 202, "sum": 1500000},
                {"id": 204, "sum": 500000}
            ],
            "marketingActions":
                {"domicile": True,
                 "healthRiskInsurance": True,
                 "realEstateInsurance": False,
                 "incomeLoanRatioDiscount": False,
                 "userVip": False
                 },
            "resourceProcessId": "f64d3ea9-185c-4cc7-9d97-13fa31d3d967"}


json_req_loan_kind_2001 = json.dumps(data_loan_kind_2001())
