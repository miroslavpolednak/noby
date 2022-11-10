import requests
import pytest


#TODO: provizorne caseId natvrdo
@pytest.fixture()
def get_sa_list_caseid(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/sales-arrangement/list/2975950",
        cookies=get_cookies
    )
    return resp
    print(resp)

