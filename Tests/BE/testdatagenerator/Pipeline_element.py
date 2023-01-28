import json

class Pipeline_element:
    def __init__(self,prev=None):
        if(prev):
            prev.register_next(self)
        self.register_prev(prev)

    def register_prev(self,prev):
        self.prev=prev

    def register_next(self,next):
        self.next=next
