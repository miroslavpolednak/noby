print("Package: data")

import json, os

f_path = os.path.join('D:\\Users\\992466q\\source\\repos\\OneSolution\\Tests\\BE\\tests\\tuning\\data', 'case.json')

def load_file_json(fila_path: str = f_path) -> dict:
    with open(fila_path, 'r') as f:
        f_content = f.read()
        f_json = json.loads(f_content)
        return f_json                    

# data = load_file_json()
# print(data)