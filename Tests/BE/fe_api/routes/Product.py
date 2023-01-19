from ._Base import Base

class Product(Base):

    def __init__(self):
        super().__init__(route='product')