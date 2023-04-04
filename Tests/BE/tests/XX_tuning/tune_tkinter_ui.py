import tkinter as tk
# from tkinter import *
# from tkinter import filedialog

window = tk.Tk()
window.title('Test DB viewer')
width, height = window.winfo_screenwidth()/1.5, window.winfo_screenheight()/1.5
window.geometry('%dx%d+0+0' % (width,height))
window.mainloop()