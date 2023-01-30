from Attribute import Attribute

class Attribute_array(Attribute):
    def get_next_value(self,doc):
        yield []
        return None


