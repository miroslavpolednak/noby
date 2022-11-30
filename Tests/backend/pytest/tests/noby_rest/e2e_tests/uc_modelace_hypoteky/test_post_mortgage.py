import pytest
from request.simulation_mortgage.simulation_mortgage_basic_json import json_req_mortgage_basic_params
from ...conftest import noby_fat_db_cursor
from ...construct_api.uc_modelace_hypoteky.post_offer_mortgage import post_offer_mortgage
from ...db_util import select_offer_fat


@pytest.mark.parametrize("call_mortgage_json", [
    (
            json_req_mortgage_basic_params
    )]
                         )
def test_post_offer_mortgage(post_offer_mortgage, noby_fat_db_cursor):
    resp = post_offer_mortgage
    assert resp.status_code == 200, resp.content
    data = resp.json()
    offer_id = data['offerId']

    # Chybi dodelat kontroly response API

    # kontrola v databazi
    output = select_offer_fat(noby_fat_db_cursor, offer_id)
    db_id = output.OfferId
    assert offer_id == db_id
