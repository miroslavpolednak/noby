import sqlite3

path_to_db: str = 'd:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\DB\\NobyTest.db'

path_to_sql: str = 'd:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\DB\\OfferService.sql'
path_to_sql_seed: str = 'd:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\DB\\OfferServiceSeed.sql'


def get_file_content(path_to_file: str):
    with open(path_to_file, 'r') as f:
        file_content = f.read()
        return file_content

# connect to DB
db = sqlite3.connect(path_to_db)
cursor = db.cursor()

# create db structure
sql = get_file_content(path_to_sql)
cursor.executescript(sql)

# insert data
sql_seed = get_file_content(path_to_sql_seed)
cursor.executescript(sql_seed)

# commit & close
db.commit()
db.close()