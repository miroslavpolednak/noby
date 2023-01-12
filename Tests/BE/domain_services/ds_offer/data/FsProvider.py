import os
import json

from pathlib import Path
from typing import List

PATH_TO_FOLDER_JSON: str = os.path.join(os.path.abspath(os.path.dirname(__file__)), 'json')

class FsProvider():
    def __init__(self, folder_path:str = PATH_TO_FOLDER_JSON):
        self._folder_path = folder_path
        
    def __str__ (self):
        return f'FsProvider [folder_path: {self._folder_path}]'

    def load(self)->List[dict]:
        list: List[dict] = []

        for dirpath, dirs, files in os.walk(self._folder_path):
            json_files = [ f for f in files if f.endswith( ('.json') ) ]
            for f_name in json_files:
                #print(f'File name: {filename}')
                f_path = os.path.join(dirpath, f_name)
                #print(f'File path: {f_path}')

                with open(f_path, 'r') as f:
                    f_content = f.read()
                    #print(f_content)
                    #json.loads(f_content)
                    f_json = json.loads(f_content)
                    #print(f_json)
                    list.append(dict( file = f_path, json_dict = f_json))

        return list
