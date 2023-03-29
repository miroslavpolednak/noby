print("Package: ds_offer")

from .data.DbProvider import DbProvider
from .data.FsProvider import FsProvider
from .model import Offer

db_provider: DbProvider = DbProvider()
fs_provider: FsProvider = FsProvider()
