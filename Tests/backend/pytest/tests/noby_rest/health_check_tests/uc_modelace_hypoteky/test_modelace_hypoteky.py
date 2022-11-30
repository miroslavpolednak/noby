import pytest

from request.create_case.create_case_json import json_req_create_case
from request.simulation_mortgage.simulation_mortgage_basic_json import json_req_mortgage_basic_params
from ...construct_api.uc_modelace_hypoteky.post_offer_mortgage import post_offer_mortgage, post_offer_mortgage_basic
from ...construct_api.uc_modelace_hypoteky.get_offer_mortgage_offerid import get_offer_mortgage_basic
from ...construct_api.uc_modelace_hypoteky.post_create_case import post_create_case


@pytest.mark.parametrize("call_mortgage_json", [
    (
        json_req_mortgage_basic_params
    )]
)
def test_post_offer_mortgage(post_offer_mortgage):
    resp = post_offer_mortgage
    print(resp)
    assert resp.status_code == 200, resp.content


def test_get_offer_mortgage(get_offer_mortgage_basic):
    resp = get_offer_mortgage_basic
    resp = resp[1]
    assert resp.status_code == 200, resp.content


#@pytest.mark.parametrize("call_mortgage_json, call_create_case_json", [
 #   (
  #      json_req_mortgage_basic_params, json_req_create_case
   # )]
#)
def test_post_create_case():
    resp = post_create_case()
    assert resp.status_code == 200, resp.content
