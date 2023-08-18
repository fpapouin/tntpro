from time import sleep
from PIL import ImageGrab, ImageOps
import win32api
from pynput.mouse import Button, Controller, Listener
display = (win32api.GetSystemMetrics(0), win32api.GetSystemMetrics(1))


def is_mouse_down():
    if win32api.GetCursorPos() == (display[0]/2, display[1]/2):
        return win32api.GetKeyState(0x05) & 0x80 == 128
    else:
        return False


class Onetap():
    def __init__(self):
        # Listener(on_move=self.on_move, on_click=self.on_click, on_scroll=self.on_scroll).start()
        # Listener(on_click=self.on_click).start()
        self.x1 = False
        self.shoot = False
        self.rec_size = 5  # must be odd
        self.bbox = [(display[0] - self.rec_size + 1)/2, (display[1] - self.rec_size + 1)/2,
                     (display[0] + self.rec_size + 1)/2, (display[1] + self.rec_size + 1)/2]
        self.lastsum = 0
        self.newsum = 0
        self.diff = 0
        self.mouse = Controller()

    def on_move(self, x, y):
        # print(f'{x=} {y=}')
        pass

    def on_click(self, x, y, button, pressed):
        # print(f'{x=} {y=} {button=} {pressed=}')
        if button == Button.x1:
            self.x1 = pressed

    def on_scroll(self, x, y, dx, dy):
        # print(f'{x=} {y=} {dx=} {dy=}')
        pass

    def scan(self):
        def is_cross(im):  # CSGO-rQvko-Lns4B-6LSLp-8MZji-T3veM
            horiz_cross = []
            for i in range(self.rec_size):
                index = int(i + self.rec_size * (self.rec_size - 1) / 2)
                horiz_cross.append(im[index])
            return all(x == horiz_cross[0] for x in horiz_cross)

        self.diff = 0
        self.newsum = 0
        image = ImageOps.grayscale(ImageGrab.grab(self.bbox))
        if is_cross(image.im):
            for pixel in image.getcolors(256):
                self.newsum += pixel[0]*pixel[1]
            if self.newsum != 0 and self.lastsum != 0:
                self.diff = abs(self.newsum - self.lastsum)


def main():
    ot = Onetap()
    while True:
        sleep(0.005)
        ot.x1 = is_mouse_down()
        if ot.x1:
            ot.scan()
            # if ot.diff > 50:
            if ot.diff > 25:
                ot.mouse.press(Button.left)
                ot.mouse.release(Button.left)
                ot.lastsum = 0
            else:
                ot.lastsum = ot.newsum
        else:
            ot.lastsum = 0


if __name__ == '__main__':
    main()
