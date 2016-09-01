// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Pxper
{
  public class User32
	{
    [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref POINT pt);

    [DllImport("User32.dll", EntryPoint = "CreateWindowExW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public extern static IntPtr CreateWindow(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lParam);

    [DllImport("User32.dll")]
    public extern static IntPtr GetForegroundWindow();

    [DllImport("User32.dll")]
    public extern static UInt32 GetWindowLong(IntPtr hWnd, int dwStyle);

    [DllImport("User32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

    [DllImport("User32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(IntPtr hWnd, [In, Out] ref RECT rect);

    [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

    [DllImport("User32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("User32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool InvalidateRect(IntPtr hWnd, IntPtr rect, [MarshalAs(UnmanagedType.Bool)] bool erase);

    [DllImport("User32.dll")]
    public static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hDCDst, ref POINT pDst, ref SIZE pSize, IntPtr hDCSrc, ref POINT pSrc, int clrKey, ref WinGdi.BLENDFUNCTION blend, int dwFlags);

    [DllImport("User32.dll")]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

    [DllImport("User32.dll")]
    public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll")]
    public static extern bool UnhookWindowsHookEx(int idHook);
	}
}