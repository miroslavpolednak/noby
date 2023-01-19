import json
from requests import cookies, Response, request

from common import config

class Base():

    __default_login = '990614w' #!?
    __cookies_by_login: dict = dict()

    def __init__(self, route: str):
        assert route is not None, f'Route must be provided!'
        self.__route = route

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

            print(f'FeAPI.HandleRequest [url: {url}, method: {method}, login: {login}, status_code: {res.status_code}]]')

        except Exception as e:
            message = f'Error occured while FeAPI request [url: {url}, method: {method}, login: {login}]. Error: {str(e)}'
            print(message)
            raise message
            
        return res

    def get(self, route: str, login: str = None) -> dict:
        response = self.__handle_request(method='GET', route=route, login=login)
        content_json = json.loads(response.content)
        return content_json

    def post(self, route: str, data: dict, login: str = None) -> dict:
        response = self.__handle_request(method='POST', route=route, login=login, json=data)
        content_json = json.loads(response.content)
        return content_json

    @staticmethod
    def get_cookies(login: str) -> cookies.RequestsCookieJar:

        if (login not in Base.__cookies_by_login):
            url = f'{config.fe_api_url}/users/signin'
            data = dict( login = login )
            response = request(url = url, method='POST', json = data)
            Base.__cookies_by_login[login] = response.cookies
        
        return Base.__cookies_by_login[login]
