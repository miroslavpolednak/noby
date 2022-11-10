

from ...construct_api.uc_case_detail.get_case_caseid import get_case_caseid
from ...construct_api.uc_case_detail.get_case_tasks import get_case_tasks
from ...construct_api.uc_case_detail.get_sa_list_caseid import get_sa_list_caseid
from ...construct_api.uc_case_detail.get_sa_said_customers import get_sa_said_customers
from ...construct_api.uc_case_detail.get_sa_said import get_sa_said


def test_get_case_caseid(get_case_caseid):
    resp = get_case_caseid
    assert resp.status_code == 200, resp.content


def test_get_case_tasks(get_case_tasks):
    resp = get_case_tasks
    assert resp.status_code == 200, resp.content


def test_get_sa_list_caseid(get_sa_list_caseid):
    resp = get_sa_list_caseid
    assert resp.status_code == 200, resp.content


def test_get_sa_said_customers(get_sa_said_customers):
    resp = get_sa_said_customers
    assert resp.status_code == 200, resp.content


def test_get_sa_said(get_sa_said):
    resp = get_sa_said
    assert resp.status_code == 200, resp.content
