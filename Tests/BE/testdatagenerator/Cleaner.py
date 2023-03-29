import copy
from Pipeline_element import Pipeline_element

class Cleaner(Pipeline_element):
    #TODO: clean arrays
    def clean_struct(self,struct):
        keys=list(struct.keys())
        for atr in keys:
            if (atr[0]=="_" and atr[1]=="_"):
                del struct[atr]
            elif (type(struct[atr]) == type({'a':'b'})):
                self.clean_struct(struct[atr])



    def process(self,doc):
        doc_updated=copy.deepcopy(doc)
        del doc_updated["__"]
        self.clean_struct(doc_updated)
        self.next.process(doc_updated)
