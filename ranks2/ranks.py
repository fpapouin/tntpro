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
    name = ''
    id = ''
    visible = True
    wingman = 0
    compet = 0

    def __init__(self, data=None):
        if data != None:
            self.name = data['name']
            self.id = data['id']
            self.visible = data['visible']
            self.wingman = data['wingman']
            self.compet = data['compet']

    def to_json(self):
        return {
            'name': self.name,
            'id': self.id,
            'visible': self.visible,
            'wingman': self.wingman,
            'compet': self.compet
        }
    
    def get_url(self):
        if self.id.isdigit():
            return f'https://steamcommunity.com/profiles/{self.id}'
        else:
            return f'https://steamcommunity.com/id/{self.id}'

    def get_avatar(self):
        return os.path.join('avatars', f'{self.id}.png')

    def get_compet(self):
        return os.path.join('skills', f'c{self.compet}.png')

    def get_wingman(self):
        return os.path.join('skills', f'w{self.wingman}.png')

    def download_avatar(self):
        if not os.path.exists(self.get_avatar()):
            for line in requests.get(self.get_url()).text.splitlines():
                if 'public/images/avatars' in line:
                    avatar = requests.get(line[line.find('https'):line.find('.jpg') + 4])
                    open(self.get_avatar(), 'wb').write(avatar.content)
                    break

    def update_name(self):
        for line in requests.get(self.get_url()).text.splitlines():
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
            p.id = pdata[3].strip('/').split('/')[-1]
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
        root.state('zoomed')
        self.load(root)
        root.mainloop()

    def load(self, root):
        accounts = []
        for data in read_data():
            accounts.append(Pohhop(data))
        accounts.sort(key=lambda x: (x.compet, x.wingman), reverse=True)
        x = 0
        y = 0
        position = 0
        delta = 0
        last_account = accounts[-1]
        for player in accounts:
            if not player.visible:
                continue
            if player.compet != last_account.compet:
                position += 1
                position += delta
                delta = 0
            elif player.wingman != last_account.wingman:
                position += 1
                position += delta
                delta = 0
            else:
                delta += 1
            last_account = player
            if position <= 35:
                pass
                #continue
            frame = Frame(root, background='Black')
            self.add_position(frame, position)
            self.add_avatar(frame, player.get_avatar())
            self.add_rank(frame, player.get_compet())
            self.add_rank(frame, player.get_wingman(), True)
            frame.grid(column=x, row=y, padx=5, pady=5)
            x += 1
            if x == 5:
                x = 0
                y += 2
            if y == 14:
                break

    def add_position(self, root, pos):
        import tkinter.font as font
        myFont = font.Font(size=35)
        foreColor = 'BlueViolet'
        if pos <= 40:
            foreColor = 'OrangeRed'
        if pos <= 30:
            foreColor = 'Green'
        if pos <= 15:
            foreColor = 'RoyalBlue'
        if pos <= 5:
            foreColor = 'Yellow'
        b = Button(root, text=str(pos), relief=FLAT, font=myFont, background='Black', foreground=foreColor, width=2)
        b.grid(column=0, rowspan=2, row=0)
        return b

    def add_avatar(self, root, img_path):
        from PIL import ImageTk, Image
        pil_image = Image.open(img_path).resize((120, 120), Image.ANTIALIAS)
        self.images.append(ImageTk.PhotoImage(pil_image))
        b = Button(root, image=self.images[-1], relief=FLAT, background='Black')
        b.grid(column=1, rowspan=2, row=0)
        return b

    def add_rank(self, root, img_path, is_wingman=False):
        from PIL import ImageTk, Image
        pil_image = Image.open(img_path).resize((150, 60), Image.ANTIALIAS)
        self.images.append(ImageTk.PhotoImage(pil_image))
        b = Button(root, image=self.images[-1], relief=FLAT, background='Black')
        if is_wingman:
            b.grid(column=2, row=1)
        else:
            b.grid(column=2, row=0)
        return b
        # c = tkinter.Canvas(root, image=img)


i = Ihm()