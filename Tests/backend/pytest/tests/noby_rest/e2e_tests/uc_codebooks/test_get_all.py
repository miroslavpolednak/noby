import pytest

from response.codebook_json import json_resp_academic_degrees_after, json_resp_paymentDays
from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks


#TODO: dodelat vsechy codebooky ze swaggeru
@pytest.mark.parametrize("codebook_name, expected_resp_json", [
    (
            Codebooks.PAYMENTDAYS,
            json_resp_paymentDays
    ),
    (
            Codebooks.ACADEMICDEGREESAFTER,
            json_resp_academic_degrees_after
    )]
)
def test_get_paymentdays(get_all_codebooks, codebook_name, get_cookies, webapi_url, expected_resp_json):
    resp = get_all_codebooks
    print(resp)

    assert resp[0] == 200
    assert resp[1] == codebook_name
    assert resp[2] == expected_resp_json
