from datetime import datetime, timedelta

from ._Modificator import Modificator

class ModificatorDate(Modificator):

    def __init__(self):
        super().__init__(regex='^Date\(days=-?\d+\)')

    def modify(self, days: str):
        date: datetime = datetime.today().date() + timedelta(days=int(days))
        return date.isoformat()