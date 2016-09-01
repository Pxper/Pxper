// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Pxper
{
  [StructLayout(LayoutKind.Sequential)]
  public struct POINT
  {
    public int x;
    public int y;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct SIZE
  {
    public int cx;
    public int cy;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public override bool Equals(object obj)
    {
      RECT r = (RECT)obj;

      return (r.left == left && r.top == top && r.right == right && r.bottom == bottom);
    }

    public override int GetHashCode()
    {
      return ((left ^ top) ^ right) ^ bottom;
    }

    public static bool operator == (RECT a, RECT b)
    {
      return (a.left == b.left && a.top == b.top && a.right == b.right && a.bottom == b.bottom);
    }

    public static bool operator != (RECT a, RECT b)
    {
      return !(a == b);
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct COLORREF
  {
    public byte R;
    public byte G;
    public byte B;
  }
}