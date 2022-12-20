import requests

from Tests.backend.pytest.tests.noby_rest.conftest import noby_sit1_url, get_noby_sit1_cookies
from request.create_case.create_case_json import json_req_create_case


def post_create_case():
    session = requests.session()
    resp = session.post(
        noby_sit1_url() + "/offer/mortgage/create-case",
        cookies=get_noby_sit1_cookies(),
        json=json_req_create_case
    )
    return resp