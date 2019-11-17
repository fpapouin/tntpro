import pyautogui
import win32api
import random
import pytweening

while True:
    if win32api.GetKeyState(0x01) & 0x80 == 128:
        dist = 30
        for i in range(2):
            x = random.randint(-dist, dist)
            y = random.randint(-dist, 0)
            pyautogui.moveRel(x, y, 0.01, tween=pytweening.easeOutElastic)