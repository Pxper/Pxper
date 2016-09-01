// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Pxper
{
  public class Magnification
  {
    public const string WC_MAGNIFIER = "Magnifier";

    public static IntPtr HWND_TOPMOST = new IntPtr(-1);

    public enum MagnifierStyle : int
    {
      MS_SHOWMAGNIFIEDCURSOR = 0x0001,
      MS_CLIPAROUNDCURSOR = 0x0002,
      MS_INVERTCOLORS = 0x0004
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Transformation
    {
      public float m00;
      public float m10;
      public float m20;
      public float m01;
      public float m11;
      public float m21;
      public float m02;
      public float m12;
      public float m22;

      public Transformation(float magnificationFactor)
        : this()
      {
        m00 = magnificationFactor;
        m11 = magnificationFactor;
        m22 = 1.0f;
      }
    }

    [DllImport("Magnification.dll", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MagInitialize();

    [DllImport("Magnification.dll", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MagUninitialize();

    [DllImport("Magnification.dll", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MagSetWindowSource(IntPtr hwnd, RECT rect);

    [DllImport("Magnification.dll", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MagSetWindowTransform(IntPtr hwnd, ref Magnification.Transformation pTransform);
  }
}