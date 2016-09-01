// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Pxper
{
  public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

  public class HookHelper
  {
    private int hKeyboardHook = 0;
    private HookProc keyboardHookProcedure;
    private int hMouseHook = 0;
    private HookProc mouseHookProcedure;

    public event EventHandler CtrlKeyDown;
    public event EventHandler CtrlKeyUp;
    public event MouseEventHandler MouseMove;
    public event MouseEventHandler MouseDown;
    public event MouseEventHandler MouseUp;
    public event MouseEventHandler MouseClick;

    public bool SetHook()
    {
      this.keyboardHookProcedure = new HookProc(this.KeyboardHookProc);
      this.hKeyboardHook = User32.SetWindowsHookEx(
        WinUser.WH_KEYBOARD_LL,
        this.keyboardHookProcedure,
        Process.GetCurrentProcess().MainModule.BaseAddress,
        0
      );

      this.mouseHookProcedure = new HookProc(this.MouseHookProc);
      this.hMouseHook = User32.SetWindowsHookEx(
        WinUser.WH_MOUSE_LL,
        this.mouseHookProcedure,
        Process.GetCurrentProcess().MainModule.BaseAddress,
        0
      );

      return this.hKeyboardHook != 0 && this.hMouseHook != 0;
    }

    public void ResetHook()
    {
      User32.UnhookWindowsHookEx(this.hKeyboardHook);
      User32.UnhookWindowsHookEx(this.hMouseHook);
    }

    private int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      { 
        WinUser.KeyboardHookStruct mouseHookStruct = (WinUser.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(WinUser.KeyboardHookStruct));
        Keys key = (Keys)mouseHookStruct.vkCode;

        if ((int)wParam == WinUser.WM_KEYDOWN && (key == Keys.LControlKey || key == Keys.RControlKey))
        {
          if (this.CtrlKeyDown != null)
            this.CtrlKeyDown(null, EventArgs.Empty);
        }

        else if ((int)wParam == WinUser.WM_KEYUP && (key == Keys.LControlKey || key == Keys.RControlKey))
        {
          if (this.CtrlKeyUp != null)
            this.CtrlKeyUp(null, EventArgs.Empty);
        }
      }

      return User32.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
    }

    private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        WinUser.MouseHookStruct mouseHookStruct = (WinUser.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(WinUser.MouseHookStruct));
        int x = mouseHookStruct.pt.x;
        int y = mouseHookStruct.pt.y;
        int delta = 0;

        if ((int)wParam == WinUser.WM_MOUSEMOVE)
        {
          if (this.MouseMove != null)
            this.MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_LBUTTONDOWN)
        {
          if (this.MouseDown != null)
            this.MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_RBUTTONDOWN)
        {
          if (this.MouseDown != null)
            this.MouseDown(null, new MouseEventArgs(MouseButtons.Right, 1, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_LBUTTONUP)
        {
          if (this.MouseUp != null)
            this.MouseUp(null, new MouseEventArgs(MouseButtons.Left, 1, x, y, delta));

          if (this.MouseClick != null)
            this.MouseClick(null, new MouseEventArgs(MouseButtons.Left, 1, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_RBUTTONUP)
        {
          if (this.MouseUp != null)
            this.MouseUp(null, new MouseEventArgs(MouseButtons.Right, 1, x, y, delta));

          if (this.MouseClick != null)
            this.MouseClick(null, new MouseEventArgs(MouseButtons.Right, 1, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_LBUTTONDBLCLK)
        {
          if (this.MouseClick != null)
            this.MouseClick(null, new MouseEventArgs(MouseButtons.Left, 2, x, y, delta));
        }

        else if ((int)wParam == WinUser.WM_RBUTTONDBLCLK)
        {
          if (this.MouseClick != null)
            this.MouseClick(null, new MouseEventArgs(MouseButtons.Right, 2, x, y, delta));
        }
      }

      return User32.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
    }
  }
}