import datetime

import datetime as datetime
import pytest
import requests
import pyodbc
from sqlalchemy.ext.declarative import declarative_base
Base = declarative_base()

# pylint: disable=unused-import

def pytest_addoption(parser):
    parser.addoption("--ns--dev-url", action="store", default="https://ds-notification-dev.vsskb.cz:30016", help="ns dev url")
    parser.addoption("--username", action="store", default="XX_INSG_RMT_USR_TEST", help="username for authenticatiion")
    parser.addoption("--password", action="store", default="dyasrykgSDSGNFN!!2dvcxgrttrrthyy", help="psw for auth")


@pytest.fixture(scope="session")
def ns_url(request):
    url = request.config.getoption("--ns--dev-url")
    username = request.config.getoption("--username")
    password = request.config.getoption("--password")
    return {
        "url": url,
        "auth": (username, password)
    }

@pytest.fixture()
def tomorrow_datetime():
    tomorrow = (datetime.date.today() + datetime.timedelta(days=1))
    return tomorrow.strftime("%Y-%m-%dT%H:%M:%S")


def get_current_date():
    today = (datetime.date.today())
    reformat_today = today.strftime("%Y-%m-%dT%H:%M:%S")
    return reformat_today
