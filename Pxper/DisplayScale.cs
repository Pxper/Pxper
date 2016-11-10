// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;

namespace Pxper
{
  public static class DisplayScale
  {
    private static float value;

    public static float Value
    {
      get
      {
        if (DisplayScale.value == 0f)
          DisplayScale.value = DisplayScale.GetDisplayScale();

        return DisplayScale.value;
      }
    }

    private static float GetDisplayScale()
    {
      Graphics g = Graphics.FromHwnd(IntPtr.Zero);
      IntPtr hDC = g.GetHdc();
      int logicalScreenHeight = Gdi32.GetDeviceCaps(hDC, (int)DeviceCap.VERTRES);
      int physicalScreenHeight = Gdi32.GetDeviceCaps(hDC, (int)DeviceCap.DESKTOPVERTRES);
      float displayScale = (float)physicalScreenHeight / (float)logicalScreenHeight;

      return displayScale;
    }
  }
}