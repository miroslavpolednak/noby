from Attribute import Attribute

class Attribute_loop(Attribute):
    def __init__(self,path,prev=None,values=[None]):
        super().__init__(path,prev)
        self.values=values
        self.indexes=[]
        for i in self.values:
            self.indexes.append(0)

    def get_next_value(self,doc):
        i=0
        for values in self.values:
            yield self.eval(doc,values[self.indexes[i]])
            self.indexes[i]=((self.indexes[i]+1) if (self.indexes[i] < len(values)-1) else 0)
            i+=1
        return None
      

