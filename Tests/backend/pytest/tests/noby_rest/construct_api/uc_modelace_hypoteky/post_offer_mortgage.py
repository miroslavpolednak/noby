import requests
import pytest

from request.simulation_mortgage.simulation_mortgage_basic_json import json_req_mortgage_basic_params


@pytest.fixture()
def post_offer_mortgage(webapi_url, get_cookies, call_json):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage",
        cookies=get_cookies,
        json=call_json
    )
    return resp


@pytest.fixture()
def post_offer_mortgage_basic(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage",
        cookies=get_cookies,
        json=json_req_mortgage_basic_params
    )
    return resp





###OLD###
# varianta: productTypeId 20001, lonKindId 2000, 2 loanPurposes 202 a 204
### je třeba dodelat JSONy zvlast do slozek
@pytest.fixture()
def post_offer_mortgage_loan_kind_2000(webapi_url, get_cookies, current_datetime_000z):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage",
        cookies=get_cookies,
        json={"productTypeId": 20001,
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
    )
    return resp


# varianta: productTypeId 20001, lonKindId 2001, 2 loanPurposes 202 a 204
@pytest.fixture()
def post_offer_mortgage_loan_kind_2001(webapi_url, get_cookies, current_datetime_000z):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage",
        cookies=get_cookies,
        json={"productTypeId": 20001,
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
    )
    return resp


#TODO: předpříprava json plnění dle jsonu
@pytest.fixture()
def post_offer_mortgage(webapi_url, get_cookies, call_json):
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage",
        cookies=get_cookies,
        json=call_json
    )
    return resp