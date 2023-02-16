import pytest

from request.customer_search_json import json_req_customer_search_lastname, json_req_customer_search_date_of_birth
from ...construct_api.uc_identifikace_klienta.post_search import post_customer_search
from ...construct_api.uc_identifikace_klienta.post_profile_check import post_customer_profile_check_true
from ...construct_api.uc_identifikace_klienta.post_get import post_customer_get
from ...construct_api.uc_identifikace_klienta.post_identify import post_customer_identify_success


@pytest.mark.parametrize("search_patern", [
    json_req_customer_search_lastname,
    json_req_customer_search_date_of_birth
])
def test_post_customer_search(search_patern):
    resp = post_customer_search(search_patern)
    assert resp.status_code == 200, resp.content


def test_post_customer_profile_check_true(post_customer_profile_check_true):
    resp = post_customer_profile_check_true
    assert resp.status_code == 200, resp.content


def test_post_customer_get(post_customer_get):
    resp = post_customer_get
    assert resp.status_code == 200, resp.content


def test_post_customer_identify(post_customer_identify_success):
    resp = post_customer_identify_success
    assert resp.status_code == 200, resp.content