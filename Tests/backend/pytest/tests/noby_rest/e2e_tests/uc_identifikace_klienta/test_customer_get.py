from ...construct_api.uc_identifikace_klienta.post_get import post_customer_get
from ...json.response.customer_get_json import json_resp_customer_get


def test_post_customer_get(post_customer_get):
    resp = post_customer_get
    assert resp.status_code == 200, resp.content
    data = resp.json()
    assert data == json_resp_customer_get
