from ...construct_api.uc_dashboard.post_case_search import post_case_search


def test_post_case_search(post_case_search):
    resp = post_case_search
    assert resp.status_code == 200, resp.content