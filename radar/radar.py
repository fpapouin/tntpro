from tkinter import Tk, Canvas
from math import cos, sin, pi
from typing import List


class datapoint:
    angle = 0
    weight = 1
    text = ''

    def __init__(self, text='', weight=1):
        self.weight = weight
        self.text = text


class Ihm():
    angles3 = [3, 7, 11]
    angles4 = [3, 6, 9, 12]
    angles5 = [3, 5, 8, 10, 13]
    angles6 = [3, 5, 7, 9, 11, 13]
    points: List[datapoint] = []
    type = 5
    english = False
    title = 'Title'

    def __init__(self):
        self.root = Tk()
        self.root.title('radar')
        self.root['background'] = 'black'
        self.root.state('zoomed')
        self.cxy = 500
        self.canvas = self.add_canvas()
        self.filldata()
        self.update_angle_and_draw()
        self.root.protocol("WM_DELETE_WINDOW", self.export)
        self.root.mainloop()

    def add_canvas(self):
        c = Canvas(self.root, width=1000, height=1000)
        c.grid()
        c.create_oval(self.cxy-300, self.cxy-300, self.cxy+300, self.cxy+300, width=2, outline='grey')
        c.create_oval(self.cxy-200, self.cxy-200, self.cxy+200, self.cxy+200, width=2, outline='grey')
        c.create_oval(self.cxy-100, self.cxy-100, self.cxy+100, self.cxy+100, width=2, outline='grey')
        return c

    def filldata(self):
        if self.type == 0:
            self.title = 'Test'
            self.points.append(datapoint('hello', 3))
            self.points.append(datapoint('sunny', 3))
            self.points.append(datapoint('world', 1))
        if self.type == 1:
            self.title = 'Langages'
            if self.english:
                self.title = 'Languages'
            self.points.append(datapoint('Python3', 3))
            self.points.append(datapoint('C#', 3))
            self.points.append(datapoint('Typescript', 1))
            self.points.append(datapoint('Pascal', 2))
            self.points.append(datapoint('Groovy', 1))
            self.points.append(datapoint('C++', 2))
        if self.type == 2:
            self.title = 'EDI'
            if self.english:
                self.title = 'IDE'
            self.points.append(datapoint('VSCode', 3))
            self.points.append(datapoint('Delphi', 2))
            self.points.append(datapoint('Visual Studio', 3))
            self.points.append(datapoint('TestStand', 1))
        if self.type == 3:
            self.title = 'Outils'
            if self.english:
                self.title = 'Tools'
            self.points.append(datapoint('Git', 3))
            self.points.append(datapoint('Svn', 3))
            self.points.append(datapoint('SQL', 1))
            self.points.append(datapoint('Gitlab-CI', 2))
            self.points.append(datapoint('Jenkins', 3))
        if self.type == 4:
            self.title = 'Humain'
            if self.english:
                self.title = 'Human'
                self.points.append(datapoint('Team work', 3))
                self.points.append(datapoint('Training', 3))
                self.points.append(datapoint('English', 3))
                self.points.append(datapoint('Agile Scrum', 2))
                self.points.append(datapoint('Autonomy', 3))
                self.points.append(datapoint('Remote', 3))
            else:
                self.points.append(datapoint('Travail en équipe', 3))
                self.points.append(datapoint('Formations', 3))
                self.points.append(datapoint('Anglais', 3))
                self.points.append(datapoint('Agile Scrum', 2))
                self.points.append(datapoint('Autonomie', 3))
                self.points.append(datapoint('Télétravail', 3))
        if self.type == 5:
            self.title = 'Passe-temps'
            if self.english:
                self.title = 'Hobbies'
                self.points.append(datapoint('Gaming', 3))
                self.points.append(datapoint('Sci-fi', 3))
                self.points.append(datapoint('Tennis', 1))
                self.points.append(datapoint('Fantasy', 2))
                self.points.append(datapoint('Cycling', 2))
            else:
                self.points.append(datapoint('Jeux-vidéos', 3))
                self.points.append(datapoint('Sci-fi', 3))
                self.points.append(datapoint('Tennis', 1))
                self.points.append(datapoint('Fantasy', 2))
                self.points.append(datapoint('Cyclisme', 2))

    def update_angle_and_draw(self):
        self.canvas.create_text(self.cxy, 32, text=self.title, font=("Arial", 48), fill='black')
        dp: datapoint
        i = 0
        array = []
        for dp in self.points:
            if len(self.points) == 3:
                dp.angle = self.angles3[i]
            if len(self.points) == 4:
                dp.angle = self.angles4[i]
            if len(self.points) == 5:
                dp.angle = self.angles5[i]
            if len(self.points) == 6:
                dp.angle = self.angles6[i]
            i += 1
            x = 100*dp.weight * cos(-dp.angle*pi/6) + self.cxy
            y = 100*dp.weight * sin(-dp.angle*pi/6) + self.cxy
            array.append(x)
            array.append(y)
            self.canvas.create_oval(x-10, y-10, x+10, y+10, outline='royal blue', fill='royal blue')
            x = (self.cxy-100) * cos(-dp.angle*pi/6) + self.cxy
            y = (self.cxy-100) * sin(-dp.angle*pi/6) + self.cxy
            self.canvas.create_text(x, y, text=dp.text, font=("Arial", 32), fill='royal blue')
        self.canvas.create_polygon(array, outline='royal blue', fill='', width=4, smooth=False)

    def export(self):
        import win32gui
        from PIL import ImageGrab, Image
        handle = self.root.winfo_id()
        rect = win32gui.GetWindowRect(handle)
        grab = ImageGrab.grab(rect)
        self.root.destroy()
        new_img = Image.new('RGB', (1000, 1000))
        new_img.paste(grab)
        new_img.save(f'{self.type}_{self.title}.png', 'png')


def main():
    Ihm()


if __name__ == '__main__':
    main()
