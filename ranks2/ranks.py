import os
import json
import requests
from tkinter import *

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
    
    def get_compet(self):
        return os.path.join('skills', f'c{self.compet}.png')

    def get_wingman(self):
        return os.path.join('skills', f'w{self.wingman}.png')

    def download_avatar(self):
        if not os.path.exists(self.get_avatar()):
            for line in requests.get(self.url).text.splitlines():
                if 'public/images/avatars' in line:
                    avatar = requests.get(line[line.find('https'):line.find('.jpg') + 4])
                    open(self.get_avatar(), 'wb').write(avatar.content)
                    break

    def update_name(self):
        for line in requests.get(self.url).text.splitlines():
            if 'actual_persona_name' in line:
                self.name = line[line.find('">') + 2:line.find('</')]
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


def update_avatar_and_name():
    new_data = []
    for data in read_data():
        player = Pohhop(data)
        player.download_avatar()
        player.update_name()
        new_data.append(player.to_json())
    write_data(new_data)

class Ihm():
    images = []
    def __init__(self):
        root = Tk()
        root.title('Ranks')
        root['background'] = '#000000'
        # root.state('zoomed')
        #sb = Scrollbar(root)
        #sb.pack(side = RIGHT, fill = Y)
        self.load(root)
        root.mainloop()

    def load(self, root):
        x = 0
        y = 0
        for data in read_data():
            player = Pohhop(data)
            if not player.visible:
                continue
            self.add_avatar(player.get_avatar(), x, y)
            x+=1
            self.add_rank(player.get_compet(), x, y)
            y+=1
            self.add_rank(player.get_wingman(), x, y)
            y-=1
            x+=1
            if x == 12:
                x=0
                y+=2

        #self.add_button(os.path.join('skills', 'c0.png'))
        #self.add_button(os.path.join('avatars', '76561197960306957.png'))
    
    def add_avatar(self, img_path, x, y):
        from PIL import ImageTk, Image
        pil_image = Image.open(img_path).resize((120, 120), Image.ANTIALIAS)
        self.images.append(ImageTk.PhotoImage(pil_image))
        b = Button(image = self.images[-1])
        b['background'] = '#000000'
        b.grid(column=x, row=y, rowspan=2)
        return b

    def add_rank(self, img_path, x, y):
        from PIL import ImageTk, Image
        pil_image = Image.open(img_path).resize((150, 60), Image.ANTIALIAS)
        self.images.append(ImageTk.PhotoImage(pil_image))
        b = Button(image = self.images[-1])
        b['background'] = '#000000'
        b.grid(column=x, row=y)
        return b
        #c = tkinter.Canvas(root, image=img)

i = Ihm()
