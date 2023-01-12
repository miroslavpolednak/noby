import pyodbc
from typing import List
from pyodbc import Cursor, Row
from .DbConnection import DbConnection


# # Trusted Connection to Named Instance
# #connection = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER=adpra173.vsstest.local;DATABASE=CIS;uid=testsql;pwd=Rud514;application name=DS_SalesArrangementService;TrustServerCertificate=Yes;')

# connection = pyodbc.connect('SERVER=adpra173.vsstest.local;DATABASE=CIS;uid=testsql;pwd=Rud514;application name=DS_SalesArrangementService;TrustServerCertificate=Yes;')

# cursor=connection.cursor()

# cursor.execute("SELECT @@VERSION as version")

# while 1:
#     row = cursor.fetchone()
#     if not row:
#         break
#     print(row.version)

# #server=adpra173.vsstest.local;database=SalesArrangementService;uid=testsql;pwd=Rud514;application name=DS_SalesArrangementService;Encrypt=True;TrustServerCertificate=Yes;",


class DbManager():
    def __init__(self, db_connection: DbConnection):
        self._db_connection = db_connection
        
    def __str__ (self):
        return f'DbManager [connection: {self._db_connection}]'

    @property
    def db_connection(self) -> str:
        return self._db_connection

    def cursor_open(self)->Cursor:
        #connection = pyodbc.connect('SERVER=adpra173.vsstest.local;DATABASE=CIS;uid=testsql;pwd=Rud514;application name=DS_SalesArrangementService;TrustServerCertificate=Yes;')
        connection = pyodbc.connect(self._db_connection.to_odbc_connection_string())
        return connection.cursor()

    def cursor_close(self, cursor: Cursor):
        cursor.close()
        #cursor.connection.close()

    def exec_query(self, query: str)->List[Row]:
        
        rows: List[Row] = []

        cursor = self.cursor_open()
        cursor.execute(query)

        while 1:
            row = cursor.fetchone()
            if not row:
                break
            rows.append(row)

        self.cursor_close(cursor)

        return rows

    def get_version(self)->str:
        rows = self.exec_query("SELECT @@VERSION as version")
        return None if len(rows) != 1 else rows[0].version
