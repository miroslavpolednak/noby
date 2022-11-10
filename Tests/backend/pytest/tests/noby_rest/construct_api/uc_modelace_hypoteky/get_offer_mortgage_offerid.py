import requests
import pytest


@pytest.fixture()
def get_offer_mortgage_basic(webapi_url, get_cookies, post_offer_mortgage_basic):
    offer_id = post_offer_mortgage_basic.json()['offerId']
    session = requests.session()
    resp = session.get(
        webapi_url + f"/offer/mortgage/{offer_id}",
        cookies=get_cookies
    )
    return offer_id, resp