
from ...construct_api.uc_dashboard.get_case_dashboard_filters import get_case_dashboard


def test_get_dashboard_filters(get_case_dashboard):
    resp = get_case_dashboard
    assert resp.status_code == 200, resp.content