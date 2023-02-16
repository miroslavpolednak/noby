import requests
import pytest
from .post_offer_mortgage import post_offer_mortgage_basic, post_offer_mortgage_basic_2


@pytest.fixture()
def get_offer_mortgage_basic(webapi_url, get_cookies, post_offer_mortgage_basic):
    offer_id = post_offer_mortgage_basic.json()['offerId']
    session = requests.session()
    resp = session.get(
        webapi_url + f"/offer/mortgage/{offer_id}",
        cookies=get_cookies
    )
    return offer_id, resp


def get_offer(mortgage):
    offer = post_offer_mortgage_basic_2(mortgage)
    offer_id = offer.json()['offerId']
    return offer_id


