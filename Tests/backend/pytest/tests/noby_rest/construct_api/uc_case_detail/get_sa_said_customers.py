import requests
import pytest


#TODO: provizorne SAId natvrdo
@pytest.fixture()
def get_sa_said_customers(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/sales-arrangement/14/customers",
        cookies=get_cookies
    )
    return resp
    print(resp)

