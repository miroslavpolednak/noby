import requests
import pytest
from request.kbid_scheme_json import json_req_kbid_true


@pytest.fixture()
def post_customer_profile_check_true(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer/profile-check",
        cookies=get_cookies,
        json=json_req_kbid_true
    )
    return resp

