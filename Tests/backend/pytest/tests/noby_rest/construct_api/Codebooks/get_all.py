import requests
import pytest


@pytest.fixture()
def get_all_codebooks(webapi_url, get_cookies, codebook_name):
    session = requests.session()
    resp = session.get(
        webapi_url + "/codebooks/get-all?q=" + codebook_name,
        cookies=get_cookies
    )
    data = resp.json()
    status_code = resp.status_code
    code = data[0]['code']
    codebook = data[0]['codebook']
    return status_code, code, codebook
