import requests
import pytest
from request.kbid_scheme_json import json_req_kbid_true


@pytest.fixture()
def post_customer_get(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer/get",
        cookies=get_cookies,
        json=json_req_kbid_true
    )
    return resp

