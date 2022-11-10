import requests
import pytest


#TODO: provizorne caseId natvrdo
@pytest.fixture()
def get_case_tasks(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/case/2975950/tasks",
        cookies=get_cookies
    )
    return resp
    print(resp)

