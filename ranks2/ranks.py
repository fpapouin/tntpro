import os
import json
import requests
import sys
from tkinter import Tk, Frame, Button, FLAT, Canvas, NW


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

    def __init__(self, pdata=None):
        if pdata != None:
            self.name = pdata['name']
            self.id = pdata['id']
            self.visible = pdata['visible']
            self.wingman = pdata['wingman']
            self.compet = pdata['compet']
            self.__avatar__ = None
            self.__wingman__ = None
            self.__compet__ = None

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

    def avatar_click(self):
        os.startfile(self.get_url())

    def compet_click(self, event):
        if event.num == 1:
            if self.compet != 18:
                self.compet += 1
        else:
            if self.compet != 0:
                self.compet -= 1
        c = event.widget
        from PIL import ImageTk, Image
        pil_image = Image.open(self.get_compet()).resize((150, 60), Image.ANTIALIAS)
        self.__compet__ = ImageTk.PhotoImage(pil_image)
        c.create_image(0, 0, anchor=NW, image=self.__compet__)

    def wingman_click(self, event):
        if event.num == 1:
            if self.wingman != 18:
                self.wingman += 1
        else:
            if self.wingman != 0:
                self.wingman -= 1
        c = event.widget
        from PIL import ImageTk, Image
        pil_image = Image.open(self.get_wingman()).resize((150, 60), Image.ANTIALIAS)
        self.__wingman__ = ImageTk.PhotoImage(pil_image)
        c.create_image(0, 0, anchor=NW, image=self.__wingman__)


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
            p.download_avatar()
            data.append(p.to_json())
    write_data(data)


def add_account(id):
    player = Pohhop()
    player.id = id
    player.download_avatar()
    player.update_name()
    data = read_data()
    data.append(player.to_json())
    write_data(data)


def update_ranks(player):
    data = read_data()
    for pdata in data:
        if pdata['id'] == player.id:
            pdata['compet'] = player.compet
            pdata['wingman'] = player.wingman
    write_data(data)


class Ihm():

    def __init__(self):
        self.root = Tk()
        self.root.title('Ranks')
        self.root['background'] = '#000000'
        self.root.state('zoomed')
        self.load()
        self.root.protocol("WM_DELETE_WINDOW", self.export)
        self.root.mainloop()

    def next(self):
        i = 1
        for key in self.root.children:
            if i <= 7*5:
                self.root.children[key].grid_remove()
            else:
                self.root.children[key].grid()
            i += 1

    def previous(self):
        i = 1
        for key in self.root.children:
            if i <= 7*5:
                self.root.children[key].grid()
            else:
                self.root.children[key].grid_remove()
            i += 1

    def load(self):
        self.accounts = []
        for pdata in read_data():
            self.accounts.append(Pohhop(pdata))
        self.accounts.sort(key=lambda x: (x.compet, x.wingman), reverse=True)
        x = 0
        y = 0
        position = 0
        delta = 0
        last_account = self.accounts[-1]
        for player in self.accounts:
            player.download_avatar()
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

            frame = Frame(self.root, background='Black')
            self.add_position(frame, position)
            self.add_avatar(frame, player)
            self.add_rank(frame, player)
            frame.grid(column=x, row=y, padx=20, pady=5)
            if y >= 7*2:
                pass
                frame.grid_remove()
            x += 1
            if x == 5:
                x = 0
                y += 2

    def add_position(self, root, pos):
        import tkinter.font as font
        myFont = font.Font(size=35)
        foreColor = 'BlueViolet'
        if pos <= 35:
            foreColor = 'OrangeRed'
        if pos <= 25:
            foreColor = 'Green'
        if pos <= 15:
            foreColor = 'RoyalBlue'
        if pos <= 5:
            foreColor = 'Yellow'
        b = Button(root, text=str(pos), relief=FLAT, font=myFont, background='Black', foreground=foreColor, width=2)
        b.grid(column=0, rowspan=2, row=0)
        b.bind('<Button-1>', self.position_click)
        b.bind('<Button-3>', self.position_click)

    def position_click(self, event):
        if event.num == 1:
            self.next()
        else:
            self.previous()

    def add_avatar(self, root, player):
        from PIL import ImageTk, Image
        pil_image = Image.open(player.get_avatar()).resize((120, 120), Image.ANTIALIAS)
        player.__avatar__ = ImageTk.PhotoImage(pil_image)
        b = Button(root, image=player.__avatar__, relief=FLAT, background='Black')
        b.grid(column=1, rowspan=2, row=0)
        b['command'] = player.avatar_click

    def add_rank(self, root, player):
        from PIL import ImageTk, Image
        pil_image = Image.open(player.get_compet()).resize((150, 60), Image.ANTIALIAS)
        player.__compet__ = ImageTk.PhotoImage(pil_image)
        c = Canvas(root, width=150, height=60, background='Black', borderwidth=0, highlightthickness=0, relief='ridge')
        c.create_image(0, 0, anchor=NW, image=player.__compet__)
        c.grid(column=2, row=0)
        c.bind('<Button-1>', player.compet_click)
        c.bind('<Button-3>', player.compet_click)
        pil_image = Image.open(player.get_wingman()).resize((150, 60), Image.ANTIALIAS)
        player.__wingman__ = ImageTk.PhotoImage(pil_image)
        c = Canvas(root, width=150, height=60, background='Black', borderwidth=0, highlightthickness=0, relief='ridge')
        c.create_image(0, 0, anchor=NW, image=player.__wingman__)
        c.grid(column=2, row=1)
        c.bind('<Button-1>', player.wingman_click)
        c.bind('<Button-3>', player.wingman_click)

    def export(self):
        for player in self.accounts:
            update_ranks(player)
        for key in self.root.children:
            self.root.children[key].grid_forget()
        self.root.children.clear()
        self.root.update()
        self.load()
        self.root.update()
        import win32gui
        from PIL import ImageGrab, Image
        handle = self.root.winfo_id()
        rect = win32gui.GetWindowRect(handle)
        self.previous()
        self.root.update()
        ranks1 = ImageGrab.grab(rect)
        self.next()
        self.root.update()
        ranks2 = ImageGrab.grab(rect)
        self.root.destroy()
        new_img = Image.new('RGB', (1920, 1100))
        new_img.paste(ranks1)
        new_img.paste(ranks2, (0, 950))
        new_img.save('ranks.png', 'png')


def upload(user, password):
    import ftplib
    session = ftplib.FTP('ftpperso.free.fr', user, password)
    with open('ranks.png', 'rb') as binary_file:
        session.storbinary('STOR ranks.png', binary_file)
    session.quit()

def update(token):
    import discord
    import uuid

    class MyClient(discord.Client):
        async def on_ready(self):
            print('Logged on as {0}!'.format(self.user))
            channel = self.get_channel(591959950322958337)
            #message = await channel.send('hello')
            message = await channel.fetch_message(705417121152237640)
            ranks_url = 'http://le.dahut.free.fr/ranks.png?' + str(uuid.uuid4())
            await message.edit(content=ranks_url)
            await self.close()
    client = MyClient()
    client.run(token)

def main():
    # parse_txt_to_json()
    if len(sys.argv) == 2:
        add_account(sys.argv[1])
    Ihm()
    if len(sys.argv) == 4:
        upload(sys.argv[1], sys.argv[2])
        update(sys.argv[3])


if __name__ == '__main__':
    main()
