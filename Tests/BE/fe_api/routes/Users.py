from ._Base import Base

class Users(Base):

    def __init__(self):
        super().__init__(route='users')