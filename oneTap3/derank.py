import pyautogui
import win32api
import random
import pytweening
import time
import win32con

ok_cowboy = True
ok_jolly = False
derank = False

while True:
    if ok_cowboy:
        if win32api.GetKeyState(0x01) & 0x80 == 128:  # mouse1
            if win32api.GetCursorPos() == (pyautogui.size().width/2, pyautogui.size().height/2):
                #x = random.random()
                #x = x / 1.0
                pyautogui.keyDown('i')
                #time.sleep(x)
                pyautogui.keyUp('i')
                #time.sleep(x)

    if ok_jolly:
        if win32api.GetKeyState(win32con.VK_SPACE) & 0x80 == 128:  # space
            if win32api.GetCursorPos() == (pyautogui.size().width/2, pyautogui.size().height/2):
                pyautogui.keyDown('p')
                time.sleep(0.05)
                pyautogui.keyUp('p')
                time.sleep(0.05)

    if ok_derank:
        if win32api.GetKeyState(0x01) & 0x80 == 128:  # mouse1
            if win32api.GetCursorPos() == (pyautogui.size().width/2, pyautogui.size().height/2):
                dist = 100
                # for i in range(2):
                x = random.randint(-dist, dist)
                y = random.randint(-dist, 0)
                pyautogui.moveRel(x, y, 0.01, tween=pytweening.easeOutElastic)
