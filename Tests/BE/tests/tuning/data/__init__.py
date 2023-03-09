print("Package: data")

import json, os

from .EInputOutput import EInputOutput

ENCODING='utf-8'

folder_data = os.path.dirname(__file__)

def load_json(file_name: str, folder_io: EInputOutput) -> dict:
    file_path = os.path.join(folder_data, folder_io.value,  file_name)
    assert os.path.isfile(file_path) is True, 'File not found!'
    with open(file_path, 'r', encoding = ENCODING) as f:
        f_content = f.read()
        f_json = json.loads(f_content)
        return f_json                    

def save_json(file_name: str, folder_io: EInputOutput, json_data: dict):
    folder_path = os.path.join(folder_data, folder_io.value)
    file_path = os.path.join(folder_path,  file_name)
    assert os.path.isfile(file_path) is False, 'File already exists!'

    if not os.path.exists(folder_path):
        os.makedirs(folder_path)
    
    with open(file_path, 'w', encoding = ENCODING) as f:
        json.dump(json_data, f)       

def clean_folder(folder_io: EInputOutput = EInputOutput.Output):
    folder_path = os.path.join(folder_data, folder_io.value)
    for file_name in os.listdir(folder_path):
        file_path = os.path.join(folder_path, file_name)
        os.remove(file_path)
    
# data = load_json('case.json')
# print(data)