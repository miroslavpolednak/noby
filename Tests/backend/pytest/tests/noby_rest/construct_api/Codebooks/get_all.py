import requests
import pytest

from Tests.backend.pytest.tests.noby_rest.conftest import noby_sit1_url, get_noby_sit1_cookies


def get_all_codebooks(codebook_name):
    session = requests.session()
    resp = session.get(
         noby_sit1_url() + "/codebooks/get-all?q=" + codebook_name,
         cookies=get_noby_sit1_cookies()
    )
    data = resp.json()
    status_code = resp.status_code
    code = data[0]['code']
    codebook = data[0]['codebook']
    return status_code, code, codebook
