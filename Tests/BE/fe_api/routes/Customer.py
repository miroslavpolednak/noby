from ._Base import Base

class Customer(Base):

    def __init__(self):
        super().__init__(route='customer')