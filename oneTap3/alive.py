# pip install pyautogui
import pyautogui
import pytweening
import sys
import random
import time


def fake_move():
    rpoints = []
    for i in range(2):
        rpoints.append([random.randint(-10, 10), random.randint(-5, 5)])

    for p in rpoints:
        pyautogui.moveRel(+p[0], +p[1],
                          tween=pytweening.easeInOutBack, duration=0.0)

    for p in rpoints:
        pyautogui.moveRel(-p[0], -p[1],
                          tween=pytweening.easeInOutBack, duration=0.0)


def do_loop():
    # time.sleep(60*20)
    # pyautogui.mouseDown()
    # fake_move()
    # pyautogui.mouseUp()
    # pyautogui.keyDown('prtscr')
    # time.sleep(0.5)
    # pyautogui.keyUp('prtscr')
    #time.sleep(3)
    pyautogui.moveTo(1920/2, 1080/2-100, 1, pytweening.easeInOutQuint, 1)
    #time.sleep(3)
    pyautogui.moveTo(1920/2-100, 1080/2, 1, pytweening.easeInOutQuint, 1)


while True:
    do_loop()
