import datetime

import datetime as datetime
import pytest
import requests
import pyodbc
from sqlalchemy.ext.declarative import declarative_base
Base = declarative_base()

# pylint: disable=unused-import


def pytest_addoption(parser):
    parser.addoption("--webapi-url", action="store", default="https://adpra136.vsskb.cz/FAT", help="web api url")
    # parser.addoption("--cpm", action="store", default="99917587", help="client id")


@pytest.fixture(scope="session")
def sb_api_url(request):
    return request.config.getoption("--webapi-url")


@pytest.fixture()
def tomorrow_datetime():
    tomorrow = (datetime.date.today() + datetime.timedelta(days=1))
    return tomorrow.strftime("%Y-%m-%dT%H:%M:%S")


def get_current_date():
    today = (datetime.date.today())
    reformat_today = today.strftime("%Y-%m-%dT%H:%M:%S")
    return reformat_today
