import requests
import pytest
from request.case_search_json import json_req_case_search


@pytest.fixture()
def post_case_search(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/case/search",
        cookies=get_cookies,
        json=json_req_case_search
    )
    return resp

