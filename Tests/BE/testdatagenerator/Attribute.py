import copy

from Pipeline_element import Pipeline_element

class Attribute(Pipeline_element):
    def __init__(self,path,prev=None):
        super().__init__(prev)
        self.path=path
        self.filters=[]
    
    @staticmethod
    def parse_element(el):
        if(el[0]=='['):
            return int(el[1:-1])
        return el

    def add_to_doc(self,doc,value,count):
        path=self.path.split('.')
        current_element=doc
        for path_element in path[:-1:]:
            current_element=current_element[Attribute.parse_element(path_element)]
        path_element=path[-1]

        path_element=Attribute.parse_element(path_element)
        if (type(path_element) == type(1)):
            while (len(current_element)<=path_element):
                current_element.append(None)
        
        current_element[path_element]=value
        
        #Todo append metadata for array elements
        if (type(path_element) != type(1)):
            current_element["__"+path_element]={}
            meta=current_element["__"+path_element]
            meta["count"]=count

    
    def process(self,doc):
        count=0
        for value in self.get_next_value(doc):
            doc_updated=copy.deepcopy(doc)
            self.add_to_doc(doc_updated,value,count)
            #We want to filter just if filters are defined
            if (self.filters):
                for filter in self.filters:
                    if(filter(doc_updated)):
                        self.next.process(doc_updated)
                        break
            else:
                self.next.process(doc_updated)
            count+=1

    def register_filter(self,filter):
        self.filters.append(filter)

    def eval(self,doc,val):
        return val
    
    def register_eval(self,eval):
        self.eval=eval