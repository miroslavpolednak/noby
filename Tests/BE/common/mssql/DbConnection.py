class DbConnection():
    def __init__(self, server: str, database: str, user: str, password: str):
        self._server = server
        self._database = database
        self._user = user
        self._password = password
        
    def __str__ (self):
        return f'DbConnection [server: {self._server}, database: {self._database}]'

    @property
    def server(self) -> str:
        return self._server

    @property
    def database(self) -> str:
        return self._database

    def to_odbc_connection_string(self)->str:
        template = 'DRIVER={ODBC Driver 17 for SQL Server};SERVER=<server>;DATABASE=<db>;uid=<login>;pwd=<pwd>;application name=DS_SalesArrangementService;TrustServerCertificate=Yes;'
        return template.replace('<server>', self._server).replace('<db>', self._database).replace('<login>', self._user).replace('<pwd>', self._password)