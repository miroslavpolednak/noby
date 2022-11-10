from ...construct_api.uc_identifikace_klienta.post_identify import post_customer_identify_success
from ...json.response.customer_identify_json import json_resp_customer_identify_success


def test_post_customer_get(post_customer_identify_success):
    resp = post_customer_identify_success
    assert resp.status_code == 200, resp.content
    data = resp.json()
    assert data == json_resp_customer_identify_success
