import concurrent.futures
import sqlite3
import requests
import pyodbc
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

from Tests.backend.pytest.tests.notification_service.conftest import URLS
from Tests.backend.pytest.tests.notification_service.json.request.mail_mpss_json import \
    json_req_mail_mpss_full_attachments


def mssql_connect(server, database, username, password):
    connection_string = f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={server};DATABASE={database};UID={username};PWD={password};PORT=1433'
    return pyodbc.connect(connection_string)

def run_test(url_name, auth_params, json_data, mssql_connection_params):
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=auth_params,
        verify=False
    )
    notification = resp.json()
    print(notification)
    notification_id = notification.get("notificationId")

    if notification_id:
        # Připojení k MSSQL databázi
        mssql_connection = mssql_connect(**mssql_connection_params)
        cursor = mssql_connection.cursor()

        # Aktualizace a ověření
        table_name = 'EmailResult'
        schema_name = 'dbo'
        cursor.execute(f"UPDATE {schema_name}.{table_name} SET State = 2 WHERE Id = ?", (notification_id,))
        mssql_connection.commit()

        cursor.execute(f"SELECT Id, State FROM {schema_name}.{table_name} WHERE Id = ?", (notification_id,))
        row = cursor.fetchone()
        mssql_connection.close()

        return notification_id, row[1] if row else None

    return None, None

# Vytvoření SQLite databáze
connection = sqlite3.connect('notifications.db')
cursor = connection.cursor()
cursor.execute('CREATE TABLE IF NOT EXISTS notifications (notificationId TEXT PRIMARY KEY, state INTEGER)')
connection.commit()

# Parametry pro test
url_name = "fat_url"
auth_params = ("XX_EPSY_RMT_USR_TEST_username", "XX_EPSY_RMT_USR_TEST_password")
json_data = json_req_mail_mpss_full_attachments
mssql_connection_params = {"server": "fat", "database": "ns", "username": "username", "password": "password"}

# Paralelní spuštění testů
with concurrent.futures.ThreadPoolExecutor(max_workers=2) as executor:
    futures = [executor.submit(run_test, url_name, auth_params, json_data, mssql_connection_params) for _ in range(10)]
    for future in concurrent.futures.as_completed(futures):
        notification_id, state = future.result()
        if notification_id:
            cursor.execute("INSERT INTO notifications (notificationId, state) VALUES (?, ?)", (notification_id, state))
            connection.commit()

# Uzavření databáze
connection.close()
