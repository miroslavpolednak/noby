import requests

from Tests.backend.pytest.tests.noby_rest.conftest import noby_sit1_url, noby_fat_url, get_noby_sit1_cookies, get_noby_fat_cookies
from Tests.backend.pytest.tests.noby_rest.json.request.create_case.create_case_json import json_req_create_case


def post_create_case(call_create_case_json):
    session = requests.session()
    resp = session.post(
        noby_fat_url() + "/offer/mortgage/create-case",
        cookies=get_noby_fat_cookies(),
        json=call_create_case_json
    )
    return resp