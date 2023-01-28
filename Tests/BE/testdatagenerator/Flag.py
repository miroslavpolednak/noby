import json
import copy
from Pipeline_element import Pipeline_element

class Flag(Pipeline_element):
    def __init__(self,flag,prev=None,flag_eval=None):
        super().__init__(prev)
        self.flag=flag
        self.flag_eval=flag_eval

    def process(self,doc):
        doc_updated=copy.deepcopy(doc)
        if (self.flag_eval):
            doc_updated["__"][self.flag]=self.flag_eval(doc_updated)
        self.next.process(doc_updated)
