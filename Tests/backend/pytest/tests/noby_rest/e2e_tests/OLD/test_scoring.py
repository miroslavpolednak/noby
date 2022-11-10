import requests


# TODO: dodelat dynamicke SAid a dodelani assertu
def test_GET_loan_application_assesment(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/sales-arrangement/71/loan-application-assessment",
        cookies=get_cookies
    )
    data = resp.json()
    print(data)
    assert resp.status_code == 200, resp.content
