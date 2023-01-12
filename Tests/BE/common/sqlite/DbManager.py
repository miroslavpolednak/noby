import sqlite3
from typing import List

class DbManager():
    def __init__(self, path_to_db: str):
        self._path_to_db = path_to_db
        
    def __str__ (self):
        return f'DbManager [db: {self._path_to_db}]'

    @property
    def path_to_db(self) -> str:
        return self._path_to_db

    @staticmethod
    def get_file_content(path_to_file: str):
        file_content = None
        with open(path_to_file, 'r') as f:
            file_content = f.read()
        return file_content

    def create_connection(self):
        connection = None
        try:
            connection = sqlite3.connect(self._path_to_db)
        except Exception as e:
            print(e)

        return connection

    def exec_scripts(self, file_paths: List[str]):
        # connect to DB
        db = sqlite3.connect(self._path_to_db)
        cursor = db.cursor()

        for path_to_sql in file_paths:

            # load content
            sql = DbManager.get_file_content(path_to_sql)

            #print(f'path_to_sql> {path_to_sql}')
            #print(sql)

            # exec
            cursor.executescript(sql)

        # commit & close
        db.commit()
        db.close()

    def exec_query(self, query: str)->List[dict]:
        data = None
        columns = None

        with self.create_connection() as connection:
            cursor = connection.cursor()
            cursor.execute(query)

            # get column names
            # cursor.description: (('MortgageSimulationInputsPk', None, None, None, None, None, None), ('ExpectedDateOfDrawing', None, None, None, None, None, None), . . . )
            columns = list(map(lambda d: d[0], cursor.description))

            # fetch all data
            data = cursor.fetchall()

        # map data to dicts
        rows: List[dict] = []
        for i in data:
            r = {}
            for index, c in enumerate(columns):
                r[c] = i[index]
            rows.append(r)
            
        return rows

