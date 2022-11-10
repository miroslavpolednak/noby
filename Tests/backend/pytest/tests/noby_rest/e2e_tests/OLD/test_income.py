import requests
import pytest


# TODO: dodelat dynamicke hodnoty pro URL CustomerOnSAId a datumy přes sysdate+x  a dodelani assertu
def test_post_income(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer-on-sa/77/income",
        cookies=get_cookies,
        json={"sum": 200000,
              "currencyCode": "CZK",
              "incomeTypeId": 1,
              "data":
                  {"employer":
                       {"name": "Pyramida",
                        "cin": "75487456",
                        "birthNumber": None,
                        "countryId": 16},
                   "job":
                       {"jobDescription": "superman",
                        "firstWorkContractSince": "2021-07-02T00:00:00.000Z",
                        "employmentTypeId": 1,
                        "currentWorkContractSince": "2022-05-03T00:00:00.000Z",
                        "currentWorkContractTo": "2032-07-13T00:00:00.000Z",
                        "grossAnnualIncome": 3000000,
                        "jobTrialPeriod": True,
                        "jobNoticePeriod": False
                        },
                   "wageDeduction":
                       {"deductionDecision": 123, "deductionPayments": 456, "deductionOther": 789},
                   "incomeConfirmation":
                       {"confirmationDate": "2022-07-14T00:00:00.000Z",
                        "confirmationPerson": "ucetni catwoman",
                        "confirmationContact": "775544551",
                        "confirmationByCompany": True
                        },
                   "proofOfIncomeToggle": True,
                   "foreignIncomeTypeId": None,
                   "wageDeductionToggle": True
                   }
              }
    )
    data = resp.json()
    print(data)
    assert resp.status_code == 200, resp.content


@pytest.mark.skip('Je treba dodelat tento resp s correct json requestem')
# TODO: dodelat dynamicke hodnoty pro URL CustomerOnSAId a incomeId(z POST výše) a datumy sysdate+x  a dodelani assertu
def test_put_income(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer-on-sa/76/income/62",
        cookies=get_cookies,
        json={"sum": 300000,
              "currencyCode": "CZK",
              "incomeTypeId": 1,
              "data":
                  {"employer":
                       {"name": "Pyramida",
                        "cin": "75487456",
                        "birthNumber": "",
                        "countryId": 16
                        },
                   "job":
                       {"jobDescription": "ironman",
                        "firstWorkContractSince": "2020-07-02T00:00:00.000Z",
                        "employmentTypeId": 1,
                        "currentWorkContractSince": "2022-05-03T00:00:00.000Z",
                        "currentWorkContractTo": "2024-07-13T00:00:00.000Z",
                        "grossAnnualIncome": 3000000,
                        "jobTrialPeriod": True,
                        "jobNoticePeriod": False
                        },
                   "wageDeduction":
                       {"deductionDecision": 123, "deductionPayments": 456, "deductionOther": 789
                        },
                   "incomeConfirmation":
                       {"confirmationDate": "2022-07-14T00:00:00.000Z",
                        "confirmationPerson": "ucetni catwoman",
                        "confirmationContact": "775544551",
                        "confirmationByCompany": True
                        },
                   "proofOfIncomeToggle": True,
                   "foreignIncomeTypeId": None,
                   "wageDeductionToggle": True
                   }
              }
    )
    data = resp.json()
    print(data)
    assert resp.status_code == 200, resp.content