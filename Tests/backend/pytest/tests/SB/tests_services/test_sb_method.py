import pytest
import requests
from requests import Session
import response as response
import zeep
from zeep import Client, Transport

from Tests.backend.pytest.tests.SB.conftest import sb_api_url
from Tests.backend.pytest.tests.SB.conftest import get_current_date


def test_get_dod():
    session = Session()
    session.verify = False
    client = Client('http://adpra136.vsskb.cz/FAT/EAS_WS_SB_Services.svc?wsdl', transport=Transport(session=session))

    result = client.service.GetDOD()
    reformat_result = result.strftime("%Y-%m-%dT%H:%M:%S")
    assert reformat_result == get_current_date()


def test_get_Case_id():
    session = Session()
    session.verify = False
    client = Client('http://adpra136.vsskb.cz/FAT/EAS_WS_SB_Services.svc?wsdl', transport=Transport(session=session))
    result = client.service.Get_CaseId(mandant=2, productCode=20001)