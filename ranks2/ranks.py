import os
import json
import requests


def write_data(data):
    with open('ranks.json', 'w') as text_file:
        print(json.dumps(data, indent=4), file=text_file)


def read_data():
    with open('ranks.json', 'r') as text_file:
        return json.loads(text_file.read())


class Pohhop():
    name = 'pohhop'
    url = 'https://steamcommunity.com/profiles/000000'
    visible = True
    wingman = 0
    compet = 0

    def __init__(self, data=None):
        if data != None:
            self.name = data['name']
            self.url = data['url']
            self.visible = data['visible']
            self.wingman = data['wingman']
            self.compet = data['compet']

    def to_json(self):
        return {
            'name': self.name,
            'url': self.url,
            'visible': self.visible,
            'wingman': self.wingman,
            'compet': self.compet
        }

    def get_avatar(self):
        return os.path.join('avatars', self.url.split('/')[-1] + '.png')
    
    def download_avatar(self):
        if not os.path.exists(self.get_avatar()):
            for line in requests.get(self.url).text.splitlines():
                if 'public/images/avatars' in line:
                    avatar = requests.get(line[line.find('https'):line.find('.jpg') + 4])
                    open(self.get_avatar(), 'wb').write(avatar.content)
                    break


def parse_txt_to_json():
    data = []
    with open(os.path.join('..', 'ranks', 'ranks.txt'), 'r') as text_file:
        ranks_txt = text_file.read()
    for player in ranks_txt.split('`'):
        pdata = player.strip().split('\n')
        if pdata != ['']:
            p = Pohhop()
            p.name = pdata[0]
            p.wingman = pdata[1]
            p.compet = pdata[2]
            p.url = pdata[3]
            if '//' in p.wingman:
                p.wingman = p.wingman.replace('//', '')
                p.visible = False
            if '//' in p.compet:
                p.compet = p.compet.replace('//', '')
                p.visible = False
            p.wingman = int(p.wingman)
            p.compet = int(p.compet)
            data.append(p.to_json())
    write_data(data)


for data in read_data():
    player = Pohhop(data)
    player.download_avatar()

