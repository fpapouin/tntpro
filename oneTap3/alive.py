# pip install pyautogui
import pyautogui, pytweening
import sys, random
import time


def fake_move():
    rpoints = []
    for i in range(2):
        rpoints.append([random.randint(-10, 10), random.randint(-5, 5)])

    for p in rpoints:
        pyautogui.moveRel(+p[0], +p[1], tween=pytweening.easeInOutBack, duration=0.0)

    for p in rpoints:
        pyautogui.moveRel(-p[0], -p[1], tween=pytweening.easeInOutBack, duration=0.0)


def do_loop():
    time.sleep(300)
    pyautogui.mouseDown()
    #fake_move()
    pyautogui.mouseUp()

while True:
    do_loop()
