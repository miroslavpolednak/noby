import json
from logging import WARNING, DEBUG
from requests import cookies, Response, request

from common import config, Log

class Base():

    __default_login = '990614w' #!?
    __cookies_by_login: dict = dict()

    def __init__(self, route: str):
        self._log = Log.getLogger(f'FeApi.{self.class_name}')
        assert route is not None, f'Route must be provided!'
        self.__route = route

    @property
    def class_name(self) -> object:
        return self.__class__.__name__

    def __build_url(self, route: str):
        return f'{config.fe_api_url}/{self.__route}/{route}'

    def __handle_request(self, method: str, route: str, login: str = None, json: dict = None) -> Response:

        res: Response = None

        try:

            if (login is None):
                login = Base.__default_login
        
            url = self.__build_url(route)
            cookies = Base.get_cookies(login)
            res = request(url=url, method=method, cookies=cookies, json=json)

            log_level: int = DEBUG if res.status_code == 200 else WARNING
            self._log.log(log_level, f'HandleRequest [url: {url}, method: {method}, login: {login}, status_code: {res.status_code}]]')

            print(f'FeAPI.HandleRequest [url: {url}, method: {method}, login: {login}, status_code: {res.status_code}]]')

        except Exception as e:
            message = f'Error occured while FeAPI request [url: {url}, method: {method}, login: {login}]. Error: {str(e)}'
            print(message)
            self._log.error(message)

            raise message
            
        return res

    def __handle_response(self, res: Response) -> dict:
        if (len(res.content) == 0):
            return None
        return json.loads(res.content)

    def get(self, route: str, login: str = None) -> dict:
        response = self.__handle_request(method='GET', route=route, login=login)
        return self.__handle_response(response)
        content_json = json.loads(response.content)
        return content_json

    def post(self, route: str, data: dict, login: str = None) -> dict:
        response = self.__handle_request(method='POST', route=route, login=login, json=data)
        return self.__handle_response(response)
        content_json = json.loads(response.content)
        return content_json

    def put(self, route: str, data: dict, login: str = None) -> dict:
        response = self.__handle_request(method='PUT', route=route, login=login, json=data)
        return self.__handle_response(response)

    @staticmethod
    def get_cookies(login: str) -> cookies.RequestsCookieJar:

        if (login not in Base.__cookies_by_login):
            url = f'{config.fe_api_url}/users/signin'
            data = dict( login = login )
            response = request(url = url, method='POST', json = data)
            Base.__cookies_by_login[login] = response.cookies
        
        return Base.__cookies_by_login[login]
