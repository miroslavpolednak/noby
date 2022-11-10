import requests
import pytest
from request.customer_search_json import json_req_customer_search_lastname


@pytest.fixture()
def post_customer_search_lastname(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/customer/search",
        cookies=get_cookies,
        json=json_req_customer_search_lastname
    )
    return resp

