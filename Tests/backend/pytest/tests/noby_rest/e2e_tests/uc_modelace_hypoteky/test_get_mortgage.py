import pytest
from ...construct_api.uc_modelace_hypoteky.get_offer_mortgage_offerid import get_offer_mortgage_basic


def test_get_offer_mortgage(get_offer_mortgage_basic):
    resp = get_offer_mortgage_basic
    resp = resp[1]
    assert resp.status_code == 200, resp.content