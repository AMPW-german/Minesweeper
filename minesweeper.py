import random
import tkinter as tk
import json
import time
import struct

def left(btn_num):
    global recived
    global btns
    btns[btn_num]["state"] = "disabled"
    print(btn_num)
    recived = "    " + str(btn_num)
    s = recived.encode('ascii')
    
    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary
    print(s)

    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    recived = s
    f.seek(0)

def right(btn_num):
    print("right")
    return True

f = open(r'\\.\pipe\NPtest', 'r+b', 0)
i = 1

master=tk.Tk()
master.title("Minesweeper")
master.geometry("350x275")

btn_nr = -1
btns = []

s = 'Message[{0}]'.format(i).encode('ascii')

f.write(struct.pack('I', len(s)) + s)   # Write str length and str
f.seek(0)                               # EDIT: This is also necessary
print(s)

n = struct.unpack('I', f.read(4))[0]    # Read str length
s = f.read(n).decode('ascii')           # Read str
recived = s
f.seek(0)                               # Important!!!
s = json.loads(s)
print(s)
print(type(s))

boardsize = s["Boardsize"]

for x in range(boardsize[0]):
    for y in range(boardsize[1]):
        btn_nr += 1
        print(btn_nr)

        btns.append(tk.Button(text=s[, height=1, width=1))

        btns[btn_nr].grid(row=x, column=y)
        btns[btn_nr].bind("<Button-1>", lambda e,c=btn_nr:left(c))
        btns[btn_nr].bind("<Button-2>", lambda e,c=btn_nr:right(c))
        btns[btn_nr].bind("<Button-3>", lambda e,c=btn_nr:right(c))

master.mainloop()


while True:
    i += 1
    if i >= 5:
        recived = "    " + recived
        s = recived.encode('ascii')
    else:
        s = 'Message[{0}]'.format(i).encode('ascii')

    f.write(struct.pack('I', len(s)) + s)   # Write str length and str
    f.seek(0)                               # EDIT: This is also necessary
    print(s)

    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')           # Read str
    recived = s
    f.seek(0)                               # Important!!!
    print(s)
        # n = struct.unpack('I', f.read(4))[0]    # Read str length
        # s = f.read(n).decode('ascii')           # Read str
        # f.seek(0)                               
        # print ('Read:', s)
        # s="Completed- "+s
        # f.write(struct.pack('I', len(s)) + s.encode('ascii'))   # Write str length and str
        # f.seek(0)                            
        # print ('Wrote:', s)       


