# pip install PIL
from PIL import ImageGrab, ImageOps
# pip install pyautogui
import pyautogui, pytweening
# pip install pypiwin32
import win32api
import sys, random
# this is a pointer to the module object instance itself.
this = sys.modules[__name__]

#time_interval_ms = 20
#min_elapsed_ms = 20

# must be odd
rec_size = 5
mouse_key = 0x05
debug = False
bbox = [(pyautogui.size().width - rec_size + 1)/2, (pyautogui.size().height - rec_size + 1)/2,
        (pyautogui.size().width + rec_size + 1)/2, (pyautogui.size().height + rec_size + 1)/2]
lastsum = 0
newsum = 0


def is_mouse_down():
    return win32api.GetKeyState(mouse_key) & 0x80 == 128


def is_cross(image):
    horiz_cross = []
    for i in range(rec_size):
        index = int(i + rec_size * (rec_size - 1) / 2)
        horiz_cross.append(image[index])
    if debug:
        print('horiz_cross = %s' % horiz_cross)
    return all(x == horiz_cross[0] for x in horiz_cross)


def ok_google():
    image = ImageOps.grayscale(ImageGrab.grab(bbox))
    if debug:
        image.save('OkGoogle.png')
    if not is_cross(image.im):
        return 0
    this.newsum = 0

    # Returns:	An unsorted list of (count, pixel) values.
    for pixel in image.getcolors(256):
        this.newsum += pixel[0]*pixel[1]
    if (this.newsum == 0) or (this.lastsum == 0):
        return 0
    diff = abs(this.newsum - this.lastsum)
    if debug:
        print('diff = %s' % diff)
    return diff

def fake_move():
    rpoints = []
    for i in range(2):
        rpoints.append([random.randint(-10, 10), random.randint(-5, 5)])

    for p in rpoints:
        pyautogui.moveRel(+p[0], +p[1], tween=pytweening.easeInOutBack, duration=0.0)

    for p in rpoints:
        pyautogui.moveRel(-p[0], -p[1], tween=pytweening.easeInOutBack, duration=0.0)


def do_loop():
    if is_mouse_down():
        if ok_google() > 150:
            pyautogui.click(clicks=2, interval=0.25)
            fake_move()

            if debug:
                print('ok_google')
            this.lastsum = 0
        else:
            this.lastsum = this.newsum
    else:
        this.lastsum = 0


while True:
    do_loop()
