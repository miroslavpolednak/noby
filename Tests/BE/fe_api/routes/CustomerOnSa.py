from ._Base import Base

class CustomerOnSa(Base):

    def __init__(self):
        super().__init__(route='customer-on-sa')