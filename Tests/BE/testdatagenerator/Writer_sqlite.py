import json
import datetime

import sqlite3
import time
from sqlite3 import Error

from Pipeline_element import Pipeline_element

class Writer_sqlite(Pipeline_element):
    def __init__(self,entity_type,prev=None,db_file='TestData.db'):
        super().__init__(prev)
        self.entity_type=entity_type
        self.db_file=db_file

    def process(self,doc):
        json_text=json.dumps(doc,indent=4)
        #json_text=json.dumps(doc)
        if not ('cnt' in self.__dict__):
            self.cnt=0
            self.timestamp=sqlite3.TimestampFromTicks(time.time())

        try:
            db = sqlite3.connect(self.db_file)
            cursor = db.cursor()
            sql = f"insert into TestData(TimeCreated, RecordOrder, RecordSource, EntityType, EntityData) values('{self.timestamp}',{self.cnt},'DATAGENERATOR','{self.entity_type}','{json_text}')"
            cursor.executescript(sql)

        except Error as e:
            print(e)

        finally:
            #Because we donn't know the last iteration and task is not mission critical, we open and close DB for every single record
            if db:
                db.close()        

        self.cnt+=1
        if ('next' in self.__dict__):
            self.next.process(doc)

