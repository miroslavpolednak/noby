import requests
import pytest

from Tests.backend.pytest.tests.noby_rest.conftest import noby_fat_url, get_noby_fat_cookies, noby_dev_url, \
    get_noby_dev_cookies
from request.customer_search_json import json_req_customer_search_lastname


def post_customer_search(search_patern):
    session = requests.session()
    resp = session.post(
        noby_dev_url() + "/customer/search",
        cookies=get_noby_dev_cookies(),
        json=search_patern
    )
    return resp

