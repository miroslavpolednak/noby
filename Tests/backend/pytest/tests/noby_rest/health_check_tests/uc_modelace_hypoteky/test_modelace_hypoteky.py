import pytest

from request.simulation_mortgage.simulation_mortgage_basic_json import json_req_mortgage_basic_params
from request.create_case.create_case_json import json_req_create_case
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


@pytest.mark.parametrize("call_create_case_json", [
    (
        json_req_create_case(json_req_mortgage_basic_params)
    )]
)
def test_post_create_case(call_create_case_json):
    resp = post_create_case(call_create_case_json)
    print(resp)
    assert resp.status_code == 200, resp.content
