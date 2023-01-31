import json
from Pipeline_element import Pipeline_element

class Pipeline_root(Pipeline_element):
    def process(self):
        self.next.process(json.loads('{"__":{}}'))
    
