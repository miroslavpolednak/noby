from datetime import datetime, timedelta

from .IModificator import IModificator

class ModificatorDate(IModificator):

    def __init__(self):
        super().__init__(name = __name__)

    def modify(self, days):
        date: datetime = datetime.today().date() + timedelta(days=int(days))
        return date.isoformat()