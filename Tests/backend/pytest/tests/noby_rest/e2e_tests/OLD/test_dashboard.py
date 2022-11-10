import requests


def test_users(webapi_url, get_cookies):
    session = requests.session()
    resp = session.get(
        webapi_url + "/users",
        cookies=get_cookies
    )
    assert resp.status_code == 200, resp.content
    user = resp.json()
    assert user == {'cpm': '99917587', 'icp': '000017587', 'id': 267, 'name': 'Pavel Gronwaldt'}


#TODO: az bude funkcni vytvoreni CASE, zde bude kontrola pro vytvorenou zadost. Pote po odeslani JSON bude dalsi stav
def test_case_search(webapi_url, get_cookies):
    session = requests.session()
    resp = session.post(
        webapi_url + "/case/search",
        cookies=get_cookies,
        json=
        {"filterId": 1,
         "term": "",
         "pagination":
            {"descending": True,
             "page": 0,
             "pageSize": 10,
             "recordOffset": 0,
             "rowsPerPage": 10,
             "rowsNumber": 20,
             "sortBy": "stateUpdated"}}
    )
    data = resp.json()
    print(data)
    rows = data['rows']
