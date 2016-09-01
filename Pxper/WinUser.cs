// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Pxper
{
  public class WinUser
	{
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;

    public const int LWA_COLORKEY = 0x00000001;
    public const int LWA_ALPHA = 0x00000002;

    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;

    public const int SWP_NOSIZE = 1;
    public const int SWP_NOMOVE = 2;
    public const int SWP_NOACTIVATE = 0x10;

    public const int ULW_COLORKEY = 1;
    public const int ULW_ALPHA = 2;
    public const int ULW_OPAQUE = 4;

    public const int USER_TIMER_MINIMUM = 0x0000000A;

    public const int WH_KEYBOARD_LL = 13;

    public const int WM_KEYDOWN = 0x100;
    public const int WM_KEYUP = 0x101;
    public const int WM_SYSKEYDOWN = 0x104;
    public const int WM_SYSKEYUP = 0x105;

    public const int WH_MOUSE_LL = 14;

    public const int WM_MOUSEMOVE = 0x200;
    public const int WM_LBUTTONDOWN = 0x201;
    public const int WM_RBUTTONDOWN = 0x204;
    public const int WM_LBUTTONUP = 0x202;
    public const int WM_RBUTTONUP = 0x205;
    public const int WM_LBUTTONDBLCLK = 0x203;
    public const int WM_RBUTTONDBLCLK = 0x206;

    public const int WS_VISIBLE = 0x10000000;
    public const int WS_CHILD = 0x40000000;

    public const int WS_EX_TOOLWINDOW = 0x00000080;
		public const int WS_EX_LAYERED = 0x80000;
		public const int WS_EX_TRANSPARENT = 0x20;
		public const int WS_EX_TOPMOST = 0x8;
    public const int WS_EX_NOACTIVATE = 0x08000000;

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardHookStruct
    {
      public int vkCode;
      public int scanCode;
      public int flags;
      public int time;
      public int dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
      public POINT pt;
      public int hWnd;
      public int wHitTestCode;
      public int dwExtraInfo;
    }
	}
}