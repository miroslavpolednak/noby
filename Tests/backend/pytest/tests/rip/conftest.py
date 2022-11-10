import datetime

import datetime as datetime
import pytest
import requests
import pyodbc
from sqlalchemy.ext.declarative import declarative_base
Base = declarative_base()


def pytest_addoption(parser):
    parser.addoption("--webapi-url", action="store", default="https://noby-fat.vsskb.cz/api", help="web api url")
    parser.addoption("--dev-rip-url", action="store", default="https://dev.noby.cz:5004/v2", help="rip dev url")
    parser.addoption("--cpm", action="store", default="99917587", help="client id")


@pytest.fixture(scope="session")
def webapi_url(request):
    return request.config.getoption("--webapi-url")


@pytest.fixture(scope="session")
def rip_api_url(request):
    return request.config.getoption("--dev-rip-url")


@pytest.fixture(scope="session")
def cpm(request):
    return request.config.getoption("--cpm")


@pytest.fixture(scope="session")
def get_cookies(webapi_url, cpm):
    session = requests.session()
    session.post(
        webapi_url + "/users/signin",
        json={
            "Login": cpm
        }
    )
    return session.cookies


@pytest.fixture(scope="session")
def noby_fat_connection():
    connection = pyodbc.connect(driver='{SQL Server Native Client 11.0}',
                                server='adpra173.vsstest.local\\FAT',
                                database='CaseService',
                                uid='testsql', pwd='Rud514')
    connection.autocommit = True
    yield connection
    connection.close()


@pytest.fixture()
def noby_fat_db_cursor(noby_fat_connection):
    cursor = noby_fat_connection.cursor()
    yield cursor
    cursor.close()


@pytest.fixture(scope="session")
def konsdb_fat_connection():
    connection = pyodbc.connect(driver='{SQL Server Native Client 11.0}',
                                server='callisto',
                                database='KonsDb_L1_HFFAT',
                                uid='testsql', pwd='Rud514')
    connection.autocommit = True
    yield connection
    connection.close()


@pytest.fixture()
def konsdb_fat_db_cursor(konsdb_fat_connection):
    cursor = konsdb_fat_connection.cursor()
    yield cursor
    cursor.close()


@pytest.fixture()
def current_datetime_000z():
    return datetime.datetime.now().strftime("%Y-%m-%dT%H:%M:%S.000Z")


@pytest.fixture()
def tomorrow_datetime():
    tomorrow = (datetime.date.today() + datetime.timedelta(days=1))
    return tomorrow.strftime("%Y-%m-%dT%H:%M:%S")


@pytest.fixture()
def guaranteeDateTo_datetime():
    guarantee_date_from = (datetime.date.today() + datetime.timedelta(days=45))
    return guarantee_date_from.strftime("%Y-%m-%dT%H:%M:%S")

@pytest.fixture()
def guaranteeDateFrom_datetime():
    guarantee_date_from = (datetime.date.today())
    return guarantee_date_from.strftime("%Y-%m-%dT%H:%M:%S")
