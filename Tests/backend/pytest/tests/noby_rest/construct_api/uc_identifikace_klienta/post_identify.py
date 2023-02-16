import requests
import pytest
from request.customer_identify_json import json_req_customer_identify_full


@pytest.fixture()
def post_customer_identify_success(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer/identify",
        cookies=get_cookies,
        json=json_req_customer_identify_full
    )
    return resp
