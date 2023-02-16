import requests
import pytest


# varianta:
@pytest.fixture()
def post_create_case_old(webapi_url, get_cookies, post_offer_mortgage_loan_kind_2000):
    resp = post_offer_mortgage_loan_kind_2000
    data = resp.json()
    offer_id = data['offerId']
    session = requests.session()
    resp = session.post(
        webapi_url + "/offer/mortgage/create-case",
        cookies=get_cookies,
        json=
        {"offerId": offer_id,
         "firstName": "KLIENT",
         "lastName": "TESTOVACI",
         "dateOfBirth": "1967-08-30T00:00:00",
         "identity": {"id": 113666423, "scheme": 2}}
    )
    return resp
