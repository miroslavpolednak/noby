import json
from Pipeline_element import Pipeline_element

class Writer(Pipeline_element):
    def process(self,doc):
        print(json.dumps(doc,indent=4))
        if not ('cnt' in self.__dict__):
            self.cnt=0
        print('Json count:'+str(self.cnt))
        self.cnt+=1

        if ('next' in self.__dict__):
            self.next.process(doc)

