import sqlite3

path_to_db: str = 'd:\\Users\\992466q\\source\\repos\\OneSolutionTests\\InputData\\SQLite\\DB\\NobyTest.db'
path_to_sql: str = 'd:\\Users\\992466q\\source\\repos\\OneSolutionTests\\InputData\\SQLite\\DB\\OfferService.sql'

with open(path_to_sql, 'r') as sql_file:
    sql_script = sql_file.read()

db = sqlite3.connect(path_to_db)
cursor = db.cursor()
cursor.executescript(sql_script)
db.commit()
db.close()