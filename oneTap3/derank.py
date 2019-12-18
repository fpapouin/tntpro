import pyautogui
import win32api
import random
import pytweening

while True:
    if win32api.GetKeyState(0x01) & 0x80 == 128:
        if win32api.GetCursorPos()==(pyautogui.size().width/2, pyautogui.size().height/2):
            dist = 100
            #for i in range(2):
            x = random.randint(-dist, dist)
            y = random.randint(-dist, 0)
            pyautogui.moveRel(x, y, 0.01, tween=pytweening.easeOutElastic)