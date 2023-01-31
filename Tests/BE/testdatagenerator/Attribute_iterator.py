from Attribute import Attribute

class Attribute_iterator(Attribute):
    def __init__(self,path,prev=None,values=[None]):
        super().__init__(path,prev)
        self.values=values

    def get_next_value(self,doc):
        for value in self.values:
            yield self.eval(doc,value)
        return None

