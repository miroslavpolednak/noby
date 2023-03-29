from ._Base import Base

class Document(Base):

    def __init__(self):
        super().__init__(route='document')