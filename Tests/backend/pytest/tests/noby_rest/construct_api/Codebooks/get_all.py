import requests
import pytest

from Tests.backend.pytest.tests.noby_rest.conftest import noby_sit1_url, get_noby_sit1_cookies, noby_dev_url, \
    noby_fat_url, get_noby_dev_cookies, get_noby_fat_cookies


def get_all_codebooks(codebook_name):
    session = requests.session()
    resp = session.get(
        noby_dev_url() + "/codebooks/get-all?q=" + codebook_name,
        cookies=get_noby_dev_cookies()
    )
    data = resp.json()
    status_code = resp.status_code
    code = data[0]['code']
    codebook = data[0]['codebook']
    return status_code, code, codebook


def get_codebooks(codebook_name, product_type):
    session = requests.session()
    resp = session.get(
        noby_dev_url() + "/codebooks/" + codebook_name,
        params={
            "productTypeId": product_type
        },
        cookies=get_noby_dev_cookies()
    )
    data = resp.json()
    status_code = resp.status_code
    return status_code, data


def get_codebooks_fixation_period_length(product_type):
    session = requests.session()
    resp = session.get(
        noby_dev_url() + "/codebooks/fixation-period-length",
        params={
            "productTypeId": product_type
        },
        cookies=get_noby_dev_cookies()
    )
    data = resp.json()
    status_code = resp.status_code
    return status_code, data