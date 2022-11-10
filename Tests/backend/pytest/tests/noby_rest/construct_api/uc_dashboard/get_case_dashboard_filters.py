import requests
import pytest


@pytest.fixture()
def get_case_dashboard(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/case/dashboard-filters",
        cookies=get_cookies
    )
    return resp

